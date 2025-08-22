namespace AppSettingsStudio;

public partial class BrowserForm : Form
{
    private readonly IEnumerable<Manager> _managers;
    private readonly TreeNode _rootNode;

    public BrowserForm(IEnumerable<Manager> managers)
    {
        _managers = managers;
        InitializeComponent();
        Icon = Res.MainIcon;

        treeViewSettings.ImageList = ImageLibrary.Images;
        _rootNode = treeViewSettings.Nodes.Add(Res.Applications);
        _rootNode.SetImageIndex(ImageLibraryIndex.Resource);
    }

    public AppSettings? AppSettings => treeViewSettings.SelectedNode?.Tag as AppSettings;

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        Main.UpdateTree(_rootNode, Main._current?._boldFont, _managers, true);
        _rootNode.Expand();
        Settings.Current.RestoreTree(treeViewSettings);
    }

    private void UpdateControls()
    {
        buttonOk.Enabled = treeViewSettings.SelectedNode?.Tag is AppSettings;
    }

    private void TreeViewSettings_AfterSelect(object sender, TreeViewEventArgs e) => UpdateControls();
    private void TreeViewSettings_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        DialogResult = DialogResult.OK;
        Close();
    }
}
