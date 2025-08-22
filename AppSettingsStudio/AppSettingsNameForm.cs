namespace AppSettingsStudio;

public partial class AppSettingsNameForm : Form
{
    public AppSettingsNameForm(Instance instance)
    {
        ArgumentNullException.ThrowIfNull(instance);
        Instance = instance;
        InitializeComponent();
        Icon = Res.MainIcon;
        textBoxName.Text = Manager.AppSettingsFileName;
        UpdateControls();
    }

    public Instance Instance { get; }

    private void UpdateControls()
    {
        var text = textBoxName.Text.Nullify();
        buttonOk.Enabled = text != null && text.StartsWith("appsettings") && text.EndsWith(".json") && Instance.AppSettings.All(a => !a.Name.EqualsIgnoreCase(text));
    }

    private void TextBoxName_TextChanged(object sender, EventArgs e) => UpdateControls();
}
