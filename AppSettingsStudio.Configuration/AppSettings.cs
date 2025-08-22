namespace AppSettingsStudio.Configuration;

public class AppSettings(Instance instance, string filePath) : IWithFilePath
{
    [Browsable(false)]
    public Instance Instance { get; } = instance;

    [DisplayName("File Path")]
    public virtual string FilePath { get; } = filePath;
    public string Name => LinkedFilePath != null ? $"→ ({Path.GetFileName(LinkedFilePath)})" : Path.GetFileName(FilePath);
    bool IWithFilePath.IsReadOnly => LinkedFilePath != null;
    string? IWithFilePath.FilePath => LinkedFilePath ?? FilePath;

    [ReadOnly(false)]
    [DisplayName("Is Active")]
    public virtual bool IsActive { get; internal set; }

    [ReadOnly(false)]
    [DisplayName("Is Link")]
    public virtual bool IsLink => LinkedFilePath != null;

    [DisplayName("Linked File Path")]
    public virtual string? LinkedFilePath { get; internal set; }

    public IReadOnlyList<AppSettings> GetLinksTo()
    {
        var links = new List<AppSettings>();
        if (!IsLink)
        {
            foreach (var kv in Instance.App.Manager.Apps)
            {
                foreach (var kv2 in kv.Value.Instances)
                {
                    foreach (var appSettings in kv2.Value.AppSettings)
                    {
                        if (appSettings.LinkedFilePath.EqualsIgnoreCase(FilePath))
                        {
                            links.Add(appSettings);
                        }
                    }
                }
            }
        }
        return links;
    }

    public override string ToString() => $"{FilePath}";
}
