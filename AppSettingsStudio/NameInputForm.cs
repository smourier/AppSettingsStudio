namespace AppSettingsStudio;

public partial class NameInputForm : Form
{
    public NameInputForm(FilesFolder folder)
    {
        ArgumentNullException.ThrowIfNull(folder);
        FilesFolder = folder;
        InitializeComponent();
        Icon = Res.MainIcon;
        UpdateControls();
    }

    public FilesFolder FilesFolder { get; }
    public string? FolderName => textBoxName.Text.Nullify();

    private void UpdateControls()
    {
        var text = textBoxName.Text.Nullify();
        buttonOk.Enabled = text != null && !FilesFolder.AllNames.Any(a => a.EqualsIgnoreCase(text));
    }

    private void TextBoxName_TextChanged(object sender, EventArgs e) => UpdateControls();
}
