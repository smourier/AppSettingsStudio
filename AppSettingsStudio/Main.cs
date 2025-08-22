namespace AppSettingsStudio;

public partial class Main : Form, ILoggable<Main>
{
    private readonly TreeNode _rootNode;
    private MonacoEditorControl? _editorControl;
    private string? _currentJsonFilePath;
    private bool _currentJsonFileHasChanged;
    private Encoding? _currentJsonFileEncoding;
    private readonly ConcurrentDictionary<string, Manager> _managers = new(StringComparer.OrdinalIgnoreCase);
    internal static Main? _current;
    internal readonly Font _boldFont;

    public Main()
    {
        _current = this;
        InitializeComponent();
        _boldFont = new Font(treeViewSettings.Font, FontStyle.Bold);
        Icon = Res.MainIcon;
        Text = AssemblyUtilities.GetProduct();

        Settings.Current.RestorePlacement(this);

        treeViewSettings.ImageList = ImageLibrary.Images;
        treeViewSettings.KeyDown += (s, e) =>
        {
            if (e.KeyCode == Keys.F2 && (treeViewSettings.SelectedNode?.IsEditing != true))
            {
                treeViewSettings.SelectedNode?.BeginEdit();
            }
        };
        Logger = Program.Host.Services.GetService<ILogger<Main>>();

        var conf = Program.Host.Services.GetRequiredService<IConfiguration>();
        DefaultRootPath = SettingsConfigurationProvider.GetRootPath(conf)!;

        ResetRootPaths();

        _rootNode = treeViewSettings.Nodes.Add(Res.Applications);
        _rootNode.SetImageIndex(ImageLibraryIndex.Resource);

        LoadMonacoEditor();
    }

    public string DefaultRootPath { get; }
    public ILogger<Main>? Logger { get; }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        UpdateTree();
        _rootNode.Expand();
        Settings.Current.RestoreTree(treeViewSettings);
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        if (_currentJsonFilePath != null && _currentJsonFileHasChanged &&
            this.ShowConfirm($"The current json file '{Path.GetFileName(_currentJsonFilePath)}' has changed, do you want to discard the changes?") != DialogResult.Yes)
        {
            e.Cancel = true;
            return;
        }

        Settings.Current.SavePlacement(this);
        Settings.Current.SaveTree(treeViewSettings);
        Settings.Current.SerializeToConfiguration();
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        if (e.KeyCode == Keys.Delete)
        {
            DeleteTreeItem();
            return;
        }
    }

    private static TreeNode EnsureAppNode(TreeNode rootNode, App app)
    {
        TreeNode appNode;

        if (Settings.Current.ViewType == ViewType.Flat)
        {
            appNode = rootNode.Nodes[app.Name]!;
            if (appNode == null)
            {
                appNode = new TreeNode(app.Name);
                rootNode.Nodes.Add(appNode);
            }
        }
        else
        {
            var levels = Math.Max(1, Settings.Current.HiearchicalViewLevels);
            var segments = app.Name.Split(['.'], levels + 1);

            var currentNode = rootNode;
            for (var i = 0; i < segments.Length; i++)
            {
                var node = currentNode.Nodes.Cast<TreeNode>().FirstOrDefault(n => n.Text == segments[i]);
                if (node == null)
                {
                    node = new TreeNode(segments[i]);
                    currentNode.Nodes.Add(node);
                    if (i < segments.Length - 1)
                    {
                        node.SetImageIndex(ImageLibraryIndex.Folder);
                        node.Name = segments[i];
                    }
                }
                currentNode = node;
            }
            appNode = currentNode;
        }
        return appNode;
    }

    private void UpdateTree()
    {
        foreach (var kv in _managers)
        {
            kv.Value.Clear();
            kv.Value.Load(kv.Key);
        }
        UpdateTree(_rootNode, _boldFont, _managers.Values, false);
    }

    private void ResetRootPaths()
    {
        _managers.Clear();
        var manager = new UIManager();
        _managers[DefaultRootPath] = manager;

        if (Settings.Current.RootPaths != null)
        {
            foreach (var rp in Settings.Current.RootPaths)
            {
                _managers[rp] = new UIManager();
            }
        }
    }

    internal static void UpdateTree(TreeNode rootNode, Font? rootFont, IEnumerable<Manager> managers, bool linkMode)
    {
        foreach (var manager in managers)
        {
            var existingAppsKeys = rootNode.EnumerateAllChildNodes().Where(n => n.Tag is App app && app.Manager == manager).ToList();

            foreach (var kv in manager.Apps.OrderBy(a => a.Key))
            {
                var existing = existingAppsKeys.FirstOrDefault(n => n.Tag is App app && app.Name == kv.Key);
                if (existing != null)
                {
                    existingAppsKeys.Remove(existing);
                }

                if (linkMode && !kv.Value.HasUnlinkedAppSettings)
                    continue;

                var appNode = EnsureAppNode(rootNode, kv.Value);

                appNode.Name = kv.Key;
                appNode.Tag = kv.Value;
                if (Settings.Current.RootNodesBold)
                {
                    appNode.NodeFont = rootFont;
                }
                appNode.ToolTipText = kv.Key;

                var existingInstancesKeys = appNode.Nodes.Cast<TreeNode>().Select(n => n.Name).ToHashSet();

                foreach (var instance in kv.Value.Instances.OrderBy(i => i.Key))
                {
                    existingInstancesKeys.Remove(instance.Key);

                    if (linkMode && !instance.Value.HasUnlinkedAppSettings)
                        continue;

                    var instanceNode = appNode.Nodes[instance.Key];
                    if (instanceNode == null)
                    {
                        instanceNode = new TreeNode(instance.Value.DisplayName);
                        appNode.Nodes.Add(instanceNode);
                    }

                    instanceNode.Name = instance.Key;
                    instanceNode.Tag = instance.Value;
                    instanceNode.ToolTipText = instance.Key;
                    var imageIndex = UIInstance.GetImageIndex(instance.Value);
                    instanceNode.SetImageIndex(imageIndex);

                    if (!linkMode)
                    {
                        instanceNode.ForeColor = instance.Value.Exists ? Color.Empty : Color.Gray;
                    }

                    var existingAppSettingsKeys = instanceNode.Nodes.Cast<TreeNode>().Select(n => n.Name).ToHashSet();

                    foreach (var appSettings in instance.Value.AppSettings)
                    {
                        existingAppSettingsKeys.Remove(appSettings.Name);
                        var appSettingsNode = instanceNode.Nodes[appSettings.Name];
                        if (appSettingsNode == null)
                        {
                            appSettingsNode = new TreeNode(appSettings.Name);
                            instanceNode.Nodes.Add(appSettingsNode);
                        }

                        appSettingsNode.Name = appSettings.Name;
                        appSettingsNode.Tag = appSettings;

                        if (appSettings.IsActive)
                        {
                            if (appSettings.IsLink)
                            {
                                appSettingsNode.SetImageIndex(ImageLibraryIndex.JsonLinkActive);
                            }
                            else
                            {
                                appSettingsNode.SetImageIndex(ImageLibraryIndex.JsonActive);
                            }
                        }
                        else if (appSettings.IsLink)
                        {
                            appSettingsNode.SetImageIndex(ImageLibraryIndex.JsonLink);
                        }
                        else
                        {
                            appSettingsNode.SetImageIndex(ImageLibraryIndex.Json);
                        }
                    }

                    foreach (var key in existingAppSettingsKeys)
                    {
                        instanceNode.Nodes.RemoveByKey(key);
                    }
                }

                ImageLibrary.ConsolidateImages(appNode);

                foreach (var key in existingInstancesKeys)
                {
                    appNode.Nodes.RemoveByKey(key);
                }
            }

            foreach (var key in existingAppsKeys)
            {
                key.RemoveUpToRootIfEmpty();
            }
        }
    }

    private TreeNode? GetAppNode(App app) => _rootNode.Nodes.Find(app.Name, false).FirstOrDefault();
    private TreeNode? GetInstanceNode(Instance instance)
    {
        var appNode = GetAppNode(instance.App);
        if (appNode == null)
            return null;

        return appNode.Nodes.Find(instance.AppSettingsPath, false).FirstOrDefault();
    }

    private void LoadMonacoEditor()
    {
        if (_editorControl == null)
        {
            var control = new MonacoEditorControl { Dock = DockStyle.Fill, Visible = false };
            control.Event += OnMonacoEvent;

            splitContainerMain.Panel2.Controls.Add(control);
            _editorControl = control;
        }
    }

    private Task SelectLastPath() => Task.Run(() =>
    {
        var last = Settings.Current.LastFullPath.Nullify();
        if (last != null)
        {
            BeginInvoke(() =>
            {
                var node = _rootNode.FindByFullPath(last);
                if (node != null)
                {
                    node.EnsureVisible();
                    treeViewSettings.SelectedNode = node;
                }
            });
        }
    });

    private async void OnMonacoEvent(object? sender, MonacoEventArgs e)
    {
        if (e.EventType == MonacoEventType.EditorCreated)
        {
            await _editorControl!.SetEditorLanguage("json");
            await _editorControl.ExecuteScript("monaco.languages.json.jsonDefaults.setDiagnosticsOptions({comments: 'ignore', trailingCommas: 'ignore'});");

            _ = SelectLastPath();
            return;
        }

        if (e.EventType == MonacoEventType.ContentChanged)
        {
            if (_currentJsonFilePath != null)
            {
                _currentJsonFileHasChanged = true;
                BeginInvoke(() =>
                {
                    Text = AssemblyUtilities.GetProduct() + " - *" + _currentJsonFilePath;
                });
            }
            return;
        }

        if (e.EventType == MonacoEventType.KeyUp)
        {
            var ke = (MonacoKeyEventArgs)e;
            if (ke.Keys == (Keys.Control | Keys.S))
            {
                await Save();
            }
        }
    }

    private void DeleteTreeItem()
    {
        if (treeViewSettings.SelectedNode?.Tag is Instance instance)
        {
            if (this.ShowConfirm($"Are you sure you want to delete '{instance.DisplayName}' instance (note: this will *not* delete pointed appsettings*.json files)?") != DialogResult.Yes)
                return;

            var app = instance.App.Manager.GetApp(instance.App.Name);
            if (app == null)
                return;

            var existing = app.GetInstance(instance.AppSettingsPath);
            if (existing == null)
                return;

            if (app.DeleteInstance(instance.AppSettingsPath))
            {
                UpdateTree();
            }
            return;
        }

        if (treeViewSettings.SelectedNode?.Tag is AppSettings appSettings)
        {
            var links = appSettings.GetLinksTo();
            if (links.Count > 0)
            {
                if (this.ShowConfirm($"These virtual settings '{appSettings.Name}' have {links.Count} virtual settings linked to it, are you sure you want to delete it and its links too?") != DialogResult.Yes)
                    return;
            }
            else
            {
                if (this.ShowConfirm($"Are you sure you want to delete '{appSettings.Name}' virtual settings?") != DialogResult.Yes)
                    return;
            }

            var newAppSetting = appSettings.Instance.DeleteAppSettings(appSettings);
            propertyGridMain.SelectedObject = newAppSetting;
            UpdateTree();
            return;
        }
    }

    private void OpenWithDefaultEditor()
    {
        if (treeViewSettings.SelectedNode?.Tag is IWithFilePath jsonFilePath && jsonFilePath.FilePath != null && IOUtilities.PathIsFile(jsonFilePath.FilePath))
        {
            WinformsUtilities.OpenFile(jsonFilePath.FilePath);
        }
    }

    private async Task Save()
    {
        if (_editorControl == null || _currentJsonFilePath == null || _currentJsonFileEncoding == null || !_currentJsonFileHasChanged)
            return;

        var text = await _editorControl.GetEditorText();
        if (text == null)
            return;

        await File.WriteAllTextAsync(_currentJsonFilePath, text, _currentJsonFileEncoding);
        _currentJsonFileHasChanged = false;
        BeginInvoke(() =>
        {
            Text = AssemblyUtilities.GetProduct() + " - " + _currentJsonFilePath;
        });
    }

    private void Export()
    {
        var dlg = new FolderBrowserDialog
        {
            ShowNewFolderButton = true,
        };

        var initialPath = Settings.Current.LastExportDirectoryPath.Nullify();
        if (initialPath != null)
        {
            dlg.InitialDirectory = initialPath;
        }
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;

        Settings.Current.LastExportDirectoryPath = dlg.SelectedPath;
        Settings.Current.SerializeToConfiguration();
        var path = Manager.ExportAsZipFile(dlg.SelectedPath, _managers.Keys);
        this.ShowMessage($"All settings have been exported successfully to {path}.");
    }

    private void ShowRootPaths()
    {
        var dlg = new RootPathsForm(DefaultRootPath);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            RootPathsForm.HandleOk(dlg);
            ResetRootPaths();
            RefreshTree();
        }
    }

    private Task RefreshTree()
    {
        Settings.Current.LastFullPath = treeViewSettings.SelectedNode?.FullPath;
        Settings.Current.SaveTree(treeViewSettings);
        _rootNode.Nodes.Clear();
        UpdateTree();
        Settings.Current.RestoreTree(treeViewSettings);
        return SelectLastPath();
    }

    private void FlatToolStripMenuItem_Click(object sender, EventArgs e) => UpdateViewType((ToolStripMenuItem)sender);
    private void Hierarchical1LevelsToolStripMenuItem_Click(object sender, EventArgs e) => UpdateViewType((ToolStripMenuItem)sender);
    private void Hierarchical2LevelsToolStripMenuItem_Click(object sender, EventArgs e) => UpdateViewType((ToolStripMenuItem)sender);
    private void Hierarchical3LevelsToolStripMenuItem_Click(object sender, EventArgs e) => UpdateViewType((ToolStripMenuItem)sender);
    private void HierarchicalToolStripMenuItem_Click(object sender, EventArgs e) => UpdateViewType((ToolStripMenuItem)sender);
    private void RootPathsToolStripMenuItem_Click(object sender, EventArgs e) => ShowRootPaths();
    private void OpenRootDirectoryPathToolStripMenuItem_Click(object sender, EventArgs e) => WinformsUtilities.OpenExplorer(Manager.GetDefaultRootPath());
    private void ExportToolStripMenuItem_Click(object sender, EventArgs e) => Export();
    private void SaveToolStripMenuItem_Click(object sender, EventArgs e) => _ = Save();
    private void OpenWithDefaultEditorToolStripMenuItem_Click(object sender, EventArgs e) => OpenWithDefaultEditor();
    private void AboutSettingsStudioToolStripMenuItem_Click(object sender, EventArgs e) => new AboutForm().ShowDialog(this);
    private void RefreshToolStripMenuItem_Click(object sender, EventArgs e) => _ = RefreshTree();
    private void DeleteToolStripMenuItem_Click(object sender, EventArgs e) => DeleteTreeItem();
    private void DeleteAppSettingToolStripMenuItem_Click(object sender, EventArgs e) => DeleteTreeItem();
    private void ExitToolStripMenuItem_Click(object sender, EventArgs e) => Close();
    private void TreeViewSettings_AfterSelect(object sender, TreeViewEventArgs e)
    {
        var tt = treeViewSettings.SelectedNode?.ToolTipText.Nullify() ?? string.Empty;
        toolStripStatusLabelLeft.Text = tt;
        propertyGridMain.SelectedObject = treeViewSettings.SelectedNode?.Tag ?? new { Namespace = treeViewSettings.SelectedNode?.FullPath?.Replace('\\', '.') };

        if (treeViewSettings.SelectedNode?.Tag is IWithFilePath jsonFilePath && jsonFilePath.FilePath != null && IOUtilities.PathIsFile(jsonFilePath.FilePath))
        {
            var filePath = jsonFilePath.FilePath;
            _ = loadText();
            async Task loadText()
            {
                if (_editorControl == null)
                    return;

                var text = EncodingDetector.ReadAllText(filePath, EncodingDetectorMode.UseUTF8AsDefault, out _currentJsonFileEncoding);
                await _editorControl.LoadText(text);
                _editorControl.Visible = true;
                _currentJsonFilePath = filePath;
                _currentJsonFileHasChanged = false;

                await _editorControl.MoveEditorTo(1, 1);
                await _editorControl.SetReadOnly(jsonFilePath.IsReadOnly);

                BeginInvoke(() =>
                {
                    Text = AssemblyUtilities.GetProduct() + " - " + filePath;

                    var fp = treeViewSettings.SelectedNode.FullPath;
                    if (Settings.Current.LastFullPath != fp)
                    {
                        Settings.Current.LastFullPath = fp;
                        Settings.Current.SerializeToConfiguration();
                    }
                });
            }
        }
        else
        {
            _currentJsonFilePath = null;
            Text = AssemblyUtilities.GetProduct();
            if (_editorControl == null)
                return;

            _editorControl.Visible = false;

            var fp = treeViewSettings.SelectedNode?.FullPath;
            if (Settings.Current.LastFullPath != fp)
            {
                Settings.Current.LastFullPath = fp;
                Settings.Current.SerializeToConfiguration();
            }
        }
    }

    private void TreeViewSettings_BeforeSelect(object sender, TreeViewCancelEventArgs e)
    {
        if (_currentJsonFilePath != null && _currentJsonFileHasChanged)
        {
            if (this.ShowConfirm($"The current json file '{Path.GetFileName(_currentJsonFilePath)}' has changed, do you want to discard the changes?") != DialogResult.Yes)
            {
                e.Cancel = true;
                return;
            }

            _currentJsonFilePath = null;
            _currentJsonFileHasChanged = false;
        }
    }

    private void RunToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (treeViewSettings.SelectedNode?.Tag is not Instance instance)
            return;

        var exePath = Path.Combine(instance.AppSettingsDirectoryPath, instance.App.Name + ".exe");
        if (IOUtilities.PathIsFile(exePath))
        {
            WinformsUtilities.OpenFile(exePath);
        }
    }

    private void FileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
    {
        saveToolStripMenuItem.Enabled = _currentJsonFilePath != null && _currentJsonFileHasChanged;
        if (saveToolStripMenuItem.Enabled)
        {
            saveToolStripMenuItem.Text = $"&Save {Path.GetFileName(_currentJsonFilePath)}";
        }
        else
        {
            saveToolStripMenuItem.Text = $"&Save {treeViewSettings.SelectedNode?.Text}";
        }
    }

    private void TreeViewSettings_MouseClick(object sender, MouseEventArgs e)
    {
        var node = treeViewSettings.GetNodeAt(new Point(e.X, e.Y));
        if (e.Button == MouseButtons.Right)
        {
            treeViewSettings.SelectedNode = node;
        }

        if (node?.Tag is Instance)
        {
            node.ContextMenuStrip = contextMenuStripInstance;
        }
        else if (node?.Tag is AppSettings)
        {
            node.ContextMenuStrip = contextMenuStripAppSettings;
        }
        else
        {
            if (node != null)
            {
                node.ContextMenuStrip = null;
            }
        }
    }

    private void PreferencesToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var dlg = new SettingsForm(Settings.Current) { Icon = Res.MainIcon };
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            Settings.Current.CopyFrom((Settings)dlg.Settings);
            Settings.Current.SerializeToConfiguration();
            ResetRootPaths();
            RefreshTree();
        }
    }

    private void VariablesToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var dlg = new VariablesForm();
        dlg.ShowDialog(this);
    }

    private void AddVirtualSettingsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (treeViewSettings.SelectedNode?.Tag is not Instance instance)
            return;

        var dlg = new AppSettingsNameForm(instance);
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;

        instance.AddAppSettings(dlg.textBoxName.Text.Nullify()!);
        UpdateTree();

        var node = GetInstanceNode(instance);
        node?.Expand();
    }

    private void ImportVirtualSettingsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (treeViewSettings.SelectedNode?.Tag is not Instance instance)
            return;

        var ofd = new System.Windows.Forms.OpenFileDialog
        {
            Multiselect = false,
            CheckFileExists = true,
            Title = "Pick an existing appsettings*.json file",
            DefaultExt = ".json",
            Filter = "JSON Files (appsettings*.json)|appsettings*.json|All Files (*.*)|*.*"
        };

        var initialPath = Settings.Current.LastImportDirectoryPath.Nullify();
        if (initialPath != null)
        {
            ofd.InitialDirectory = initialPath;
        }
        else
        {
            ofd.RestoreDirectory = true;
        }

        if (ofd.ShowDialog(this) != DialogResult.OK)
            return;

        Settings.Current.LastImportDirectoryPath = Path.GetDirectoryName(ofd.FileName);
        Settings.Current.SerializeToConfiguration();

        var directory = Path.GetDirectoryName(ofd.FileName);
        if (directory.EqualsIgnoreCase(instance.DirectoryPath))
            return;

        var target = Path.Combine(instance.DirectoryPath, Path.GetFileName(ofd.FileName));
        if (IOUtilities.PathIsFile(target))
        {
            if (this.ShowConfirm($"The file '{Path.GetFileName(target)}' already exists in instance's directory, do you want to overwrite it?") != DialogResult.Yes)
                return;
        }

        IOUtilities.FileOverwrite(ofd.FileName, target, true);
        UpdateTree();

        var node = GetInstanceNode(instance);
        node?.Expand();
    }

    private void LinkVirtualSettingsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (treeViewSettings.SelectedNode?.Tag is not Instance instance)
            return;

        var dlg = new BrowserForm(_managers.Values);
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;

        var target = dlg.AppSettings;
        if (target == null)
            return;

        instance.LinkToAppSettings(target);
        UpdateTree();

        var node = GetInstanceNode(instance);
        node?.Expand();
    }

    private void MakeActiveToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (treeViewSettings.SelectedNode?.Tag is AppSettings appSettings && !appSettings.IsActive)
        {
            appSettings.Instance.ActivateAppSettings(appSettings);
            appSettings.Instance.Save();
            propertyGridMain.Refresh();
            UpdateTree();
        }
    }

    private void ContextMenuStripAppSettings_Opening(object sender, CancelEventArgs e)
    {
        makeActiveToolStripMenuItem.Enabled = treeViewSettings.SelectedNode?.Tag is AppSettings appSetting && !appSetting.IsActive;
    }

    private void TreeViewSettings_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
    {
        if (e.Label == null || e.Node?.Tag is not AppSettings appSettings || appSettings.IsLink)
        {
            e.CancelEdit = true;
            return;
        }

        if (!e.Label.StartsWith("appsettings", StringComparison.OrdinalIgnoreCase) || !e.Label.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
        {
            e.CancelEdit = true;
            this.ShowWarning("Name must start with 'appsettings' and end with '.json'.");
            e.Node.BeginEdit();
            return;
        }

        if (appSettings.Instance.AppSettings.Any(a => !a.IsLink && a.Name.EqualsIgnoreCase(e.Label)))
        {
            e.CancelEdit = true;
            this.ShowWarning("There's already a virtual settings with this name.");
            e.Node.BeginEdit();
            return;
        }

        if (appSettings.Instance.RenameAppSettings(appSettings, e.Label))
        {
            e.Node.EndEdit(false);
            UpdateTree();
            propertyGridMain.Refresh();

            // not sure why we have to do this, it's related to UpdateTree
            _ = endEdit();
            Task endEdit() => Task.Run(() => BeginInvoke(() =>
            {
                var instanceNode = GetInstanceNode(appSettings.Instance);
                if (instanceNode == null)
                    return;

                foreach (TreeNode appSettingsNode in instanceNode.Nodes)
                {
                    appSettingsNode.EndEdit(false);
                }
            }));
        }
    }

    private void TreeViewSettings_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
    {
        if (e.Node?.Tag is not AppSettings appSettings || appSettings.IsLink)
        {
            e.CancelEdit = true;
            return;
        }
    }

    private sealed class UIManager : Manager
    {
        protected override Instance CreateInstance(App app, string directoryPath, string appSettingsPath) => new UIInstance(app, directoryPath, appSettingsPath);
        protected override AppSettings CreateAppSettings(Instance instance, string filePath) => new UIAppSettings(instance, filePath);
    }

    private sealed class UIAppSettings(Instance instance, string filePath) : AppSettings(instance, filePath)
    {
        [Editor(typeof(OpenLocationEditor), typeof(UITypeEditor))]
        public override string FilePath => base.FilePath;
    }

    private sealed class UIInstance(App app, string directoryPath, string appSettingsPath) : Instance(app, directoryPath, appSettingsPath)
    {
        [Editor(typeof(OpenLocationEditor), typeof(UITypeEditor))]
        public override string AppSettingsPath { get => base.AppSettingsPath; set => base.AppSettingsPath = value; }

        [Editor(typeof(OpenPathEditor), typeof(UITypeEditor))]
        public new string DirectoryPath => base.DirectoryPath;

        private static ImageLibraryIndex? GetWellKnownImages(string exePath)
        {
            var name = Path.GetFileName(exePath);
            if (name.EqualsIgnoreCase("iisexpress.exe"))
                return ImageLibraryIndex.Culture;

            if (name.EqualsIgnoreCase("testhost.exe"))
                return ImageLibraryIndex.Forms;

            if (name.EqualsIgnoreCase("dotnet.exe"))
                return ImageLibraryIndex.UIControlDefinition;

            return null;
        }

        public static ImageLibraryIndex GetImageIndex(Instance instance)
        {
            var exePath = Path.Combine(instance.AppSettingsDirectoryPath, instance.App.Name + ".exe");
            if (!IOUtilities.PathIsFile(exePath))
                return GetWellKnownImages(exePath) ?? ImageLibraryIndex.Application2;

            var index = ImageLibrary.AddOrGetImageIndex(exePath);
            if (index < 0)
                return GetWellKnownImages(exePath) ?? ImageLibraryIndex.Application2;

            return (ImageLibraryIndex)index;
        }
    }

    private void UpdateViewType(ToolStripMenuItem clicked)
    {
        flatToolStripMenuItem.Checked = clicked == flatToolStripMenuItem;
        hierarchical1LevelsToolStripMenuItem.Checked = clicked == hierarchical1LevelsToolStripMenuItem;
        hierarchical2LevelsToolStripMenuItem.Checked = clicked == hierarchical2LevelsToolStripMenuItem;
        hierarchical3LevelsToolStripMenuItem.Checked = clicked == hierarchical3LevelsToolStripMenuItem;
        hierarchicalToolStripMenuItem.Checked = clicked == hierarchicalToolStripMenuItem;
        if (flatToolStripMenuItem.Checked)
        {
            Settings.Current.ViewType = ViewType.Flat;
        }
        else
        {
            Settings.Current.ViewType = ViewType.Hierarchical;
            if (hierarchical1LevelsToolStripMenuItem.Checked)
            {
                Settings.Current.HiearchicalViewLevels = 1;
            }
            else if (hierarchical2LevelsToolStripMenuItem.Checked)
            {
                Settings.Current.HiearchicalViewLevels = 2;
            }
            else if (hierarchical3LevelsToolStripMenuItem.Checked)
            {
                Settings.Current.HiearchicalViewLevels = 3;
            }
            else
            {
                Settings.Current.HiearchicalViewLevels = 4;
            }
        }

        Settings.Current.SerializeToConfiguration();
        _rootNode.Nodes.Clear();
        UpdateTree();
    }

    private void TreeToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
    {
        flatToolStripMenuItem.Checked = Settings.Current.ViewType == ViewType.Flat;
        hierarchical1LevelsToolStripMenuItem.Checked = false;
        hierarchical2LevelsToolStripMenuItem.Checked = false;
        hierarchical3LevelsToolStripMenuItem.Checked = false;
        hierarchicalToolStripMenuItem.Checked = false;
        if (Settings.Current.ViewType == ViewType.Hierarchical)
        {
            switch (Settings.Current.HiearchicalViewLevels)
            {
                case 1:
                    hierarchical1LevelsToolStripMenuItem.Checked = true;
                    break;

                case 2:
                    hierarchical2LevelsToolStripMenuItem.Checked = true;
                    break;

                case 3:
                    hierarchical3LevelsToolStripMenuItem.Checked = true;
                    break;

                default:
                    hierarchicalToolStripMenuItem.Checked = true;
                    break;
            }
        }
    }
}
