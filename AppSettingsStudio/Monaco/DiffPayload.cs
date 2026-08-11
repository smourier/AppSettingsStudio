namespace AppSettingsStudio.Monaco;

public sealed class DiffPayload
{
    public string Kind { get; set; } = "setDiff";
    public string? Original { get; set; }
    public string? Modified { get; set; }
    public string? OriginalLanguage { get; set; }
    public string? ModifiedLanguage { get; set; }
    public string? LeftTitle { get; set; }
    public string? RightTitle { get; set; }
    public bool RenderSideBySide { get; set; }
    public bool IgnoreTrimWhitespace { get; set; }
    public bool WordWrap { get; set; }
    public bool RenderWhitespace { get; set; }
    public string? Theme { get; set; }
}
