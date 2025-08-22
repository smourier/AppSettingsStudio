namespace AppSettingsStudio.Monaco;

public static class MonacoExtensions
{
    public static JsonSerializerOptions SerializerOptions { get; } = new() { PropertyNameCaseInsensitive = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    public static bool LanguagesLoaded { get; private set; }

    private static readonly ConcurrentDictionary<string, MonacoLanguageExtensionPoint> _languagesById = new(StringComparer.OrdinalIgnoreCase);
    private static readonly ConcurrentDictionary<string, IReadOnlyList<MonacoLanguageExtensionPoint>> _languagesByExtension = new(StringComparer.OrdinalIgnoreCase);
    private static bool _loadingLanguages;

    public static async Task LoadLanguages(WebView2 webView)
    {
        ArgumentNullException.ThrowIfNull(webView);
        if (LanguagesLoaded)
            return;

        if (_loadingLanguages)
        {
            do
            {
                await Task.Delay(20);
            }
            while (_loadingLanguages);
            return;
        }

        _loadingLanguages = true;

        var json = await webView.ExecuteScriptAsync("monaco.languages.getLanguages()");
        var languages = JsonSerializer.Deserialize<MonacoLanguageExtensionPoint[]>(json, SerializerOptions);
        if (languages != null)
        {
            foreach (var language in languages)
            {
                if (language == null || language.Id == null)
                    continue;

                _languagesById[language.Id] = language;
                if (language.Extensions != null)
                {
                    foreach (var ext in language.Extensions)
                    {
                        if (!_languagesByExtension.TryGetValue(ext, out var list))
                        {
                            var l = new List<MonacoLanguageExtensionPoint>();
                            list = l;
                            _languagesByExtension[ext] = list;
                        }
                        ((List<MonacoLanguageExtensionPoint>)list).Add(language);
                    }
                }
            }
        }

        if (!_languagesByExtension.IsEmpty)
        {
            // TODO: add some well-known languages that are not recognized by Monaco
            addExtensionLike(".idl", ".c");

            static void addExtensionLike(string ext, string likeExt)
            {
                if (_languagesByExtension.ContainsKey(ext))
                    return;

                if (!_languagesByExtension.TryGetValue(likeExt, out var list) || list.Count == 0)
                    return;

                var first = list.FirstOrDefault(l => l.Extensions != null) ?? list[0];
                if (first.Extensions == null)
                {
                    first.Extensions = [ext];
                }
                else
                {
                    var exts = new List<string>(first.Extensions)
                    {
                        ext
                    };
                    first.Extensions = [.. exts];
                }
                _languagesByExtension[ext] = list;
            }
        }

        _loadingLanguages = false;
        LanguagesLoaded = true;
    }

    public static string? GetLanguageName(string id)
    {
        if (!LanguagesLoaded)
            throw new InvalidOperationException();

        if (id == null)
            return null;

        var languages = GetLanguages();
        languages.TryGetValue(id, out var lang);
        if (lang != null)
            return lang.Name;

        return null;
    }

    public static IDictionary<string, IReadOnlyList<MonacoLanguageExtensionPoint>> GetLanguagesByExtension()
    {
        if (!LanguagesLoaded)
            throw new InvalidOperationException();

        return _languagesByExtension;
    }

    public static IDictionary<string, MonacoLanguageExtensionPoint> GetLanguages()
    {
        if (!LanguagesLoaded)
            throw new InvalidOperationException();

        return _languagesById;
    }

    public static string? GetLanguageByExtension(string ext)
    {
        if (string.IsNullOrWhiteSpace(ext) || !LanguagesLoaded)
            return MonacoLanguageExtensionPoint.DefaultLanguageId;

        if (_languagesByExtension.TryGetValue(ext, out var langs) && langs.Count > 0)
            return langs[0].Id;

        using var key = Registry.ClassesRoot.OpenSubKey(ext, false);
        if (key != null)
        {
            var ct = key.GetValue("Content Type") as string;
            if (!string.IsNullOrWhiteSpace(ct))
            {
                using var mime = Registry.ClassesRoot.OpenSubKey(Path.Combine(@"MIME\Database\Content Type", ct), false);
                var mimeExt = (mime?.GetValue("Extension") as string)?.Nullify();
                if (mimeExt != null && _languagesByExtension.TryGetValue(mimeExt, out langs) && langs.Count > 0)
                    return langs[0].Id;
            }
        }

        return MonacoLanguageExtensionPoint.DefaultLanguageId;
    }

    public static bool IsUnknownLanguageExtension(string? ext)
    {
        if (string.IsNullOrWhiteSpace(ext))
            return true;

        if (LanguagesLoaded)
        {
            var languages = GetLanguagesByExtension();
            if (languages.TryGetValue(ext, out var list) && list.Count > 0)
                return false;
        }

        using var key = Registry.ClassesRoot.OpenSubKey(ext, false);
        if (key == null)
            return true;

        var ct = key.GetValue("Content Type") as string;
        if (string.IsNullOrWhiteSpace(ct))
            return true;
        return false;
    }

    public static string? UnescapeEditorText(string? text)
    {
        if (text == null)
            return null;

        if (text.Length > 1 && text[0] == '"' && text[^1] == '"')
        {
            text = text[1..^1];
        }
        return Regex.Unescape(text);
    }

    public static async Task<T?> ExecuteScriptAsync<T>(this WebView2 webView, string javaScript, T? defaultValue = default, JsonSerializerOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(javaScript);
        if (webView == null)
            return defaultValue;

        try
        {
            var json = await webView.ExecuteScriptAsync(javaScript);
            if (json == null)
                return defaultValue;

            return JsonSerializer.Deserialize<T>(json, options);
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex);
            return defaultValue;
        }
    }
}
