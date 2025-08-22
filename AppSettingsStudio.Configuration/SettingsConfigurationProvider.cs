namespace AppSettingsStudio.Configuration;

public partial class SettingsConfigurationProvider : ConfigurationProvider
{
    public SettingsConfigurationProvider(SettingsConfigurationSource source)
    {
        ArgumentNullException.ThrowIfNull(source);
        var rootPath = source.RootPath.Nullify();
        if (rootPath != null && !Path.IsPathRooted(rootPath))
            throw new ArgumentException(null, nameof(source));

        RootPath = rootPath.Nullify() ?? Manager.GetDefaultRootPath();
        var applicationName = source.ApplicationName.Nullify() ?? Path.GetFileNameWithoutExtension(Environment.ProcessPath);
        if (applicationName == null)
            throw new ArgumentException(null, nameof(source));

        ApplicationName = applicationName;
        Manager = new Manager();
        if (source.Options.HasFlag(SettingsOptions.GatherAppSettingsFile))
        {
            Manager.GatherAppSettingsFile(RootPath, AppContext.BaseDirectory, ApplicationName);
        }
    }

    public Manager Manager { get; }
    public string RootPath { get; }
    public string ApplicationName { get; }

    // append all keys
    public override void Set(string key, string? value) => base.Set($"{Manager.DirectoryName}:{key}", value);

    public override void Load()
    {
        base.Load();
        var appSettings = Manager.GetInstance(RootPath, AppContext.BaseDirectory, ApplicationName)?.ActiveAppSettings;
        if (appSettings != null)
        {
            var filePath = appSettings.LinkedFilePath ?? appSettings.FilePath;
            if (File.Exists(filePath))
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
        }

        Set(nameof(RootPath), RootPath);
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
