namespace AppSettingsStudio.Monaco;

public enum MonacoEventType
{
    Unknown = 0,
    Load = 1,
    ContentChanged = 2,
    KeyUp = 3,
    KeyDown = 4,
    EditorCreated = 5,
    EditorLostFocus = 6,
    EditorGotFocus = 7,
    CursorPositionChanged = 8,
    CursorSelectionChanged = 9,
    ModelLanguageChanged = 10,
    ConfigurationChanged = 11,
    OpenResource = 12,
    Paste = 13,
}
