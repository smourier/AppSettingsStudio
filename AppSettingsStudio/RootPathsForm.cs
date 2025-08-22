namespace AppSettingsStudio;

public partial class RootPathsForm : Form
{
    private readonly string _rootPath;

    public RootPathsForm(string rootPath)
    {
        ArgumentNullException.ThrowIfNull(rootPath);
        _rootPath = rootPath;
        InitializeComponent();
        Icon = Res.MainIcon;

        listViewPaths.Items.Add(rootPath);
        var rootPaths = Settings.Current.RootPaths;
        if (rootPaths != null)
        {
            foreach (var item in rootPaths)
            {
                listViewPaths.Items.Add(item);
            }
        }

        listViewPaths.AutoResizeColumnsWidth();
        UpdateControls();
    }

    public IEnumerable<string> NonRootPaths => listViewPaths.Items.OfType<ListViewItem>().Where(p => !p.Text.EqualsIgnoreCase(_rootPath)).Select(p => p.Text);

    internal static string[] HandleOk(RootPathsForm form)
    {
        Settings.Current.RootPaths = [.. form.NonRootPaths];
        Settings.Current.SerializeToConfiguration();
        return Settings.Current.RootPaths;
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        if (e.KeyCode == Keys.Escape)
        {
            Close();
        }
    }

    private string? GetCurrentPath() => listViewPaths.SelectedItems.OfType<ListViewItem>().FirstOrDefault()?.Text;
    private void UpdateControls()
    {
        var path = GetCurrentPath();
        buttonRemove.Enabled = path != null && !path.EqualsIgnoreCase(_rootPath);
        buttonBrowse.Enabled = path != null;
    }

    private void ListViewPaths_SelectedIndexChanged(object sender, EventArgs e) => UpdateControls();
    private void ButtonBrowse_Click(object sender, EventArgs e) => WinformsUtilities.OpenExplorer(GetCurrentPath());
    private void ButtonRemove_Click(object sender, EventArgs e)
    {
        var path = GetCurrentPath();
        if (path == null || path.EqualsIgnoreCase(_rootPath))
            return;

        if (this.ShowConfirm($"Are you sure you want to remove '{path}'?") != DialogResult.Yes)
            return;

        var index = listViewPaths.Items.OfType<ListViewItem>().IndexOf(i => i.Text.EqualsIgnoreCase(path));
        if (index < 0)
            return;

        listViewPaths.Items.RemoveAt(index);
    }

    private void ButtonAdd_Click(object sender, EventArgs e)
    {
        var dlg = new FolderPicker();
        if (dlg.ShowDialog(this) != true)
            return;

        if (NonRootPaths.Contains(dlg.ResultPath, StringComparer.OrdinalIgnoreCase))
            return;

        listViewPaths.Items.Add(dlg.ResultPath);
    }
}
