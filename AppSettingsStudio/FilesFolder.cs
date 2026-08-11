namespace AppSettingsStudio;

public sealed class FilesFolder
{
    public string Name { get; set; } = string.Empty;
    public IList<FilesFolder> Folders { get; set; } = [];
    public IList<string> Files { get; set; } = [];
    public IEnumerable<string> AllNames
    {
        get
        {
            foreach (var file in Files)
            {
                yield return file;
            }

            foreach (var folder in Folders)
            {
                yield return folder.Name;
                foreach (var file in folder.AllNames)
                {
                    yield return file;
                }
            }
        }
    }

    public override string ToString() => Name;
}
