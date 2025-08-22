namespace AppSettingsStudio.Monaco;

public class MonacoConfigurationChangedEventArgs(string? json)
    : MonacoEventArgs(MonacoEventType.ConfigurationChanged, json)
{
    // https://microsoft.github.io/monaco-editor/typedoc/enums/editor.EditorOption.html#fontSize
    public int Index => RootElement.GetValue("index", -1);
    public MonacoEditorOption Option => (MonacoEditorOption)Index;

    public override string ToString() => EventType + " " + Option;
}
