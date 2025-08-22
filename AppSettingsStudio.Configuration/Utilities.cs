namespace AppSettingsStudio.Configuration;

internal static class Utilities
{
    public static bool EqualsIgnoreCase(this string? thisString, string? text, bool trim = true)
    {
        if (trim)
        {
            thisString = thisString.Nullify();
            text = text.Nullify();
        }

        if (thisString == null)
            return text == null;

        if (text == null)
            return false;

        if (thisString.Length != text.Length)
            return false;

        return string.Compare(thisString, text, StringComparison.OrdinalIgnoreCase) == 0;
    }

    public static string? Nullify(this string? text)
    {
        if (text == null)
            return null;

        if (string.IsNullOrWhiteSpace(text))
            return null;

        var t = text.Trim();
        return t.Length == 0 ? null : t;
    }

    public static bool PathEnsureDirectory(string path, bool throwOnError = true)
    {
        ArgumentNullException.ThrowIfNull(path);
        if (!Path.IsPathRooted(path))
        {
            path = Path.GetFullPath(path);
        }

        var dir = Path.GetDirectoryName(path);
        if (dir == null || Directory.Exists(dir))
            return false;

        try
        {
            Directory.CreateDirectory(dir);
            return true;
        }
        catch
        {
            if (throwOnError)
                throw;

            return false;
        }
    }

    public static string ComputeHashString(this string text) => ComputeGuidHash(text).ToString("N");
    public static Guid ComputeGuidHash(this string text)
    {
        ArgumentNullException.ThrowIfNull(text);
        return new Guid(XxHash128.Hash(Encoding.UTF8.GetBytes(text)));
    }

    public static bool PathIsChild(string path, string child, bool normalize = true) => PathIsChild(path, child, normalize, out _);
    public static bool PathIsChild(string path, string child, bool normalize, out string? subPath)
    {
        ArgumentNullException.ThrowIfNull(path);
        ArgumentNullException.ThrowIfNull(child);
        subPath = null;
        try
        {
            if (normalize)
            {
                path = Path.GetFullPath(path);
                child = Path.GetFullPath(child);
            }

            path = StripTerminatingPathSeparators(path)!;
            if (child.Length < (path.Length + 1))
                return false;

            var newChild = Path.Combine(path, child[(path.Length + 1)..]);
            var b = newChild.EqualsIgnoreCase(child);
            if (b)
            {
                subPath = child[path.Length..];
                while (subPath.StartsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    subPath = subPath[1..];
                }
            }
            return b;
        }
        catch
        {
            return false;
        }
    }

    [return: NotNullIfNotNull(nameof(path))]
    public static string? StripTerminatingPathSeparators(string path)
    {
        if (path == null)
            return null;

        while (path.EndsWith(Path.DirectorySeparatorChar.ToString()))
        {
            path = path[..^1];
        }
        return path;
    }

    public static string? GetNullifiedString(this IDictionary<string, object?>? dictionary, string name, IFormatProvider? provider = null)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (dictionary == null)
            return null;

        if (!dictionary.TryGetValue(name, out var obj) || obj == null)
            return null;

        return string.Format(provider, "{0}", obj).Nullify();
    }

    public static void SaveDictionaryAsJson(IDictionary<string, object?>? dictionary, Stream utf8Json, JsonSerializerOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(utf8Json);
        if (dictionary == null)
            return;

        JsonSerializer.Serialize(utf8Json, dictionary, options);
    }

    public static void SaveDictionaryAsJson(IDictionary<string, object?>? dictionary, string filePath, JsonSerializerOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(filePath);
        PathEnsureDirectory(filePath);
        using var file = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
        SaveDictionaryAsJson(dictionary, file, options);
    }

    public static IDictionary<string, object?> LoadJsonAsDictionary(Stream utf8Stream, JsonSerializerOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(utf8Stream);
        var obj = JsonSerializer.Deserialize<JsonElement?>(utf8Stream, options);
        if (obj != null && ConvertToObject(obj.Value, null) is IDictionary<string, object?> dic)
            return dic;

        return new Dictionary<string, object?>(StringComparer.Ordinal);
    }

    public static IDictionary<string, object?> LoadJsonAsDictionary(string path, JsonSerializerOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(path);
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path, Encoding.UTF8);
            var obj = JsonSerializer.Deserialize<JsonElement?>(json, options);
            if (obj != null && ConvertToObject(obj.Value, null) is IDictionary<string, object?> dic)
                return dic;
        }
        return new Dictionary<string, object?>(StringComparer.Ordinal);
    }

    public static object? ConvertToObject(this JsonElement element, object? defaultValue)
    {
        if (!TryConvertToObject(element, out var value))
            return defaultValue;

        return value;
    }

    public static bool TryConvertToObject(this JsonElement element, out object? value)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Null:
                value = null;
                return true;

            case JsonValueKind.Object:
                var dic = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
                foreach (var child in element.EnumerateObject())
                {
                    if (!TryConvertToObject(child.Value, out var childValue))
                    {
                        value = null;
                        return false;
                    }

                    dic[child.Name] = childValue;
                }

                // empty dic => null
                if (dic.Count == 0)
                {
                    value = null;
                    return true;
                }

                value = dic;
                return true;

            case JsonValueKind.Array:
                var objects = new object?[element.GetArrayLength()];
                var i = 0;
                foreach (var child in element.EnumerateArray())
                {
                    if (!TryConvertToObject(child, out var childValue))
                    {
                        value = null;
                        return false;
                    }

                    objects[i++] = childValue;
                }

                value = objects;
                return true;

            case JsonValueKind.String:
                var str = element.ToString();
                if (DateTime.TryParseExact(str, ["o", "r", "s"], null, DateTimeStyles.None, out var dt))
                {
                    value = dt;
                    return true;
                }

                value = str;
                return true;

            case JsonValueKind.Number:
                if (element.TryGetInt32(out var i32))
                {
                    value = i32;
                    return true;
                }

                if (element.TryGetInt32(out var i64))
                {
                    value = i64;
                    return true;
                }

                if (element.TryGetDecimal(out var dec))
                {
                    value = dec;
                    return true;
                }

                if (element.TryGetDouble(out var dbl))
                {
                    value = dbl;
                    return true;
                }
                break;

            case JsonValueKind.True:
                value = true;
                return true;

            case JsonValueKind.False:
                value = false;
                return true;
        }

        value = null;
        return false;
    }

    public static void AddFromDirectory(this ZipArchive archive,
        string directoryPath,
        bool includeBaseDirectory = true,
        EnumerationOptions? enumerationOptions = null,
        CompressionLevel compressionLevel = CompressionLevel.Optimal,
        Func<FileSystemInfo, bool>? includeEntry = null)
    {
        ArgumentNullException.ThrowIfNull(archive);
        ArgumentNullException.ThrowIfNull(directoryPath);

        var rootDir = new DirectoryInfo(directoryPath);
        var basePath = rootDir.FullName;

        if (includeBaseDirectory && rootDir.Parent != null)
        {
            basePath = rootDir.Parent.FullName;
        }

        foreach (var info in rootDir.EnumerateFiles("*.*", enumerationOptions ?? new EnumerationOptions { RecurseSubdirectories = true }))
        {
            if (includeEntry != null && !includeEntry.Invoke(info))
                continue;

            if (info is FileInfo fi)
            {
                var entryName = fi.FullName.AsSpan(basePath.Length).ToString();
                while (entryName.StartsWith('/') || entryName.StartsWith('\\'))
                {
                    entryName = entryName[1..];
                }
                archive.CreateEntryFromFile(fi.FullName, entryName, compressionLevel);
            }
        }
    }

    public static bool MoveToRecycleBin(string fileOrDirectoryPath, bool deleteIfNotMoved = true)
    {
        if (!File.Exists(fileOrDirectoryPath) && !Directory.Exists(fileOrDirectoryPath))
            return false;

        if (OperatingSystem.IsWindows())
        {
            try
            {
                // from https://learn.microsoft.com/en-us/windows/win32/api/shldisp/ne-shldisp-shellspecialfolderconstants
                const int ssfBITBUCKET = 0xa;
                dynamic shell = Activator.CreateInstance(Type.GetTypeFromProgID("Shell.Application")!)!;
                var recycleBin = shell.Namespace(ssfBITBUCKET);
                recycleBin.MoveHere(fileOrDirectoryPath);
            }
            catch
            {
                // continue
            }
        }
        else
        {
            deleteIfNotMoved = true; // delete on other systems
        }

        if (deleteIfNotMoved)
        {
            if (File.Exists(fileOrDirectoryPath))
            {
                try
                {
                    File.Delete(fileOrDirectoryPath);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            if (Directory.Exists(fileOrDirectoryPath))
            {
                Directory.Delete(fileOrDirectoryPath, true);
                return true;
            }
        }
        return true;
    }

    // builder shorter names but keep them uniques
    public static IReadOnlyList<string> ShortenNames(IReadOnlyList<string> list, char[]? inputSeparators, string? outputSeparator, StringComparer? comparer = null)
    {
        ArgumentNullException.ThrowIfNull(list);
        comparer ??= StringComparer.OrdinalIgnoreCase;

        var newList = new List<StringList>();
        var splitList = new List<StringList>();
        var passedList = new List<StringList>();

        // initial step removes last segment
        for (var i = 0; i < list.Count; i++)
        {
            var split = list[i].Split(inputSeparators, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            splitList.Add(new StringList([.. split]));
            newList.Add(new StringList([splitList[i].List.Last()]));
            passedList.Add(new StringList([]));
        }

        if (list.Count <= 1)
            return [splitList[0].List.LastOrDefault() ?? string.Empty];

        do
        {
            var finished = 0;
            var lasts = new List<string?>();
            for (var i = 0; i < splitList.Count; i++)
            {
                var last = splitList[i].List.LastOrDefault();
                if (last == null)
                {
                    finished++;
                }
                lasts.Add(last);
            }

            if (finished == splitList.Count)
                break;

            // look back in passed elements see if we don't have it already
            do
            {
                var backed = false;
                var removed = new HashSet<int>();
                var equalsIndices = new HashSet<int>();
                for (var i = 0; i < splitList.Count; i++)
                {
                    if (passedList[i].Count == 0)
                        continue;

                    for (var j = 0; j < splitList.Count; j++)
                    {
                        if (j == i || passedList[j].Count == 0)
                            continue;

                        if (lasts[i].EqualsIgnoreCase(passedList[j].List[0]))
                        {
                            equalsIndices.Add(j);
                            removed.Add(i);
                        }
                    }
                }

                foreach (var i in removed)
                {
                    if (equalsIndices.Count > 0)
                    {
                        splitList[i].List.RemoveAt(splitList[i].Count - 1);
                        lasts[i] = splitList[i].List.LastOrDefault();
                        backed = true;
                    }

                    foreach (var j in equalsIndices)
                    {
                        var index = newList[j].Count - passedList[j].Count;
                        if (index < newList[j].List.Count)
                        {
                            newList[j].List.RemoveAt(index);
                        }

                        if (passedList[j].List.Count > 0)
                        {
                            passedList[j].List.RemoveAt(0);
                        }
                    }
                }

                if (!backed)
                    break;
            }
            while (true);

            var lastsHash = lasts.ToHashSet(StringComparer.OrdinalIgnoreCase).Count;
            // all same or all differents
            if (lastsHash == 1 || lastsHash == lasts.Count)
            {
                for (var i = 0; i < splitList.Count; i++)
                {
                    if (splitList[i].List.Count > 0)
                    {
                        splitList[i].List.RemoveAt(splitList[i].Count - 1);
                    }

                    if (lastsHash == lasts.Count)
                    {
                        var last = lasts[i];
                        if (last != null)
                        {
                            newList[i].List.Add(last);
                        }
                    }

                    passedList[i].List.Clear();
                }
                continue;
            }

            for (var i = 0; i < splitList.Count; i++)
            {
                if (splitList[i].List.Count > 0)
                {
                    splitList[i].List.RemoveAt(splitList[i].Count - 1);
                }

                var last = lasts[i];
                if (last != null)
                {
                    newList[i].List.Add(last);
                    passedList[i].List.Add(last);
                }
            }
        }
        while (true);

        // remove common beginning
        var sameIndex = 0;
        do
        {
            if (newList.Select(l => l.List.Count > 0 ? l.List[l.Count - sameIndex - 1] : null).ToHashSet(StringComparer.OrdinalIgnoreCase).Count != 1)
                break;

            sameIndex++;
        }
        while (true);

        if (sameIndex >= 0)
        {
            for (var i = 0; i < newList.Count; i++)
            {
                newList[i].List.RemoveRange(newList[i].List.Count - sameIndex, sameIndex);
            }
        }

        return [.. newList.Select(l => string.Join(outputSeparator, l.List.Reverse<string>()))];
    }

    private sealed class StringList(List<string> list)
    {
        public List<string> List { get; } = list;
        public int Count => List.Count;

        public override string ToString() => $"{string.Join(", ", List)}";
    }
}
