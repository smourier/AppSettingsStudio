namespace AppSettingsStudio.Resources;

public static class MonacoResources
{
    [NotNull]
    [MaybeNull]
    private static string MonacoFilesDirectoryPath { get; set; }

    [NotNull]
    [MaybeNull]
    public static string IndexFilePath { get; set; }

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

        const string indexName = "index.html";
        IndexFilePath = Path.Combine(MonacoFilesDirectoryPath, indexName);
        using var indexStream = asm.GetManifestResourceStream(typeof(MonacoResources).Namespace + "." + indexName) ?? throw new InvalidOperationException();

        IOUtilities.FileEnsureDirectory(IndexFilePath);
        using var index = new FileStream(IndexFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
        indexStream.CopyTo(index);

        // we check the last known file is there
        var someFile = Path.Combine(MonacoFilesDirectoryPath, @"vs\language\typescript\tsWorker.js");
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
}
