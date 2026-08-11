namespace AppSettingsStudio.Monaco;

public sealed class DiffStats
{
    public string? Kind { get; set; }
    public int Count { get; set; }
    public int Added { get; set; }
    public int Removed { get; set; }
}
