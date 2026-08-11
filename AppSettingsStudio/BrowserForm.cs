namespace AppSettingsStudio;

public partial class BrowserForm : Form
{
    private readonly IEnumerable<Manager> _managers;
    private readonly TreeNode _rootNode;
    private readonly Func<object?, bool> _canSelect;
    private readonly string? _selectedFilePath;
    private readonly bool _linkMode;

    public BrowserForm(IEnumerable<Manager> managers, Func<object?, bool>? canSelect = null, string? title = null, string? selectedFilePath = null, bool linkMode = true)
    {
        _managers = managers;
        _canSelect = canSelect ?? (tag => tag is AppSettings);
        _selectedFilePath = selectedFilePath;
        _linkMode = linkMode;
        InitializeComponent();
        Icon = Res.MainIcon;
        if (title != null)
        {
            Text = title;
        }

        treeViewSettings.ImageList = ImageLibrary.Images;
        _rootNode = treeViewSettings.Nodes.Add(Res.Applications);
        _rootNode.SetImageIndex(ImageLibraryIndex.Resource);
    }

    public AppSettings? AppSettings => treeViewSettings.SelectedNode?.Tag as AppSettings;
    public object? SelectedTag => treeViewSettings.SelectedNode?.Tag;

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        Main.UpdateTree(_rootNode, Main._current?._boldFont, _managers, _linkMode);
        _rootNode.Expand();

        if (!_linkMode)
        {
            var filesNode = treeViewSettings.Nodes.Add(Res.Files);
            filesNode.SetImageIndex(ImageLibraryIndex.Folder);
            Main.BuildFilesTree(filesNode);
            filesNode.Expand();
        }

        Settings.Current.RestoreTree(treeViewSettings);
        SelectByFilePath(_selectedFilePath);
        UpdateControls();
    }

    private void SelectByFilePath(string? filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            return;

        var node = FindByFilePath(treeViewSettings.Nodes, filePath);
        if (node != null)
        {
            treeViewSettings.SelectedNode = node;
            node.EnsureVisible();
        }
    }

    private static TreeNode? FindByFilePath(TreeNodeCollection nodes, string filePath)
    {
        foreach (TreeNode node in nodes)
        {
            if (node.Tag is IWithFilePath withFilePath && withFilePath.FilePath is { } path && string.Equals(path, filePath, StringComparison.OrdinalIgnoreCase))
                return node;

            var found = FindByFilePath(node.Nodes, filePath);
            if (found != null)
                return found;
        }

        return null;
    }

    private void UpdateControls()
    {
        buttonOk.Enabled = _canSelect(treeViewSettings.SelectedNode?.Tag);
    }

    private void TreeViewSettings_AfterSelect(object sender, TreeViewEventArgs e) => UpdateControls();
    private void TreeViewSettings_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        if (!_canSelect(treeViewSettings.SelectedNode?.Tag))
            return;

        DialogResult = DialogResult.OK;
        Close();
    }
}
