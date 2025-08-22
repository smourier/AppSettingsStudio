namespace AppSettingsStudio.Monaco;

// https://microsoft.github.io/monaco-editor/typedoc/classes/Range.html
public class MonacoRange(int startLineNumber, int startColumn, int endLineNumber, int endColumn)
{
    public int StartLineNumber { get; set; } = startLineNumber;
    public int StartColumn { get; set; } = startColumn;
    public int EndLineNumber { get; set; } = endLineNumber;
    public int EndColumn { get; set; } = endColumn;

    public override string ToString() => $"{StartLineNumber}:{StartColumn} - {EndLineNumber}:{EndColumn}";
}

