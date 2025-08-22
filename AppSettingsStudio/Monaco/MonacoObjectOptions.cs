namespace AppSettingsStudio.Monaco;

#pragma warning disable IDE1006 // Naming Styles
public class MonacoObjectOptions
{
    public bool automaticLayout { get; set; } = true;
    public string? language { get; set; } // = "plaintext";
    public string? fontSize { get; set; } = "13px";
    public bool dragAndDrop { get; set; } = false;
    public bool mouseWheelZoom { get; set; } = true;
    public bool contextmenu { get; set; } = false;
    public string? theme { get; set; } = "vs";
}
#pragma warning restore IDE1006 // Naming Styles
