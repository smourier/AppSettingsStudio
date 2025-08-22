namespace AppSettingsStudio;

public partial class VariableForm : Form
{
    public VariableForm()
    {
        InitializeComponent();
        Icon = Res.MainIcon;
        UpdateControls();
    }

    private void UpdateControls()
    {
        var name = textBoxName.Text.Nullify();
        buttonOk.Enabled = !textBoxName.Enabled || (name != null && !Manager.Variables.ContainsKey(name));
    }

    private void TextBoxName_TextChanged(object sender, EventArgs e) => UpdateControls();
    private void TextBoxValue_TextChanged(object sender, EventArgs e) => UpdateControls();
}
