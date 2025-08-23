namespace AppSettingsStudio.Configuration;

public class Instance(App app, string directoryPath, string appSettingsPath) : IWithFilePath
{
    internal const string _linkName = "link";
    internal const string _wslServerName = @"\\wsl.localhost\";

    [Browsable(false)]
    public App App { get; } = app;

    [ReadOnly(true)]
    [DisplayName("App Settings Path")]
    public virtual string AppSettingsPath { get; set; } = appSettingsPath;

    [Browsable(false)]
    public string AppSettingsDirectoryPath => Path.GetDirectoryName(AppSettingsPath)!;

    [Browsable(false)]
    public virtual IList<AppSettings> AppSettings { get; } = [];

    [Browsable(false)]
    public AppSettings? ActiveAppSettings => AppSettings.FirstOrDefault(a => a.IsActive);

    [Browsable(false)]
    public bool HasUnlinkedAppSettings => AppSettings.Any(i => !i.IsLink);

    [ReadOnly(true)]
    [DisplayName("Display Name")]
    public string? DisplayName { get; set; }

    [DisplayName("Directory Path")]
    public string DirectoryPath { get; } = directoryPath;

    [DisplayName("WSL Directory Path")]
    public string? WslDirectoryPath { get; } = GetWslDirectoryPath(directoryPath);

    private static string? GetWslDirectoryPath(string path)
    {
        if (!path.StartsWith(_wslServerName, StringComparison.OrdinalIgnoreCase))
            return null;

        return path[_wslServerName.Length..];
    }

    [DisplayName("Is WSL")]
    public bool IsWsl => DirectoryPath.StartsWith(_wslServerName, StringComparison.OrdinalIgnoreCase);
    public bool Exists => File.Exists(AppSettingsPath);

    string? IWithFilePath.FilePath => AppSettingsPath;
    bool IWithFilePath.IsReadOnly => true;

    public override string ToString() => $"{App} {Path.GetFileName(DirectoryPath)} {AppSettingsPath}";

    public virtual bool RenameAppSettings(AppSettings appSettings, string newName)
    {
        ArgumentNullException.ThrowIfNull(appSettings);
        ArgumentNullException.ThrowIfNull(newName);
        var index = AppSettings.IndexOf(appSettings);
        if (index < 0)
            return false;

        if (appSettings.IsLink)
            return false;

        if (!newName.StartsWith("appsettings", StringComparison.OrdinalIgnoreCase) || !newName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            return false;

        if (newName.EqualsIgnoreCase(appSettings.Name))
            return false;

        var newFilePath = Path.Combine(Path.GetDirectoryName(appSettings.FilePath)!, newName);
        File.Move(appSettings.FilePath, newFilePath);
        var newAppSettings = App.Manager.CreateAppSettings(this, newFilePath);
        AppSettings.Remove(appSettings);
        AppSettings.Insert(index, newAppSettings);
        return true;
    }

    public virtual AppSettings? DeleteAppSettings(AppSettings appSettings)
    {
        ArgumentNullException.ThrowIfNull(appSettings);
        var index = AppSettings.IndexOf(appSettings);
        if (index < 0)
            return null;

        var links = appSettings.GetLinksTo();
        foreach (var link in links)
        {
            link.Instance.DeleteAppSettings(link);
        }

        AppSettings? newAppSettings = null;
        if (AppSettings.Count > 1)
        {
            if (index == 0)
            {
                newAppSettings = AppSettings[1];
            }
            else
            {
                newAppSettings = AppSettings[index - 1];
            }
            ActivateAppSettings(newAppSettings);
        }

        Utilities.MoveToRecycleBin(appSettings.FilePath);
        AppSettings.RemoveAt(index);
        Save();
        return newAppSettings;
    }

    public virtual bool CanLinkToAppSettings(AppSettings appSettings)
    {
        ArgumentNullException.ThrowIfNull(appSettings);
        return IsWsl == appSettings.Instance.IsWsl;
    }

    public virtual AppSettings LinkToAppSettings(AppSettings appSettings)
    {
        ArgumentNullException.ThrowIfNull(appSettings);
        if (!CanLinkToAppSettings(appSettings))
            throw new InvalidOperationException("Links cannot be established between Unix paths with Windows paths.");

        var existing = AppSettings.FirstOrDefault(a => a.LinkedFilePath.EqualsIgnoreCase(appSettings.FilePath));
        if (existing != null)
            return existing;

        var id = Guid.NewGuid().ToString("N");
        var filePath = Path.Combine(DirectoryPath, $"link.{id}.json");

        var dic = new Dictionary<string, object?>(StringComparer.Ordinal)
        {
            [_linkName] = appSettings.FilePath
        };

        Utilities.SaveDictionaryAsJson(dic, filePath);

        var link = App.Manager.CreateAppSettings(this, filePath);
        link.LinkedFilePath = appSettings.FilePath;
        return link;
    }

    protected virtual string GetAppSettingsPath()
    {
        var path = AppSettingsPath;
        if (!Path.IsPathFullyQualified(path))
        {
            // when DirectoryPath = \\wsl.localhost\Ubuntu\etc.
            if (IsWsl)
            {
                var split = AppSettingsPath.Split('/');
                // format is /mnt/d/path...
                if (split.Length > 2 && split[0] == string.Empty && split[1] == "mnt")
                    return $"{split[2]}:\\{string.Join('\\', split.Skip(3))}";
            }

            var root = Path.GetPathRoot(DirectoryPath);
            if (root != null)
            {
                if (path.StartsWith('/') || path.StartsWith('\\'))
                {
                    path = path[1..];
                }
                return Path.Combine(root, path);
            }
        }
        return path;
    }

    public virtual AppSettings AddAppSettings(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        var filePath = Path.Combine(DirectoryPath, name);
        var appSettings = AppSettings.FirstOrDefault(a => a.FilePath.EqualsIgnoreCase(filePath));
        if (appSettings == null)
        {
            Utilities.PathEnsureDirectory(filePath);
            var path = GetAppSettingsPath();
            File.Copy(path, filePath, true);
            appSettings = App.Manager.CreateAppSettings(this, filePath);
            AppSettings.Add(appSettings);
        }
        return appSettings;
    }

    public virtual void ActivateAppSettings(AppSettings appSettings)
    {
        ArgumentNullException.ThrowIfNull(appSettings);
        foreach (var appSetting in AppSettings)
        {
            appSetting.IsActive = false;
        }
        appSettings.IsActive = true;
    }

    public virtual void Save() => App.Manager.SaveInstance(this);
}
