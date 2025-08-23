namespace AppSettingsStudio.Configuration;

public partial class SettingsConfigurationProvider : ConfigurationProvider, IDisposable
{
    private IDisposable? _changeToken;

    public SettingsConfigurationProvider(SettingsConfigurationSource source)
    {
        ArgumentNullException.ThrowIfNull(source);
        var rootPath = source.RootPath.Nullify();
        if (rootPath != null && !Path.IsPathRooted(rootPath))
            throw new ArgumentException(null, nameof(source));

        RootPath = rootPath.Nullify() ?? Manager.GetDefaultRootPath();
        var applicationName = (source.ApplicationName.Nullify() ?? Path.GetFileNameWithoutExtension(Environment.ProcessPath)) ?? throw new ArgumentException(null, nameof(source));
        ApplicationName = applicationName;
        Manager = new Manager();
        Options = source.Options;
        if (Options.HasFlag(SettingsOptions.GatherAppSettingsFile))
        {
            Manager.GatherAppSettingsFile(RootPath, AppContext.BaseDirectory, ApplicationName);
        }

        if (Options.HasFlag(SettingsOptions.MonitorChanges))
        {
            var filePath = GetFilePath();
            if (filePath != null)
            {
                MonitorChanges(filePath);
            }
        }
    }

    public Manager Manager { get; }
    public SettingsOptions Options { get; }
    public string RootPath { get; }
    public string ApplicationName { get; }

    // append all keys
    public override void Set(string key, string? value) => base.Set($"{Manager.DirectoryName}:{key}", value);

    public virtual void MonitorChanges(string filePath)
    {
        ArgumentNullException.ThrowIfNull(filePath);
        var dir = Path.GetDirectoryName(filePath);
        // null or empty dir may happend when mixing wsl & windows paths & links
        if (string.IsNullOrWhiteSpace(dir) || !Directory.Exists(dir))
            return;

        var fileProvider = new PhysicalFileProvider(dir);
        _changeToken = ChangeToken.OnChange(
            () => fileProvider.Watch("*.json"),
            () =>
            {
                Load();
                OnReload();
            });
    }

    private string? GetFilePath()
    {
        var appSettings = Manager.GetInstance(RootPath, AppContext.BaseDirectory, ApplicationName)?.ActiveAppSettings;
        if (appSettings != null)
            return appSettings.LinkedFilePath ?? appSettings.FilePath;

        return null;
    }

    public override void Load()
    {
        base.Load();
        var filePath = GetFilePath();
        if (filePath != null && File.Exists(filePath))
        {
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var provider = new JsonStreamConfigurationProvider(new JsonStreamConfigurationSource());
            provider.Load(stream);

            // bit of a hack but Data is protected (not fully private) and so we can reuse json provider workings
            Data = (IDictionary<string, string?>)provider.GetType().GetProperty(nameof(Data), BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(provider)!;

            // replace manager's variables and env variables
            foreach (var kv in Data)
            {
                if (kv.Value != null)
                {
                    var value = kv.Value;
                    if (Manager.Variables.Count > 0)
                    {
                        var vregex = VariableRegex();
                        value = vregex.Replace(value, m =>
                        {
                            var name = m.Groups["v"].Value;
                            if (Manager.Variables.TryGetValue(name, out var variable))
                                return variable.Value;

                            return string.Empty;
                        });
                    }

                    var eregex = EnvVariableRegex();
                    value = eregex.Replace(value, m =>
                    {
                        var name = m.Groups["v"].Value;
                        return Environment.GetEnvironmentVariable(name) ?? string.Empty;
                    });

                    if (value != kv.Value)
                    {
                        Data[kv.Key] = value;
                    }
                }
            }
        }

        Set(nameof(RootPath), RootPath);
    }

    public void Dispose() { Dispose(disposing: true); GC.SuppressFinalize(this); }
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _changeToken?.Dispose();
        }
    }

    // only used by appsettings studio's app
    public static string? GetRootPath(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        return configuration.GetValue<string>($"{Manager.DirectoryName}:{nameof(RootPath)}");
    }

    // env format $(VARIABLE)
    [GeneratedRegex(@"\$\((?<v>[\s\S]*?)\)", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.ExplicitCapture)]
    private static partial Regex EnvVariableRegex();

    // manager's variable format @(VARIABLE)
    [GeneratedRegex(@"@\((?<v>[\s\S]*?)\)", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.ExplicitCapture)]
    private static partial Regex VariableRegex();
}
