namespace AppSettingsStudio.Configuration;

public class Manager
{
    public const string DirectoryName = ".AppSettingsStudio";

    public const string MetadataFileName = "props.json";
    public const string VariablesFileName = "variables.json";
    public const string InstanceFileName = "instance.json";

    public const string AppSettingsFileName = "appsettings.json";

    private const string _appSettingsPath = "appSettingsPath";
    private const string _appSettingsHash = "appSettingsHash";
    private const string _activeAppSettings = "activeAppSettings";

    public virtual IDictionary<string, App> Apps { get; } = new Dictionary<string, App>(StringComparer.OrdinalIgnoreCase);
    public static IDictionary<string, SettingsVariable> Variables { get; } = new Dictionary<string, SettingsVariable>(StringComparer.OrdinalIgnoreCase);

    protected virtual internal App CreateApp(string name) => new(this, name);
    protected virtual internal AppSettings CreateAppSettings(Instance instance, string filePath) => new(instance, filePath);
    protected virtual internal Instance CreateInstance(App app, string directoryPath, string appSettingsPath) => new(app, directoryPath, appSettingsPath);

    public static string ExportAsZipFile(string targetDirectoryPath, IEnumerable<string> rootPaths, CompressionLevel compressionLevel = CompressionLevel.Optimal, Encoding? entryNameEncoding = null)
    {
        ArgumentNullException.ThrowIfNull(targetDirectoryPath);
        ArgumentNullException.ThrowIfNull(rootPaths);
        if (!Path.IsPathRooted(targetDirectoryPath))
            throw new ArgumentException(null, nameof(targetDirectoryPath));

        foreach (var path in rootPaths)
        {
            if (Utilities.PathIsChild(path, targetDirectoryPath))
                throw new ArgumentException($"Target path '{targetDirectoryPath}' cannot contain root path '{path}'.", nameof(targetDirectoryPath));
        }

        var name = DateTime.Now;
        var zipFilePath = Path.Combine(targetDirectoryPath, $"Backup{DirectoryName}{name:yyyy-MM-dd-HH-mm-ss}.zip");
        using var zip = ZipFile.Open(zipFilePath, ZipArchiveMode.Create, entryNameEncoding);
        foreach (var path in rootPaths)
        {
            zip.AddFromDirectory(path, true, null, compressionLevel);
        }
        return zipFilePath;
    }

    protected internal virtual void SaveInstance(Instance instance)
    {
        ArgumentNullException.ThrowIfNull(instance);
        var dic = new Dictionary<string, object?>
        {
            [_activeAppSettings] = instance.ActiveAppSettings?.Name
        };

        var filePath = Path.Combine(instance.DirectoryPath, InstanceFileName);
        Utilities.SaveDictionaryAsJson(dic, filePath);
    }

    protected virtual void LoadInstance(Instance instance)
    {
        ArgumentNullException.ThrowIfNull(instance);
        if (Directory.Exists(instance.DirectoryPath))
        {
            foreach (var file in Directory.EnumerateFiles(instance.DirectoryPath, "appsettings*.json"))
            {
                var settings = CreateAppSettings(instance, file);
                instance.AppSettings.Add(settings);
            }

            foreach (var file in Directory.EnumerateFiles(instance.DirectoryPath, "link.*.json"))
            {
                var link = Utilities.LoadJsonAsDictionary(file);
                var linkedFilePath = link.GetNullifiedString(Instance._linkName);
                if (linkedFilePath != null)
                {
                    var settings = CreateAppSettings(instance, file);
                    instance.AppSettings.Add(settings);
                    settings.LinkedFilePath = linkedFilePath;
                }
            }
        }

        if (instance.AppSettings.Count > 0)
        {
            instance.AppSettings[0].IsActive = true;
        }

        var props = Path.Combine(instance.DirectoryPath, InstanceFileName);
        var dic = Utilities.LoadJsonAsDictionary(props);

        // determine active app settings
        if (instance.AppSettings.Count > 1)
        {
            var def = dic.GetNullifiedString(_activeAppSettings);
            if (def != null)
            {
                var defApp = instance.AppSettings.FirstOrDefault(a => a.Name.EqualsIgnoreCase(def));
                if (defApp != null)
                {
                    instance.ActivateAppSettings(defApp);
                }
            }
        }
    }

    public virtual void Clear()
    {
        Apps.Clear();
    }

    // used by client
    public App? GetApp(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        _ = Apps.TryGetValue(name, out var app);
        return app;
    }

    // used by client
    public virtual bool Load(string directoryPath)
    {
        ArgumentNullException.ThrowIfNull(directoryPath);
        if (!Directory.Exists(directoryPath))
            return false;

        var changed = false;
        foreach (var dir in Directory.EnumerateDirectories(directoryPath))
        {
            var name = Path.GetFileName(dir);
            if (!Apps.TryGetValue(name, out var app))
            {
                app = CreateApp(name);
                Apps[name] = app;
                changed = true;
            }

            // subdir are hashes of paths
            foreach (var hash in Directory.EnumerateDirectories(dir))
            {
                var md = Path.Combine(hash, MetadataFileName);
                var metaData = Utilities.LoadJsonAsDictionary(md);
                var appSettingsPath = metaData.GetNullifiedString(_appSettingsPath);
                if (appSettingsPath == null)
                    continue;

                if (!app.Instances.TryGetValue(appSettingsPath, out var instance))
                {
                    instance = CreateInstance(app, hash, appSettingsPath);
                    LoadInstance(instance);
                    app.Instances[appSettingsPath] = instance;
                    changed = true;
                }
            }

            app.MakeInstanceUniqueNames();
        }

        if (LoadVariables())
        {
            changed = true;
        }

        return changed;
    }

    // used by server-side
    public virtual Instance GetInstance(string directoryPath, string applicationDirectoryPath, string applicationName)
    {
        ArgumentNullException.ThrowIfNull(directoryPath);
        ArgumentNullException.ThrowIfNull(applicationDirectoryPath);
        ArgumentNullException.ThrowIfNull(applicationName);

        LoadVariables();
        var source = Path.Combine(applicationDirectoryPath, AppSettingsFileName);
        var instanceHash = source.ToLowerInvariant().ComputeHashString();
        var app = CreateApp(applicationName);
        var instance = CreateInstance(app, Path.Combine(directoryPath, applicationName, instanceHash), source);
        LoadInstance(instance);
        app.Instances[instance.AppSettingsPath] = instance; // just to be consistent
        return instance;
    }

    // used by server-side
    public virtual void GatherAppSettingsFile(string directoryPath, string applicationDirectoryPath, string applicationName)
    {
        ArgumentNullException.ThrowIfNull(directoryPath);
        ArgumentNullException.ThrowIfNull(applicationDirectoryPath);
        ArgumentNullException.ThrowIfNull(applicationName);

        var source = Path.Combine(applicationDirectoryPath, AppSettingsFileName);
        if (!File.Exists(source))
            return;

        var instanceHash = source.ToLowerInvariant().ComputeHashString(); // appsettings.json full path (local to a machine) represents one instance of this app
        var bytes = File.ReadAllBytes(source);
        var fileHash = new Guid(XxHash128.Hash(bytes)).ToString("N");
        var target = Path.Combine(directoryPath, applicationName, instanceHash, MetadataFileName);
        var dic = Utilities.LoadJsonAsDictionary(target);
        var prevFileHash = dic.GetNullifiedString(_appSettingsHash);
        if (prevFileHash == null || prevFileHash != fileHash)
        {
            dic[_appSettingsPath] = source;
            dic[_appSettingsHash] = fileHash;
            dic["DateTime"] = DateTime.Now;

            Utilities.SaveDictionaryAsJson(dic, target);
        }
    }

    public static string GetDefaultRootPath() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), DirectoryName);
    public static void ClearVariables() => Variables.Clear();
    public static void SaveVariables()
    {
        var dic = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        foreach (var kv in Variables)
        {
            dic[kv.Key] = kv.Value.Value;
        }

        var rootPath = GetDefaultRootPath();
        var variablesPath = Path.Combine(rootPath, VariablesFileName);
        Utilities.SaveDictionaryAsJson(dic, variablesPath);
    }

    public static bool LoadVariables()
    {
        var changed = false;
        var rootPath = GetDefaultRootPath();
        var variablesPath = Path.Combine(rootPath, VariablesFileName);

        var dic = Utilities.LoadJsonAsDictionary(variablesPath);
        // use only dictionary's first level
        foreach (var kv in dic)
        {
            var value = string.Format(CultureInfo.InvariantCulture, "{0}", kv.Value);
            if (!Variables.TryGetValue(kv.Key, out var variable))
            {
                variable = new SettingsVariable(kv.Key, value);
                Variables[kv.Key] = variable;
                changed = true;
            }
            else
            {
                if (value != variable.Value)
                {
                    variable.Value = value;
                    changed = true;
                }
            }
        }
        return changed;
    }
}
