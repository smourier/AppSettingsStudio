namespace AppSettingsStudio.Utilities;

public struct POINT
{
    [JsonInclude]
    public int x;

    [JsonInclude]
    public int y;

    public readonly Point Point => new(x, y);
}
