namespace AppSettingsStudio;

public sealed partial class DiffForm : Form
{
    private readonly IEnumerable<Manager> _managers;
    private readonly List<FileSystemWatcher> _watchers = [];
    private System.Windows.Forms.Timer? _debounce;
    private CompareSide? _left;
    private CompareSide? _right;
    private bool _ready;

    internal DiffForm(IEnumerable<Manager> managers, CompareSide? left, CompareSide? right)
    {
        _managers = managers;
        _left = left;
        _right = right;

        InitializeComponent();
        Icon = Res.MainIcon;

        leftLabel.Text = Res.DiffLeftLabel;
        rightLabel.Text = Res.DiffRightLabel;
        optSideBySide.Text = Res.DiffSideBySide;
        optIgnoreWhitespace.Text = Res.DiffIgnoreWhitespace;
        optWordWrap.Text = Res.DiffWordWrap;
        optShowWhitespace.Text = Res.DiffShowWhitespace;
        refreshButton.Text = Res.DiffRefresh;

        optSideBySide.Checked = Settings.Current.DiffRenderSideBySide;
        optIgnoreWhitespace.Checked = Settings.Current.DiffIgnoreTrimWhitespace;
        optWordWrap.Checked = Settings.Current.DiffWordWrap;
        optShowWhitespace.Checked = Settings.Current.DiffRenderWhitespace;

        UpdateSideButtons();
        UpdateTitle();
    }

    private void OnLeftClick(object? sender, EventArgs e) => PickSide(true);
    private void OnRightClick(object? sender, EventArgs e) => PickSide(false);
    private void OnRefreshClick(object? sender, EventArgs e) => Render();

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        Settings.Current.RestorePlacement(this);
        Settings.Current.PropertyChanged += OnSettingsChanged;
        UpdateWatchers();
        _ = InitializeWebView2();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        Settings.Current.PropertyChanged -= OnSettingsChanged;
        StopWatching();
        _debounce?.Dispose();
        Settings.Current.SavePlacement(this);
        Settings.Current.SerializeToConfiguration();
        base.OnFormClosing(e);
    }

    private async Task InitializeWebView2()
    {
        await Program._monacoInstalledTask;
        if (IsDisposed)
            return;

        var env = await CoreWebView2Environment.CreateAsync(userDataFolder: Settings.WebView2UserDataPath);
        if (IsDisposed)
            return;

        await webView.EnsureCoreWebView2Async(env);
        if (IsDisposed || webView.CoreWebView2 == null)
            return;

        webView.CoreWebView2.ContextMenuRequested += (s, args) => args.Handled = true;
        webView.CoreWebView2.WebMessageReceived += OnWebMessageReceived;
        webView.Source = new Uri(MonacoResources.DiffFilePath);
    }

    private void OnSettingsChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Settings.DiffAutoRefresh))
        {
            UpdateWatchers();
        }
    }

    private void UpdateWatchers()
    {
        StopWatching();
        if (!Settings.Current.DiffAutoRefresh)
            return;

        WatchSide(_left);
        WatchSide(_right);
    }

    private void WatchSide(CompareSide? side)
    {
        if (side == null)
            return;

        var directory = Path.GetDirectoryName(side.FilePath);
        if (string.IsNullOrEmpty(directory) || !Directory.Exists(directory))
            return;

        var watcher = new FileSystemWatcher(directory, Path.GetFileName(side.FilePath))
        {
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.Size,
        };
        watcher.Changed += OnWatchedFileChanged;
        watcher.Created += OnWatchedFileChanged;
        watcher.Deleted += OnWatchedFileChanged;
        watcher.Renamed += OnWatchedFileChanged;
        watcher.EnableRaisingEvents = true;
        _watchers.Add(watcher);
    }

    private void StopWatching()
    {
        foreach (var watcher in _watchers)
        {
            watcher.EnableRaisingEvents = false;
            watcher.Dispose();
        }

        _watchers.Clear();
    }

    private void OnWatchedFileChanged(object sender, FileSystemEventArgs e)
    {
        if (IsDisposed)
            return;

        try
        {
            BeginInvoke(() =>
            {
                _debounce ??= CreateDebounceTimer();
                _debounce.Stop();
                _debounce.Start();
            });
        }
        catch
        {
            // race condition
        }
    }

    private System.Windows.Forms.Timer CreateDebounceTimer()
    {
        var timer = new System.Windows.Forms.Timer { Interval = 300 };
        timer.Tick += (s, e) =>
        {
            timer.Stop();
            Render();
        };
        return timer;
    }

    private void OnWebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
    {
        string json;
        try
        {
            json = e.TryGetWebMessageAsString();
        }
        catch
        {
            // don't care
            return;
        }

        var stats = JsonSerializer.Deserialize<DiffStats>(json, MonacoExtensions.SerializerOptions);
        if (stats == null)
            return;

        if (stats.Kind == "ready")
        {
            _ready = true;
            Render();
            return;
        }

        if (stats.Kind == "diffStats")
        {
            statusLabel.Text = stats.Count == 0 ? Res.DiffIdentical : string.Format(Res.DiffStatsFormat, stats.Count, stats.Added, stats.Removed);
        }
    }

    private void PickSide(bool left)
    {
        var current = left ? _left : _right;
        using var dlg = new BrowserForm(_managers, tag => CompareSide.FromTag(tag) != null, left ? Res.DiffChooseLeft : Res.DiffChooseRight, current?.FilePath, linkMode: false);
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;

        var side = CompareSide.FromTag(dlg.SelectedTag);
        if (side == null)
            return;

        if (left)
        {
            _left = side;
        }
        else
        {
            _right = side;
        }

        UpdateSideButtons();
        UpdateTitle();
        UpdateWatchers();
        Render();
    }

    private void OnOptionChanged(object? sender, EventArgs e)
    {
        Settings.Current.DiffRenderSideBySide = optSideBySide.Checked;
        Settings.Current.DiffIgnoreTrimWhitespace = optIgnoreWhitespace.Checked;
        Settings.Current.DiffWordWrap = optWordWrap.Checked;
        Settings.Current.DiffRenderWhitespace = optShowWhitespace.Checked;
        Post(BuildPayload("options"));
    }

    private void UpdateSideButtons()
    {
        leftButton.Text = _left != null ? Path.GetFileName(_left.FilePath) : Res.DiffChooseNode;
        leftButton.ToolTipText = _left?.FilePath ?? Res.DiffChooseLeft;
        rightButton.Text = _right != null ? Path.GetFileName(_right.FilePath) : Res.DiffChooseNode;
        rightButton.ToolTipText = _right?.FilePath ?? Res.DiffChooseRight;
    }

    private void UpdateTitle()
    {
        var product = AssemblyUtilities.GetProduct();
        Text = _left != null && _right != null ? $"{product} - {string.Format(Res.DiffTitleFormat, _left.Title, _right.Title)}" : $"{product} - {Res.DiffTitle}";
    }

    private void Render()
    {
        if (_left == null || _right == null)
        {
            statusLabel.Text = Res.DiffChooseTwoNodes;
            return;
        }

        if (!_ready)
            return;

        statusLabel.Text = Res.DiffComputing;
        var payload = BuildPayload("setDiff");
        payload.Original = ReadText(_left.FilePath);
        payload.Modified = ReadText(_right.FilePath);
        payload.OriginalLanguage = "json";
        payload.ModifiedLanguage = "json";
        payload.LeftTitle = _left.Title;
        payload.RightTitle = _right.Title;
        Post(payload);
    }

    private static string ReadText(string filePath)
    {
        if (!IOUtilities.PathIsFile(filePath))
            return string.Empty;

        try
        {
            return EncodingDetector.ReadAllText(filePath, EncodingDetectorMode.UseUTF8AsDefault, out _);
        }
        catch
        {
            return string.Empty;
        }
    }

    private static DiffPayload BuildPayload(string kind) => new()
    {
        Kind = kind,
        RenderSideBySide = Settings.Current.DiffRenderSideBySide,
        IgnoreTrimWhitespace = Settings.Current.DiffIgnoreTrimWhitespace,
        WordWrap = Settings.Current.DiffWordWrap,
        RenderWhitespace = Settings.Current.DiffRenderWhitespace,
        Theme = Settings.Current.JsonTheme.Nullify() ?? "vs",
    };

    private void Post(DiffPayload payload)
    {
        if (webView.CoreWebView2 == null)
            return;

        var json = JsonSerializer.Serialize(payload, MonacoExtensions.SerializerOptions);
        webView.CoreWebView2.PostWebMessageAsJson(json);
    }
}
