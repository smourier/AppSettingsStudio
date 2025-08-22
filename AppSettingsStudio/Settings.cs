namespace AppSettingsStudio;

public class Settings : Serializable<Settings>
{
    public const string FileName = "settings.json";

    public static string ConfigurationDirectoryPath { get; }
    public static string ConfigurationBackupDirectoryPath { get; }
    public static string ConfigurationFilePath { get; }
    public static string TempDirectoryPath { get; }
    public static string WebView2UserDataPath { get; }
    public static Settings Current { get; }

    static Settings()
    {
        // configuration files are stored in %localappdata%
        ConfigurationDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), typeof(Settings).Namespace!);

        // data is stored in user's Documents
        ConfigurationFilePath = Path.Combine(ConfigurationDirectoryPath, FileName);
        ConfigurationBackupDirectoryPath = Path.Combine(Path.GetDirectoryName(ConfigurationFilePath)!, BackupDirectoryName);
        TempDirectoryPath = Path.Combine(Path.GetTempPath(), typeof(Settings).Namespace!);
        WebView2UserDataPath = Path.Combine(ConfigurationDirectoryPath, "WebView2");

        // build settings
        Settings? settings = null;
        if (IOUtilities.PathIsFile(ConfigurationFilePath))
        {
            settings = Deserialize(ConfigurationFilePath, true);
        }
        Current = settings ?? new Settings();
    }

    [Browsable(false)]
    public virtual string? LastFullPath { get => GetPropertyValue<string?>(null); set => SetPropertyValue(value); }

    [Browsable(false)]
    public virtual string? LastExportDirectoryPath { get => GetPropertyValue<string?>(null); set => SetPropertyValue(value); }

    [Browsable(false)]
    public virtual string? LastImportDirectoryPath { get => GetPropertyValue<string?>(null); set => SetPropertyValue(value); }

    [Browsable(false)]
    public virtual IDictionary<string, FormPlacement> Placements { get; set; } = new Dictionary<string, FormPlacement>(StringComparer.OrdinalIgnoreCase);

    [Browsable(false)]
    public virtual IDictionary<string, IList<string>> Trees { get; set; } = new Dictionary<string, IList<string>>(StringComparer.OrdinalIgnoreCase);

    [Browsable(false)]
    public virtual float MainSpliterDistance { get => GetPropertyValue<float>(1.0f); set => SetPropertyValue(value); }

    [Category("UI")]
    [DisplayName("Display Root Nodes In Bold")]
    [DefaultValue(false)]
    public virtual bool RootNodesBold { get => GetPropertyValue(false); set => SetPropertyValue(value); }

    [Category("UI")]
    [DisplayName("View Type")]
    [DefaultValue(ViewType.Flat)]
    public virtual ViewType ViewType { get => GetPropertyValue(ViewType.Flat); set => SetPropertyValue(value); }

    [Category("UI")]
    [DisplayName("Hiearchical View Levels")]
    [DefaultValue(1)]
    public virtual int HiearchicalViewLevels
    {
        get => GetPropertyValue(1);
        set
        {
            value = Math.Max(1, value);
            SetPropertyValue(value);
        }
    }

    [Category("Json Editor")]
    [DisplayName("AllowAppSettingsModifications")]
    [DefaultValue(false)]
    public virtual bool AllowAppSettingsModifications { get => GetPropertyValue(false); set => SetPropertyValue(value); }

    [Category("Json Editor")]
    [DisplayName("Enable Minimap")]
    [DefaultValue(true)]
    public virtual bool JsonMinimap { get => GetPropertyValue(true); set => SetPropertyValue(value); }

    [Category("Json Editor")]
    [DisplayName("Theme (vs, vs-dark, hc-light, hc-black)")]
    [DefaultValue("vs")]
    public virtual string? JsonTheme { get => GetPropertyValue("vs"); set { SetPropertyValue(value); } }

    [Category("Json Editor")]
    [DisplayName("Font Size")]
    [DefaultValue(13d)]
    public virtual double JsonFontSize { get => GetPropertyValue(13d); set { SetPropertyValue(value); } }

    [Category("Configuration")]
    [DisplayName("Additional Root Paths")]
    [Editor(typeof(RootPathsEditor), typeof(UITypeEditor))]
    [TypeConverter(typeof(CollectionListTypeConverter))]
    public virtual string[]? RootPaths { get => GetPropertyValue<string[]>(); set { SetPropertyValue(value); } }

    public virtual void SerializeToConfiguration() => Serialize(ConfigurationFilePath);

    public virtual void SaveTree(TreeView treeView)
    {
        var list = new List<string>();
        SaveTree(list, treeView.Nodes);
        Trees[treeView.Name] = list;
    }

    private static void SaveTree(IList<string> save, TreeNodeCollection nodes)
    {
        foreach (var node in nodes.Cast<TreeNode>().Where(n => n.IsExpanded))
        {
            save.Add(node.FullPath);
            SaveTree(save, node.Nodes);
        }
    }

    public virtual bool RestoreTree(TreeView treeView)
    {
        ArgumentNullException.ThrowIfNull(treeView);
        if (!Trees.TryGetValue(treeView.Name, out var keys))
            return false;

        foreach (var key in keys)
        {
            var node = treeView.FindByFullPath(key);
            node?.Expand();
        }
        return true;
    }

    public virtual void SavePlacement(Form form)
    {
        ArgumentNullException.ThrowIfNull(form);
        if (form is Main main && main.splitContainerMain.Width > 100)
        {
            MainSpliterDistance = main.splitContainerMain.SplitterDistance / (float)main.splitContainerMain.Width;
        }

        var placement = FormPlacement.SavePlacement(form);
        Placements[form.Name] = placement;
    }

    public virtual bool RestorePlacement(Form form)
    {
        ArgumentNullException.ThrowIfNull(form);
        if (form is Main main)
        {
            if (MainSpliterDistance > 0.1 && MainSpliterDistance < 0.9)
            {
                main.splitContainerMain.SplitterDistance = (int)(main.splitContainerMain.Width * MainSpliterDistance);
            }
        }

        if (!Placements.TryGetValue(form.Name, out var placement))
            return false;

        return placement.RestorePlacement(form);
    }
}
