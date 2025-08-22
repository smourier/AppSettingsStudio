namespace AppSettingsStudio.Monaco;

public class MonacoEventArgs(MonacoEventType type, string? json = null) : HandledEventArgs
{
    private readonly Lazy<JsonDocument?> _document = new(() =>
    {
        if (json == null)
            return null;

        return JsonSerializer.Deserialize<JsonDocument>(json);
    });

    public MonacoEventType EventType { get; } = type;
    public string? Json { get; } = json;
    public JsonDocument? Document => _document.Value;
    public JsonElement RootElement => (Document?.RootElement).GetValueOrDefault();

    public override string ToString()
    {
        var str = EventType.ToString();
        if (Json != null)
        {
            str += " " + Json;
        }
        return str;
    }
}
