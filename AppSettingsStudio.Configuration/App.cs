namespace AppSettingsStudio.Configuration;

public class App(Manager manager, string name)
{
    [Browsable(false)]
    public Manager Manager { get; } = manager;

    [DisplayName("Application Name")]
    public string Name { get; } = name;

    [Browsable(false)]
    public virtual IDictionary<string, Instance> Instances { get; } = new Dictionary<string, Instance>(StringComparer.OrdinalIgnoreCase);

    [Browsable(false)]
    public bool HasUnlinkedAppSettings => Instances.Values.Any(i => i.HasUnlinkedAppSettings);

    public override string ToString() => Name;

    public Instance? GetInstance(string appSettingsPath)
    {
        ArgumentNullException.ThrowIfNull(appSettingsPath);
        _ = Instances.TryGetValue(appSettingsPath, out var instance);
        return instance;
    }

    public virtual bool DeleteInstance(string appSettingsPath)
    {
        ArgumentNullException.ThrowIfNull(appSettingsPath);
        var instance = GetInstance(appSettingsPath);
        if (instance == null)
            return false;

        Instances.Remove(appSettingsPath);
        Utilities.MoveToRecycleBin(instance.DirectoryPath);
        return true;
    }

    public virtual void MakeInstanceUniqueNames()
    {
        var instances = Instances.Values.ToArray();
        var newNames = Utilities.ShortenNames([.. instances.Select(n => n.AppSettingsPath)], [Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar], " → ");
        for (var i = 0; i < instances.Length; i++)
        {
            instances[i].DisplayName = newNames[i];
        }
    }
}
