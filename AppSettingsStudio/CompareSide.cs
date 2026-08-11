namespace AppSettingsStudio;

internal sealed class CompareSide(string title, string filePath)
{
    public string Title { get; } = title;
    public string FilePath { get; } = filePath;

    public static CompareSide? FromTag(object? tag)
    {
        if (tag is IWithFilePath withFilePath && withFilePath.FilePath is { } filePath && IOUtilities.PathIsFile(filePath))
            return new CompareSide(filePath, filePath);

        return null;
    }
}
