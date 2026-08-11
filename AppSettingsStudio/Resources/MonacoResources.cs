namespace AppSettingsStudio.Resources;

public static class MonacoResources
{
    [NotNull]
    [MaybeNull]
    private static string MonacoFilesDirectoryPath { get; set; }

    [NotNull]
    [MaybeNull]
    public static string IndexFilePath { get; set; }

    [NotNull]
    [MaybeNull]
    public static string DiffFilePath { get; set; }

    private static Task? _ensureMonacoFilesTask;
    private static readonly Lock _ensureMonacoFilesLock = new();

    public static Task EnsureMonacoFilesAsync()
    {
        if (_ensureMonacoFilesTask != null)
            return _ensureMonacoFilesTask;

        lock (_ensureMonacoFilesLock)
        {
            _ensureMonacoFilesTask ??= Task.Run(EnsureMonacoFiles);
            return _ensureMonacoFilesTask;
        }
    }

    public static void EnsureMonacoFiles()
    {
        var asm = Assembly.GetExecutingAssembly();
        var startTok = typeof(MonacoResources).Namespace + ".vs.";
        const string ext = ".zip";
        var zip = asm.GetManifestResourceNames().FirstOrDefault(n => n.StartsWith(startTok) && n.EndsWith(ext)) ?? throw new InvalidOperationException();
        var version = zip.Substring(startTok.Length, zip.Length - startTok.Length - ext.Length);
        MonacoFilesDirectoryPath = Path.Combine(Settings.TempDirectoryPath, "Monaco", version);

        IndexFilePath = ExtractHtml(asm, "index.html");
        DiffFilePath = ExtractHtml(asm, "Diff.html");

        // we check the last known file is there. the 0.55.x layout dropped vs\language\typescript\tsWorker.js,
        // so we probe monaco.contribution.js which the new build still ships.
        var someFile = Path.Combine(MonacoFilesDirectoryPath, @"vs\language\typescript\monaco.contribution.js");
        var fi = new FileInfo(someFile);
        if (fi.Exists && fi.Length > 0)
        {
            _ensureMonacoFilesTask = null;
            return;
        }

        using var stream = asm.GetManifestResourceStream(zip) ?? throw new InvalidOperationException();
        using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
        archive.ExtractToDirectory(MonacoFilesDirectoryPath, true);
        _ensureMonacoFilesTask = null;
    }

    private static string ExtractHtml(Assembly asm, string name)
    {
        var path = Path.Combine(MonacoFilesDirectoryPath, name);
        using var resourceStream = asm.GetManifestResourceStream(typeof(MonacoResources).Namespace + "." + name) ?? throw new InvalidOperationException();
        IOUtilities.FileEnsureDirectory(path);
        using var file = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
        resourceStream.CopyTo(file);
        return path;
    }
}
