namespace AppSettingsStudio.Configuration;

public interface IWithFilePath
{
    bool IsReadOnly { get; }
    string? FilePath { get; }
}
