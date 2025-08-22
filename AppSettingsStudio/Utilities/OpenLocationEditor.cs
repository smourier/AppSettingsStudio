namespace AppSettingsStudio.Utilities;

public class OpenLocationEditor : UITypeEditor
{
    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext? context) => UITypeEditorEditStyle.Modal;
    public override object? EditValue(ITypeDescriptorContext? context, IServiceProvider provider, object? value)
    {
        if (value is string filePath)
        {
            var dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir))
            {
                WinformsUtilities.OpenExplorer(dir);
            }
            return value;
        }
        return base.EditValue(context, provider, value);
    }
}
