namespace AppSettingsStudio.Utilities;

public static partial class EncodingDetector
{
    public static Encoding UTF8NoBom { get; } = new UTF8Encoding(false);

    public static Bom ReadBom(this Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        Span<byte> bytes = stackalloc byte[4];
        var read = stream.Read(bytes);
        if (read >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
        {
            stream.Seek(3, SeekOrigin.Begin);
            return Bom.Utf8;
        }

        if (read >= 2 && bytes[0] == 0xFF && bytes[1] == 0xFE)
        {
            stream.Seek(2, SeekOrigin.Begin);
            return Bom.Utf16Le;
        }

        if (read >= 2 && bytes[0] == 0xFE && bytes[1] == 0xFF)
        {
            stream.Seek(2, SeekOrigin.Begin);
            return Bom.Utf16Be;
        }

        if (read == 4 && bytes[0] == 0xFE && bytes[1] == 0xFF && bytes[2] == 0 && bytes[3] == 0)
            return Bom.Utf32Le;

        if (read >= 4 && bytes[0] == 0 && bytes[1] == 0 && bytes[2] == 0xFE && bytes[3] == 0xFF)
            return Bom.Utf32Be;

        stream.Seek(0, SeekOrigin.Begin);
        return Bom.None;
    }

    public static Encoding DetectEncoding(string filePath, EncodingDetectorMode mode = EncodingDetectorMode.AutoDetect, int autoDetectTestBytesLength = 1024)
    {
        ArgumentNullException.ThrowIfNull(filePath);
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return DetectEncoding(stream, mode, autoDetectTestBytesLength);
    }

    public static Encoding DetectEncoding(Stream stream, EncodingDetectorMode mode = EncodingDetectorMode.AutoDetect, int autoDetectTestBytesLength = 1024)
    {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(autoDetectTestBytesLength, 64);

        Encoding defaultEncoding;
        switch (mode)
        {
            case EncodingDetectorMode.UseUTF8AsDefault:
                defaultEncoding = Encoding.UTF8;
                break;

            case EncodingDetectorMode.UseAnsiAsDefault:
                defaultEncoding = Encoding.Default;
                break;

            default:
                return DetectEncoding(stream, autoDetectTestBytesLength);
        }

        using var reader = new StreamReader(stream, defaultEncoding, true);
        reader.Peek();
        return reader.CurrentEncoding;
    }

    public static string ReadAllText(string filePath, EncodingDetectorMode mode) => ReadAllText(filePath, mode, out _);
    public static string ReadAllText(string filePath, EncodingDetectorMode mode, out Encoding detectedEncoding)
    {
        ArgumentNullException.ThrowIfNull(filePath);

        Encoding defaultEncoding;
        switch (mode)
        {
            case EncodingDetectorMode.UseUTF8AsDefault:
                defaultEncoding = Encoding.UTF8;
                break;

            case EncodingDetectorMode.UseAnsiAsDefault:
                defaultEncoding = Encoding.Default;
                break;

            default:
                return ReadAllText(filePath, out detectedEncoding);
        }

        using (var reader = new StreamReader(filePath, defaultEncoding, true))
        {
            reader.Peek();
            detectedEncoding = reader.CurrentEncoding;
        }

        return File.ReadAllText(filePath, detectedEncoding);
    }

    public static string ReadAllText(Stream stream, EncodingDetectorMode mode = EncodingDetectorMode.AutoDetect) => ReadAllText(stream, mode, out _);
    public static string ReadAllText(Stream stream, out Encoding detectedEncoding) => ReadAllText(stream, EncodingDetectorMode.AutoDetect, out detectedEncoding);
    public static string ReadAllText(Stream stream, EncodingDetectorMode mode, out Encoding detectedEncoding)
    {
        ArgumentNullException.ThrowIfNull(stream);
        Encoding defaultEncoding;
        switch (mode)
        {
            case EncodingDetectorMode.UseUTF8AsDefault:
                defaultEncoding = Encoding.UTF8;
                break;

            case EncodingDetectorMode.UseAnsiAsDefault:
                defaultEncoding = Encoding.Default;
                break;

            default:
                return DoReadAllText(stream, out detectedEncoding);
        }

        using (var reader = new StreamReader(stream, defaultEncoding, true, leaveOpen: true))
        {
            reader.Peek();
            detectedEncoding = reader.CurrentEncoding;
        }
        return ReadAllText(stream, detectedEncoding);
    }

    public static string ReadText(Stream stream, int maximumSize, EncodingDetectorMode mode = EncodingDetectorMode.AutoDetect) => ReadText(stream, maximumSize, mode, out _);
    public static string ReadText(Stream stream, int maximumSize, out Encoding detectedEncoding) => ReadText(stream, maximumSize, EncodingDetectorMode.AutoDetect, out detectedEncoding);
    public static string ReadText(Stream stream, int maximumSize, EncodingDetectorMode mode, out Encoding detectedEncoding)
    {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maximumSize);
        if (maximumSize == int.MaxValue)
            return ReadAllText(stream, mode, out detectedEncoding);

        Encoding defaultEncoding;
        switch (mode)
        {
            case EncodingDetectorMode.UseUTF8AsDefault:
                defaultEncoding = Encoding.UTF8;
                break;

            case EncodingDetectorMode.UseAnsiAsDefault:
                defaultEncoding = Encoding.Default;
                break;

            default:
                return DoReadText(stream, maximumSize, out detectedEncoding);
        }

        using (var reader = new StreamReader(stream, defaultEncoding, true, leaveOpen: true))
        {
            reader.Peek();
            detectedEncoding = reader.CurrentEncoding;
        }

        return ReadText(stream, maximumSize, detectedEncoding);
    }

    // https://devblogs.microsoft.com/oldnewthing/20190701-00/?p=102636
    [LibraryImport("kernel32")]
    private static partial int MultiByteToWideChar(int CodePage, int dwFlags, in byte[] lpMultiByteStr, int cbMultiByte, [Out] ushort[] lpWideCharStr, int cchWideChar);

    private static readonly char[] _to1252Table = Init1252Table();
    private static char[] Init1252Table()
    {
        var as8bit = new byte[32];
        for (var i = 0; i < as8bit.Length; i++)
        {
            as8bit[i] = (byte)(i + 0x80);
        }

        var table = new ushort[32];
        _ = MultiByteToWideChar(1252, 0, as8bit, as8bit.Length, table, table.Length);
        return [.. table.Select(us => (char)us)];
    }

    private static byte To1252(char ch)
    {
        if (ch < 0x100)
            return (byte)ch;

        for (var i = 0; i < _to1252Table.Length; i++)
        {
            if (_to1252Table[i] == ch)
                return (byte)(i + 0x80);
        }
        return 0;
    }

    private static string ReadAllText(string filePath, out Encoding detectedEncoding)
    {
        using (var reader = new StreamReader(filePath, UTF8NoBom, true))
        {
            reader.Peek();
            if (reader.CurrentEncoding != UTF8NoBom) // is there a BOM?
            {
                detectedEncoding = reader.CurrentEncoding;
                return File.ReadAllText(filePath, detectedEncoding);
            }
        }

        var bytes = File.ReadAllBytes(filePath);
        var p = Encoding.UTF8.GetString(bytes);
        for (var i = 0; i < p.Length; i++)
        {
            if (IsSuspicious(p, i))
            {
                detectedEncoding = Encoding.Default;
                return detectedEncoding.GetString(bytes);
            }
        }

        detectedEncoding = UTF8NoBom;
        return p;
    }

    private static string DoReadAllText(Stream stream, out Encoding detectedEncoding)
    {
        using (var reader = new StreamReader(stream, UTF8NoBom, true, leaveOpen: true))
        {
            reader.Peek();
            if (reader.CurrentEncoding != UTF8NoBom) // is there a BOM?
            {
                detectedEncoding = reader.CurrentEncoding;
                return ReadAllText(stream, detectedEncoding);
            }
        }

        var bytes = ReadAllBytes(stream);
        var p = Encoding.UTF8.GetString(bytes);
        for (var i = 0; i < p.Length; i++)
        {
            if (IsSuspicious(p, i))
            {
                detectedEncoding = Encoding.Default;
                return detectedEncoding.GetString(bytes);
            }
        }

        detectedEncoding = UTF8NoBom;
        return p;
    }

    private static string ReadAllText(Stream stream, Encoding encoding)
    {
        var bytes = ReadAllBytes(stream);
        return encoding.GetString(bytes, 0, bytes.Length);
    }

    private static string ReadText(Stream stream, int maximumSize, Encoding encoding)
    {
        var bytes = ReadBytes(stream, maximumSize);
        return encoding.GetString(bytes, 0, bytes.Length);
    }

    private static byte[] ReadAllBytes(Stream stream)
    {
        stream.Position = 0;
        var bytes = new byte[stream.Length];
        var read = stream.Read(bytes, 0, bytes.Length);
        if (read < bytes.Length)
        {
            Array.Resize(ref bytes, read);
        }
        return bytes;
    }

    private static byte[] ReadBytes(Stream stream, int maximumSize)
    {
        stream.Position = 0;
        var bytes = new byte[Math.Min(maximumSize, stream.Length)];
        var read = stream.Read(bytes, 0, bytes.Length);
        if (read < bytes.Length)
        {
            Array.Resize(ref bytes, read);
        }
        return bytes;
    }

    private static string DoReadText(Stream stream, int maximumSize, out Encoding detectedEncoding)
    {
        using (var reader = new StreamReader(stream, UTF8NoBom, true, leaveOpen: true))
        {
            reader.Peek();
            if (reader.CurrentEncoding != UTF8NoBom) // is there a BOM?
            {
                detectedEncoding = reader.CurrentEncoding;
                return ReadText(stream, maximumSize, detectedEncoding);
            }
        }

        var bytes = ReadBytes(stream, maximumSize);
        var p = Encoding.UTF8.GetString(bytes);
        for (var i = 0; i < p.Length; i++)
        {
            if (IsSuspicious(p, i))
            {
                detectedEncoding = Encoding.Default;
                return detectedEncoding.GetString(bytes);
            }
        }

        detectedEncoding = UTF8NoBom;
        return p;
    }

    private static bool IsSuspicious(string str, int i)
    {
        char c = str[i];
        if (c == 0xFFFD)
            return true;

        var ch = To1252(c);
        if (ch > 0x7F)
        {
            if ((c & 0xE0) == 0xC0 && i + 1 < str.Length && (To1252(str[i + 1]) & 0xC0) == 0x80)
                return true;

            if ((c & 0xF0) == 0xE0 && i + 2 < str.Length && (To1252(str[i + 1]) & 0xC0) == 0x80 && (To1252(str[i + 2]) & 0xC0) == 0x80)
                return true;

            if ((c & 0xF8) == 0xF0 && i + 3 < str.Length && (To1252(str[i + 1]) & 0xC0) == 0x80 && (To1252(str[i + 2]) & 0xC0) == 0x80 && (To1252(str[i + 3]) & 0xC0) == 0x80)
                return true;
        }
        return false;
    }

    private static Encoding DetectEncoding(Stream stream, int autoDetectTestBytesLength)
    {
        var reader = new StreamReader(stream, UTF8NoBom, true, leaveOpen: true);
        reader.Peek();
        if (reader.CurrentEncoding != UTF8NoBom) // is there a BOM?
            return reader.CurrentEncoding;

        if (autoDetectTestBytesLength > stream.Length)
        {
            autoDetectTestBytesLength = (int)stream.Length;
        }

        if (autoDetectTestBytesLength % 2 == 1)
        {
            autoDetectTestBytesLength--;
        }

        var bytes = new byte[autoDetectTestBytesLength];
        var read = stream.Read(bytes, 0, bytes.Length);
        var str = Encoding.UTF8.GetString(bytes, 0, read);
        for (var i = 0; i < str.Length; i++)
        {
            if (IsSuspicious(str, i))
                return Encoding.Default;
        }
        return UTF8NoBom;
    }
}
