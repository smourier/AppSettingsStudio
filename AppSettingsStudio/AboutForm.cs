namespace AppSettingsStudio;

public partial class AboutForm : Form
{
    public AboutForm()
    {
        InitializeComponent();
        Icon = Res.MainIcon;
        pictureBoxIcon.Image = Res.MainIcon.ToBitmap();
        var asm = Assembly.GetEntryAssembly();
        var text = asm?.GetCustomAttribute<AssemblyProductAttribute>()?.Product + " V" + asm?.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;
        text += Environment.NewLine + asm?.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright;
        labelText.Text = text;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            pictureBoxIcon.Image?.Dispose();
        }
        base.Dispose(disposing);
    }
}
