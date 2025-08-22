namespace AppSettingsStudio;

internal class RootPathsEditor : UITypeEditor
{
    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext? context) => UITypeEditorEditStyle.Modal;
    public override object? EditValue(ITypeDescriptorContext? context, IServiceProvider provider, object? value)
    {
        if (provider.GetService(typeof(IWindowsFormsEditorService)) is IWindowsFormsEditorService editorService)
        {
            var conf = Program.Host.Services.GetRequiredService<IConfiguration>();
            var path = SettingsConfigurationProvider.GetRootPath(conf)!;
            var dlg = new RootPathsForm(path);
            if (editorService.ShowDialog(dlg) == DialogResult.OK)
                return RootPathsForm.HandleOk(dlg);
        }

        return base.EditValue(context, provider, value);
    }
}
