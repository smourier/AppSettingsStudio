namespace AppSettingsStudio.Monaco;

public class MonacoLoadEventArgs() : MonacoEventArgs(MonacoEventType.Load)
{
    public string? DocumentText { get; set; }
}
