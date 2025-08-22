namespace AppSettingsStudio.Utilities;

public class OpenPathEditor : UITypeEditor
{
    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext? context) => UITypeEditorEditStyle.Modal;
    public override object? EditValue(ITypeDescriptorContext? context, IServiceProvider provider, object? value)
    {
        if (value is string filePath)
        {
            WinformsUtilities.OpenExplorer(filePath);
            return value;
        }
        return base.EditValue(context, provider, value);
    }
}
