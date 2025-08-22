namespace AppSettingsStudio.Monaco;

public partial class MonacoEditorControl : Control
{
    [Conditional("DEBUG")]
    public static void Trace(object? value = null, [CallerMemberName] string? methodName = null)
    {
#if DEBUG
        System.Diagnostics.Trace.WriteLine(methodName + ":" + value);
#endif
    }

    private readonly MonacoObject _controlObject = new();
    private string _text = string.Empty;
    private string? _document;
    private readonly Task _webView2Initialized;

    public event EventHandler<MonacoEventArgs>? Event;

    public MonacoEditorControl()
    {
        _controlObject.Load += OnMonacoLoad;
        _controlObject.Event += OnMonacoEvent;
        InitializeComponent();
        WebView.Dock = DockStyle.Fill;
        Controls.Add(WebView);

        WebView.CoreWebView2InitializationCompleted += CoreWebView2InitializationCompleted;
        var env = CoreWebView2Environment.CreateAsync(userDataFolder: Settings.WebView2UserDataPath);
        _webView2Initialized = WebView.EnsureCoreWebView2Async(env.Result); // calling Result might not be ideal I guess, but simpler here
    }

    private async void CoreWebView2InitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e)
    {
        await Program._monacoInstalledTask;
        WebView.CoreWebView2.ContextMenuRequested += (s, args) => args.Handled = true;
        WebView.CoreWebView2.AddHostObjectToScript("settings", _controlObject);
        WebView.Source = new Uri(MonacoResources.IndexFilePath);
    }

    private void OnMonacoLoad(object? sender, MonacoLoadEventArgs e)
    {
        if (_document != null)
        {
            e.DocumentText = _document;
            _document = null;
            if (IsReadOnly)
            {
                _ = SetReadOnly(true);
            }
        }
    }

    private async void OnMonacoEvent(object? sender, MonacoEventArgs e)
    {
        switch (e.EventType)
        {
            case MonacoEventType.EditorCreated:
                if (!MonacoExtensions.LanguagesLoaded)
                {
                    await MonacoExtensions.LoadLanguages(WebView);
                }

                await EnableMinimap(Settings.Current.JsonMinimap);
                await SetEditorTheme(Settings.Current.JsonTheme);
                await FocusEditor();
                break;
        }

        Event?.Invoke(this, e);
    }

    public Task ExecuteScript(string javaScript, JsonSerializerOptions? options = null) => WebView.ExecuteScriptAsync<object>(javaScript, null, options);
    public Task<T?> ExecuteScriptAsync<T>(string javaScript, T? defaultValue = default, JsonSerializerOptions? options = null) => WebView.ExecuteScriptAsync(javaScript, defaultValue, options);

    public override string Text
    {
        get => _text;
#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        set
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        {
            if (_text == value)
                return;

            _ = LoadText(value ?? string.Empty);
        }
    }

    public bool IsReadOnly { get; private set; }

    public async Task LoadText(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        await SetReadOnly(false);
        _document = text;
        await ExecuteScript($"loadFromHost()");
        _text = text;
        BeginInvoke(() => OnTextChanged(EventArgs.Empty));
    }

    public async Task SetReadOnly(bool readOnly)
    {
        await ExecuteScript("editor.updateOptions({readOnly:" + readOnly.ToString().ToLowerInvariant() + "});");
        IsReadOnly = readOnly;
    }

    public Task<string?> GetEditorText() => ExecuteScriptAsync<string>("editor.getValue()");
    public Task<bool> EditorHasFocus() => WebView.ExecuteScriptAsync<bool>("editor.hasTextFocus()");
    public Task BlurEditor() => ExecuteScript("editor.blur()");
    public Task SetEditorPosition(int lineNumber = 0, int column = 0) => ExecuteScript("editor.setPosition({lineNumber:" + lineNumber + ",column:" + column + "})");
    public Task MoveEditorTo(int? line = null, int? column = null) => ExecuteScript($"moveEditorTo({column}, {line})");
    public Task EditorRevealLineInCenter(int lineNumber = 0) => ExecuteScript("editor.revealLineInCenter(" + lineNumber + ")");
    public Task EnableMinimap(bool enabled) => ExecuteScript("editor.updateOptions({minimap:{enabled:" + enabled.ToString().ToLowerInvariant() + "}})");
    public async Task SetEditorTheme(string? theme = null) { theme = theme.Nullify() ?? "vs"; await ExecuteScript($"monaco.editor.setTheme('{theme}')"); }
    public Task SetFontSize(double size) => ExecuteScript("editor.updateOptions({fontSize:'" + size.ToString(CultureInfo.InvariantCulture) + "px'})");
    public async Task FocusEditor() { await ExecuteScript("editor.focus()"); WebView.Focus(); }
    public Task FormatDocument() => ExecuteScript("editor.getAction('editor.action.formatDocument').run()");
    public async Task SetEditorLanguage(string lang)
    {
        ArgumentNullException.ThrowIfNull(lang);
        await ExecuteScript($"monaco.editor.setModelLanguage(editor.getModel(), '{lang}');");
    }
}
