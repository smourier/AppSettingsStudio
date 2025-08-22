namespace AppSettingsStudio.Utilities;

public partial struct RECT(int left, int top, int right, int bottom)
{
    [JsonInclude]
    public int left = left;

    [JsonInclude]
    public int top = top;

    [JsonInclude]
    public int right = right;

    [JsonInclude]
    public int bottom = bottom;

    [JsonIgnore]
    public readonly int Width => right - left;

    [JsonIgnore]
    public readonly int Height => bottom - top;

    [JsonIgnore]
    public readonly Point LeftTop => new(left, top);

    [JsonIgnore]
    public readonly Point RightBottom => new(right, bottom);

    [JsonIgnore]
    public readonly Size Size => new(Width, Height);

    [JsonIgnore]
    public readonly Rectangle Rectangle => new(left, top, Width, Height);

    public override readonly string ToString() => left + "," + top + "," + right + "," + bottom;
}
