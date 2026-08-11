namespace AppSettingsStudio;

public sealed class FileItem(string filePath) : IWithFilePath
{
    public string FilePath { get; } = filePath;
    public string Name => Path.GetFileName(FilePath);

    string? IWithFilePath.FilePath => FilePath;

    bool IWithFilePath.IsReadOnly
    {
        get
        {
            try
            {
                if (!IOUtilities.PathIsFile(FilePath))
                    return true;

                return (File.GetAttributes(FilePath) & FileAttributes.ReadOnly) != 0;
            }
            catch
            {
                return true;
            }
        }
    }
}
