namespace AppSettingsStudio.Utilities;

public static class Conversions
{
    private static readonly char[] _enumSeparators = [',', ';', '+', '|', ' '];
    private static readonly string[] _dateFormatsUtc = ["yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", "yyyy'-'MM'-'dd'T'HH':'mm'Z'", "yyyyMMdd'T'HH':'mm':'ss'Z'"];

    public static Type? GetEnumeratedType(Type collectionType)
    {
        ArgumentNullException.ThrowIfNull(collectionType);

        var etype = GetEnumeratedItemType(collectionType);
        if (etype != null)
            return etype;

        foreach (var type in collectionType.GetInterfaces())
        {
            etype = GetEnumeratedItemType(type);
            if (etype != null)
                return etype;
        }
        return null;
    }

    private static Type? GetEnumeratedItemType(Type type)
    {
        if (!type.IsGenericType)
            return null;

        if (type.GetGenericArguments().Length != 1)
            return null;

        if (type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            return type.GetGenericArguments()[0];

        if (type.GetGenericTypeDefinition() == typeof(ICollection<>))
            return type.GetGenericArguments()[0];

        if (type.GetGenericTypeDefinition() == typeof(IList<>))
            return type.GetGenericArguments()[0];

        if (type.GetGenericTypeDefinition() == typeof(ISet<>))
            return type.GetGenericArguments()[0];

        if (type.GetGenericTypeDefinition() == typeof(IReadOnlyCollection<>))
            return type.GetGenericArguments()[0];

        if (type.GetGenericTypeDefinition() == typeof(IReadOnlyList<>))
            return type.GetGenericArguments()[0];

        if (type.GetGenericTypeDefinition() == typeof(IReadOnlySet<>))
            return type.GetGenericArguments()[0];

        if (type.GetGenericTypeDefinition() == typeof(IAsyncEnumerable<>))
            return type.GetGenericArguments()[0];

        return null;
    }

    public static IList<string> SplitToNullifiedList(this string? text, char[] separators, int count = int.MaxValue, StringSplitOptions options = StringSplitOptions.None)
    {
        var list = new List<string>();
        if (!string.IsNullOrEmpty(text))
        {
            foreach (var str in text.Split(separators, count, options))
            {
                var s = str.Nullify();
                if (s != null)
                {
                    list.Add(s);
                }
            }
        }
        return list;
    }

    public static IList<T> SplitToList<T>(this string? text, char[] separators, IFormatProvider? provider = null, int count = int.MaxValue, StringSplitOptions options = StringSplitOptions.None)
    {
        var list = new List<T>();
        if (!string.IsNullOrEmpty(text))
        {
            foreach (var str in text.Split(separators, count, options))
            {
                var newStr = str;
                if (newStr != null && options.HasFlag(StringSplitOptions.TrimEntries))
                {
                    newStr = newStr.Trim();
                }

                if (string.IsNullOrEmpty(newStr) && options.HasFlag(StringSplitOptions.RemoveEmptyEntries))
                    continue;

                if (TryChangeType<T>(newStr, provider, out var value) && value != null)
                {
                    list.Add(value);
                }
            }
        }
        return list;
    }

    public static bool IsNullable(this Type type) { ArgumentNullException.ThrowIfNull(type); return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>); }
    public static bool IsReallyValueType(this Type type) { ArgumentNullException.ThrowIfNull(type); return type.IsValueType && !type.IsNullable(); }

    public static object? ChangeObjectType(object? input, Type conversionType, object? defaultValue = null, IFormatProvider? provider = null)
    {
        if (!TryChangeObjectType(input, conversionType, provider, out object? value))
        {
            if (TryChangeObjectType(defaultValue, conversionType, provider, out var def))
                return def;

            return GetDefaultValue(conversionType);
        }
        return value;
    }

    public static T? ChangeType<T>(object? input, T? defaultValue = default, IFormatProvider? provider = null)
    {
        if (!TryChangeType(input, provider, out T? value))
            return defaultValue;

        return value;
    }

    public static bool TryChangeObjectType(object? input, Type conversionType, out object? value) => TryChangeObjectType(input, conversionType, null, out value);
    public static bool TryChangeObjectType(object? input, Type conversionType, IFormatProvider? provider, out object? value)
    {
        ArgumentNullException.ThrowIfNull(conversionType);
        if (input == null)
        {
            value = GetDefaultValue(conversionType);
            return true;
        }

        var inputType = input.GetType();
        if (conversionType.IsAssignableFrom(inputType))
        {
            value = input;
            return true;
        }

        if (conversionType.IsNullable())
        {
            var firstType = conversionType.GetGenericArguments()[0];
            if (TryChangeObjectType(input, firstType, provider, out object? vtValue))
            {
                var nt = typeof(Nullable<>).MakeGenericType(firstType);
                value = Activator.CreateInstance(nt, vtValue);
                return true;
            }

            value = null;
            return false;
        }

        if (conversionType.IsEnum)
            return EnumTryParse(conversionType, input, out value);

        if (inputType.IsEnum)
            return TryGetEnumValue(input, conversionType, inputType, out value);

        if (conversionType == typeof(int))
        {
            if (inputType == typeof(uint))
            {
                value = unchecked((int)(uint)input);
                return true;
            }

            if (inputType == typeof(ulong))
            {
                value = unchecked((int)(ulong)input);
                return true;
            }

            if (inputType == typeof(ushort))
            {
                value = unchecked((int)(ushort)input);
                return true;
            }

            if (inputType == typeof(byte))
            {
                value = unchecked((int)(byte)input);
                return true;
            }
        }

        if (conversionType == typeof(long))
        {
            if (inputType == typeof(uint))
            {
                value = unchecked((long)(uint)input);
                return true;
            }

            if (inputType == typeof(ulong))
            {
                value = unchecked((long)(ulong)input);
                return true;
            }

            if (inputType == typeof(ushort))
            {
                value = unchecked((long)(ushort)input);
                return true;
            }

            if (inputType == typeof(byte))
            {
                value = unchecked((long)(byte)input);
                return true;
            }

            if (inputType == typeof(TimeSpan))
            {
                value = ((TimeSpan)input).Ticks;
                return true;
            }

            if (inputType == typeof(DateTime))
            {
                value = ((DateTime)input).Ticks;
                return true;
            }

            if (inputType == typeof(DateTimeOffset))
            {
                value = ((DateTimeOffset)input).Ticks;
                return true;
            }
        }

        if (conversionType == typeof(short))
        {
            if (inputType == typeof(uint))
            {
                value = unchecked((short)(uint)input);
                return true;
            }

            if (inputType == typeof(ulong))
            {
                value = unchecked((short)(ulong)input);
                return true;
            }

            if (inputType == typeof(ushort))
            {
                value = unchecked((short)(ushort)input);
                return true;
            }

            if (inputType == typeof(byte))
            {
                value = unchecked((short)(byte)input);
                return true;
            }
        }

        if (conversionType == typeof(sbyte))
        {
            if (inputType == typeof(uint))
            {
                value = unchecked((sbyte)(uint)input);
                return true;
            }

            if (inputType == typeof(ulong))
            {
                value = unchecked((sbyte)(ulong)input);
                return true;
            }

            if (inputType == typeof(ushort))
            {
                value = unchecked((sbyte)(ushort)input);
                return true;
            }

            if (inputType == typeof(byte))
            {
                value = unchecked((sbyte)(byte)input);
                return true;
            }
        }

        if (conversionType == typeof(uint))
        {
            if (inputType == typeof(int))
            {
                value = unchecked((uint)(int)input);
                return true;
            }

            if (inputType == typeof(long))
            {
                value = unchecked((uint)(long)input);
                return true;
            }

            if (inputType == typeof(short))
            {
                value = unchecked((uint)(short)input);
                return true;
            }

            if (inputType == typeof(sbyte))
            {
                value = unchecked((uint)(sbyte)input);
                return true;
            }
        }

        if (conversionType == typeof(ulong))
        {
            if (inputType == typeof(int))
            {
                value = unchecked((ulong)(int)input);
                return true;
            }

            if (inputType == typeof(long))
            {
                value = unchecked((ulong)(long)input);
                return true;
            }

            if (inputType == typeof(short))
            {
                value = unchecked((ulong)(short)input);
                return true;
            }

            if (inputType == typeof(sbyte))
            {
                value = unchecked((ulong)(sbyte)input);
                return true;
            }
        }

        if (conversionType == typeof(ushort))
        {
            if (inputType == typeof(int))
            {
                value = unchecked((ushort)(int)input);
                return true;
            }

            if (inputType == typeof(long))
            {
                value = unchecked((ushort)(long)input);
                return true;
            }

            if (inputType == typeof(short))
            {
                value = unchecked((ushort)(short)input);
                return true;
            }

            if (inputType == typeof(sbyte))
            {
                value = unchecked((ushort)(sbyte)input);
                return true;
            }
        }

        if (conversionType == typeof(byte))
        {
            if (inputType == typeof(int))
            {
                value = unchecked((byte)(int)input);
                return true;
            }

            if (inputType == typeof(long))
            {
                value = unchecked((byte)(long)input);
                return true;
            }

            if (inputType == typeof(short))
            {
                value = unchecked((byte)(short)input);
                return true;
            }

            if (inputType == typeof(sbyte))
            {
                value = unchecked((byte)(sbyte)input);
                return true;
            }
        }

        if (conversionType == typeof(bool))
        {
            if (true.Equals(input))
            {
                value = true;
                return true;
            }

            if (false.Equals(input))
            {
                value = false;
                return true;
            }

            var svalue = string.Format(provider, "{0}", input).Nullify();
            if (svalue == null)
            {
                value = false;
                return false;
            }

            if (bool.TryParse(svalue, out bool b))
            {
                value = b;
                return true;
            }

            if (svalue.EqualsIgnoreCase("y") || svalue.EqualsIgnoreCase("yes"))
            {
                value = true;
                return true;
            }

            if (svalue.EqualsIgnoreCase("n") || svalue.EqualsIgnoreCase("no"))
            {
                value = false;
                return true;
            }

            if (TryChangeType(input, out long bl))
            {
                value = bl != 0;
                return true;
            }

            value = false;
            return false;
        }

        if (conversionType == typeof(DateTime))
        {
            if (inputType == typeof(long))
            {
                value = new DateTime((long)input, DateTimeKind.Utc);
                return true;
            }

            if (inputType == typeof(DateTimeOffset))
            {
                value = ((DateTimeOffset)input).DateTime;
                return true;
            }

            if (TryChangeToDateTime(input, provider, DateTimeStyles.None, out var dt))
            {
                value = dt;
                return true;
            }
        }

        if (conversionType == typeof(DateTimeOffset))
        {
            if (inputType == typeof(long))
            {
                value = new DateTimeOffset(new DateTime((long)input, DateTimeKind.Utc));
                return true;
            }

            if (inputType == typeof(DateTime))
            {
                var dt = (DateTime)input;
                if (dt.IsValid())
                {
                    value = new DateTimeOffset((DateTime)input);
                    return true;
                }
            }

            if (TryChangeToDateTime(input, provider, DateTimeStyles.None, out var dto))
            {
                value = new DateTimeOffset(dto);
                return true;
            }
        }

        if (conversionType == typeof(TimeSpan))
        {
            if (inputType == typeof(long))
            {
                value = new TimeSpan((long)input);
                return true;
            }

            if (inputType == typeof(DateTime))
            {
                value = ((DateTime)input).TimeOfDay;
                return true;
            }

            if (inputType == typeof(DateTimeOffset))
            {
                value = ((DateTimeOffset)input).TimeOfDay;
                return true;
            }

            if (TryChangeType(input, provider, out string? sv) && TimeSpan.TryParse(sv, provider, out var ts))
            {
                value = ts;
                return true;
            }
        }

        if (conversionType == typeof(Guid))
        {
            if (input is byte[] bytes && bytes.Length == 16)
            {
                value = new Guid(bytes);
                return true;
            }

            var svalue = string.Format(provider, "{0}", input).Nullify();
            if (svalue != null && Guid.TryParse(svalue, out Guid guid))
            {
                value = guid;
                return true;
            }

            value = GetDefaultValue(conversionType);
            return false;
        }

        if (conversionType == typeof(Uri))
        {
            var svalue = string.Format(provider, "{0}", input).Nullify();
            if (svalue != null && Uri.TryCreate(svalue, UriKind.RelativeOrAbsolute, out var uri))
            {
                value = uri;
                return true;
            }

            value = GetDefaultValue(conversionType);
            return false;
        }

        if (conversionType == typeof(nint))
        {
            if (nint.Size == 8)
            {
                if (TryChangeType(input, provider, out long l))
                {
                    value = new nint(l);
                    return true;
                }
            }
            else if (TryChangeType(input, provider, out int i))
            {
                value = new nint(i);
                return true;
            }

            value = GetDefaultValue(conversionType);
            return false;
        }

        // in general, nothing is convertible to anything but one of these, IConvertible is 100% stupid thing
        bool isWellKnownConvertible()
        {
            return conversionType == typeof(short) || conversionType == typeof(int) ||
                conversionType == typeof(string) || conversionType == typeof(byte) ||
                conversionType == typeof(char) || conversionType == typeof(DateTime) ||
                conversionType == typeof(DBNull) || conversionType == typeof(decimal) ||
                conversionType == typeof(double) || conversionType.IsEnum ||
                conversionType == typeof(short) || conversionType == typeof(int) ||
                conversionType == typeof(long) || conversionType == typeof(sbyte) ||
                conversionType == typeof(bool) || conversionType == typeof(float) ||
                conversionType == typeof(ushort) || conversionType == typeof(uint) ||
                conversionType == typeof(ulong);
        }

        if (isWellKnownConvertible() && input is IConvertible convertible)
        {
            try
            {
                value = convertible.ToType(conversionType, provider);
                if (value is DateTime dt && !dt.IsValid())
                    return false;

                return true;
            }
            catch
            {
                // continue;
            }
        }

        if (conversionType == typeof(string))
        {
            value = string.Format(provider, "{0}", input);
            return true;
        }

        value = GetDefaultValue(conversionType);
        return false;
    }

    public static bool TryChangeToDateTime(object? input, DateTimeStyles styles, out DateTime value) => TryChangeToDateTime(input, null, styles, out value);
    public static bool TryChangeToDateTime(object? input, IFormatProvider? provider, DateTimeStyles styles, out DateTime value)
    {
        if (input == null)
        {
            value = DateTime.MinValue;
            return false;
        }

        if (input is long il)
        {
            if (styles.HasFlag(DateTimeStyles.AssumeLocal))
            {
                value = new DateTime(il, DateTimeKind.Local);
            }
            else
            {
                value = new DateTime(il, DateTimeKind.Utc);
            }
            return true;
        }

        if (input is double dbl)
        {
            try
            {
                value = DateTime.FromOADate(dbl);
                return true;
            }
            catch
            {
                value = DateTime.MinValue;
                return false;
            }
        }

        if (input is DateTimeOffset offset)
        {
            value = offset.DateTime;
            return true;
        }

        var text = string.Format(provider, "{0}", input).Nullify();
        if (text == null)
        {
            value = DateTime.MinValue;
            return false;
        }

        if (DateTime.TryParse(text, provider, styles, out value))
            return true;

        DateTime dt;
        // 01234567890123456
        // 20150525T15:50:00
        if (text != null && text.Length == 17)
        {
            if ((text[8] == 'T' || text[8] == 't') && text[11] == ':' && text[14] == ':')
            {
                _ = int.TryParse(text.AsSpan(0, 4), out var year);
                _ = int.TryParse(text.AsSpan(4, 2), out var month);
                _ = int.TryParse(text.AsSpan(6, 2), out var day);
                _ = int.TryParse(text.AsSpan(9, 2), out var hour);
                _ = int.TryParse(text.AsSpan(12, 2), out var minute);
                _ = int.TryParse(text.AsSpan(15, 2), out var second);
                if (month > 0 && month < 13 &&
                    day > 0 && day < 32 &&
                    year >= 0 &&
                    hour >= 0 && hour < 24 &&
                    minute >= 0 && minute < 60 &&
                    second >= 0 && second < 60)
                {
                    try
                    {
                        dt = new DateTime(year, month, day, hour, minute, second);
                        value = dt;
                        return true;
                    }
                    catch
                    {
                        // do nothing
                    }
                }
            }
        }

        if (text != null && text.EndsWith("Z", StringComparison.OrdinalIgnoreCase))
        {
            if (DateTime.TryParseExact(text, _dateFormatsUtc, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out dt))
            {
                value = dt;
                return true;
            }
        }

        if (long.TryParse(text, out var l))
        {
            if (styles.HasFlag(DateTimeStyles.AssumeLocal))
            {
                value = new DateTime(l, DateTimeKind.Local);
            }
            else
            {
                value = new DateTime(l, DateTimeKind.Utc);
            }
            return true;
        }

        if (double.TryParse(text, provider, out dbl))
        {
            try
            {
                value = DateTime.FromOADate(dbl);
                return true;
            }
            catch
            {
                value = DateTime.MinValue;
                return false;
            }
        }
        return false;
    }

    public static object ToEnum(string text, Type enumType)
    {
        ArgumentNullException.ThrowIfNull(enumType);
        _ = EnumTryParse(enumType, text, out object value);
        return value;
    }

    public static Enum ToEnum(string text, Enum defaultValue)
    {
        if (EnumTryParse(defaultValue.GetType(), text, out object value))
            return (Enum)value;

        return defaultValue;
    }

    public static object ToEnum(Type type, string text)
    {
        EnumTryParse(type, text, out var value);
        return value;
    }

    public static bool EnumTryParse(Type type, object input, out object value)
    {
        ArgumentNullException.ThrowIfNull(type);
        if (!type.IsEnum)
            throw new ArgumentException(null, nameof(type));

        if (input == null)
        {
            value = Activator.CreateInstance(type)!;
            return false;
        }

        var stringInput = string.Format(CultureInfo.InvariantCulture, "{0}", input);
        stringInput = stringInput.Nullify();
        if (stringInput == null)
        {
            value = Activator.CreateInstance(type)!;
            return false;
        }

        if (stringInput.StartsWith("0x", StringComparison.OrdinalIgnoreCase) && ulong.TryParse(stringInput.AsSpan(2), NumberStyles.HexNumber, null, out var ulx))
        {
            value = ToEnum(type, ulx.ToString(CultureInfo.InvariantCulture));
            return true;
        }

        var names = Enum.GetNames(type);
        if (names.Length == 0)
        {
            value = Activator.CreateInstance(type)!;
            return false;
        }

        // this is ok for enums
        var values = type.GetFields(BindingFlags.Public | BindingFlags.Static).Select(f => new { name = f.Name, value = f.GetValue(null) }).ToDictionary(x => x.name, x => x.value!, StringComparer.OrdinalIgnoreCase);
        // some enums like System.CodeDom.MemberAttributes *are* flags but are not declared with Flags...
        if (!type.IsDefined(typeof(FlagsAttribute), true) && stringInput.IndexOfAny(_enumSeparators) < 0)
            return StringToEnum(type, values, stringInput, out value);

        // multi value enum
        var tokens = stringInput.Split(_enumSeparators, StringSplitOptions.RemoveEmptyEntries);
        if (tokens.Length == 0)
        {
            value = Activator.CreateInstance(type)!;
            return false;
        }

        ulong ul = 0;
        foreach (var tok in tokens)
        {
            var token = tok.Nullify(); // NOTE: we don't consider empty tokens as errors
            if (token == null)
                continue;

            if (!StringToEnum(type, values, token, out var tokenValue))
            {
                value = Activator.CreateInstance(type)!;
                return false;
            }

            var tokenUl = Convert.GetTypeCode(tokenValue) switch
            {
                TypeCode.Int16 or TypeCode.Int32 or TypeCode.Int64 or TypeCode.SByte => (ulong)Convert.ToInt64(tokenValue, CultureInfo.InvariantCulture),
                _ => Convert.ToUInt64(tokenValue, CultureInfo.InvariantCulture),
            };
            ul |= tokenUl;
        }
        value = Enum.ToObject(type, ul);
        return true;
    }

    private static bool StringToEnum(Type type, Dictionary<string, object> values, string input, out object value)
    {
        if (values.TryGetValue(input, out value!))
            return true;

        foreach (var kv in values)
        {
            if (input.Length > 0 && input[0] == '-')
            {
                var ul = (long)EnumToUInt64(kv.Value);
                if (ul.ToString().EqualsIgnoreCase(input))
                {
                    value = kv.Value;
                    return true;
                }
            }
            else
            {
                var ul = EnumToUInt64(kv.Value);
                if (ul.ToString().EqualsIgnoreCase(input))
                {
                    value = kv.Value;
                    return true;
                }
            }
        }

        if (char.IsDigit(input[0]) || input[0] == '-' || input[0] == '+')
        {
            var obj = EnumToObject(type, input);
            if (obj == null)
            {
                value = Activator.CreateInstance(type)!;
                return false;
            }

            value = obj;
            return true;
        }

        value = Activator.CreateInstance(type)!;
        return false;
    }

    public static ulong EnumToUInt64(object? value)
    {
        if (value == null)
            return 0;

        var typeCode = Convert.GetTypeCode(value);
        return typeCode switch
        {
            TypeCode.SByte or TypeCode.Int16 or TypeCode.Int32 or TypeCode.Int64 => (ulong)Convert.ToInt64(value, CultureInfo.InvariantCulture),
            TypeCode.Byte or TypeCode.UInt16 or TypeCode.UInt32 or TypeCode.UInt64 => Convert.ToUInt64(value, CultureInfo.InvariantCulture),
            _ => ChangeType<ulong>(value, 0, CultureInfo.InvariantCulture),
        };
    }

    public static object EnumToObject(Type type, object value)
    {
        ArgumentNullException.ThrowIfNull(type);
        if (!type.IsEnum)
            throw new ArgumentException(null, nameof(type));

        var underlyingType = Enum.GetUnderlyingType(type);
        if (underlyingType == typeof(long))
            return Enum.ToObject(type, ChangeType<long>(value));

        if (underlyingType == typeof(ulong))
            return Enum.ToObject(type, ChangeType<ulong>(value));

        if (underlyingType == typeof(int))
            return Enum.ToObject(type, ChangeType<int>(value));

        if (underlyingType == typeof(uint))
            return Enum.ToObject(type, ChangeType<uint>(value));

        if (underlyingType == typeof(short))
            return Enum.ToObject(type, ChangeType<short>(value));

        if (underlyingType == typeof(ushort))
            return Enum.ToObject(type, ChangeType<ushort>(value));

        if (underlyingType == typeof(byte))
            return Enum.ToObject(type, ChangeType<byte>(value));

        if (underlyingType == typeof(sbyte))
            return Enum.ToObject(type, ChangeType<sbyte>(value));

        throw new NotSupportedException();
    }

    private static bool TryGetEnumValue(object input, Type conversionType, Type inputType, out object? value)
    {
        var tc = Type.GetTypeCode(inputType);
        if (conversionType == typeof(int))
        {
            switch (tc)
            {
                case TypeCode.Int32:
                    value = (int)input;
                    return true;

                case TypeCode.Int16:
                    value = (int)(short)input;
                    return true;

                case TypeCode.Int64:
                    value = (int)(long)input;
                    return true;

                case TypeCode.UInt32:
                    value = (int)(uint)input;
                    return true;

                case TypeCode.UInt16:
                    value = (int)(ushort)input;
                    return true;

                case TypeCode.UInt64:
                    value = (int)(ulong)input;
                    return true;

                case TypeCode.Byte:
                    value = (int)(byte)input;
                    return true;

                case TypeCode.SByte:
                    value = (int)(sbyte)input;
                    return true;
            }

            value = GetDefaultValue(conversionType);
            return false;
        }

        if (conversionType == typeof(short))
        {
            switch (tc)
            {
                case TypeCode.Int32:
                    value = (short)(int)input;
                    return true;

                case TypeCode.Int16:
                    value = (short)input;
                    return true;

                case TypeCode.Int64:
                    value = (short)(long)input;
                    return true;

                case TypeCode.UInt32:
                    value = (short)(uint)input;
                    return true;

                case TypeCode.UInt16:
                    value = (short)(ushort)input;
                    return true;

                case TypeCode.UInt64:
                    value = (short)(ulong)input;
                    return true;

                case TypeCode.Byte:
                    value = (short)(byte)input;
                    return true;

                case TypeCode.SByte:
                    value = (short)(sbyte)input;
                    return true;
            }

            value = GetDefaultValue(conversionType);
            return false;
        }

        if (conversionType == typeof(long))
        {
            switch (tc)
            {
                case TypeCode.Int32:
                    value = (long)(int)input;
                    return true;

                case TypeCode.Int16:
                    value = (long)(short)input;
                    return true;

                case TypeCode.Int64:
                    value = (long)input;
                    return true;

                case TypeCode.UInt32:
                    value = (long)(uint)input;
                    return true;

                case TypeCode.UInt16:
                    value = (long)(ushort)input;
                    return true;

                case TypeCode.UInt64:
                    value = (long)(ulong)input;
                    return true;

                case TypeCode.Byte:
                    value = (long)(byte)input;
                    return true;

                case TypeCode.SByte:
                    value = (long)(sbyte)input;
                    return true;
            }

            value = GetDefaultValue(conversionType);
            return false;
        }

        if (conversionType == typeof(uint))
        {
            switch (tc)
            {
                case TypeCode.Int32:
                    value = (uint)(int)input;
                    return true;

                case TypeCode.Int16:
                    value = (uint)(short)input;
                    return true;

                case TypeCode.Int64:
                    value = (uint)(long)input;
                    return true;

                case TypeCode.UInt32:
                    value = (uint)input;
                    return true;

                case TypeCode.UInt16:
                    value = (uint)(ushort)input;
                    return true;

                case TypeCode.UInt64:
                    value = (uint)(ulong)input;
                    return true;

                case TypeCode.Byte:
                    value = (uint)(byte)input;
                    return true;

                case TypeCode.SByte:
                    value = (uint)(sbyte)input;
                    return true;
            }

            value = GetDefaultValue(conversionType);
            return false;
        }

        if (conversionType == typeof(ushort))
        {
            switch (tc)
            {
                case TypeCode.Int32:
                    value = (ushort)(int)input;
                    return true;

                case TypeCode.Int16:
                    value = (ushort)(short)input;
                    return true;

                case TypeCode.Int64:
                    value = (ushort)(long)input;
                    return true;

                case TypeCode.UInt32:
                    value = (ushort)(uint)input;
                    return true;

                case TypeCode.UInt16:
                    value = (ushort)input;
                    return true;

                case TypeCode.UInt64:
                    value = (ushort)(ulong)input;
                    return true;

                case TypeCode.Byte:
                    value = (ushort)(byte)input;
                    return true;

                case TypeCode.SByte:
                    value = (ushort)(sbyte)input;
                    return true;
            }

            value = GetDefaultValue(conversionType);
            return false;
        }

        if (conversionType == typeof(ulong))
        {
            switch (tc)
            {
                case TypeCode.Int32:
                    value = (ulong)(int)input;
                    return true;

                case TypeCode.Int16:
                    value = (ulong)(short)input;
                    return true;

                case TypeCode.Int64:
                    value = (ulong)(long)input;
                    return true;

                case TypeCode.UInt32:
                    value = (ulong)(uint)input;
                    return true;

                case TypeCode.UInt16:
                    value = (ulong)(ushort)input;
                    return true;

                case TypeCode.UInt64:
                    value = (ulong)input;
                    return true;

                case TypeCode.Byte:
                    value = (ulong)(byte)input;
                    return true;

                case TypeCode.SByte:
                    value = (ulong)(sbyte)input;
                    return true;
            }

            value = GetDefaultValue(conversionType);
            return false;
        }

        if (conversionType == typeof(byte))
        {
            switch (tc)
            {
                case TypeCode.Int32:
                    value = (byte)(int)input;
                    return true;

                case TypeCode.Int16:
                    value = (byte)(short)input;
                    return true;

                case TypeCode.Int64:
                    value = (byte)(long)input;
                    return true;

                case TypeCode.UInt32:
                    value = (byte)(uint)input;
                    return true;

                case TypeCode.UInt16:
                    value = (byte)(ushort)input;
                    return true;

                case TypeCode.UInt64:
                    value = (byte)(ulong)input;
                    return true;

                case TypeCode.Byte:
                    value = (byte)input;
                    return true;

                case TypeCode.SByte:
                    value = (byte)(sbyte)input;
                    return true;
            }

            value = GetDefaultValue(conversionType);
            return false;
        }

        if (conversionType == typeof(sbyte))
        {
            switch (tc)
            {
                case TypeCode.Int32:
                    value = (sbyte)(int)input;
                    return true;

                case TypeCode.Int16:
                    value = (sbyte)(short)input;
                    return true;

                case TypeCode.Int64:
                    value = (sbyte)(long)input;
                    return true;

                case TypeCode.UInt32:
                    value = (sbyte)(uint)input;
                    return true;

                case TypeCode.UInt16:
                    value = (sbyte)(ushort)input;
                    return true;

                case TypeCode.UInt64:
                    value = (sbyte)(ulong)input;
                    return true;

                case TypeCode.Byte:
                    value = (sbyte)(byte)input;
                    return true;

                case TypeCode.SByte:
                    value = (sbyte)input;
                    return true;
            }

            value = GetDefaultValue(conversionType);
            return false;
        }

        value = GetDefaultValue(conversionType);
        return false;
    }

    private static object? GetDefaultValue(Type type)
    {
        if (type.IsReallyValueType())
            return Activator.CreateInstance(type)!;
        return null;
    }

    public static bool TryChangeType<T>(object? input, out T? value) => TryChangeType(input, null, out value);
    public static bool TryChangeType<T>(object? input, IFormatProvider? provider, out T? value)
    {
        if (!TryChangeObjectType(input, typeof(T), provider, out object? tvalue))
        {
            value = default;
            return false;
        }

        value = (T?)tvalue;
        return true;
    }

    public static bool IsFlagsEnum(Type enumType)
    {
        ArgumentNullException.ThrowIfNull(enumType);
        if (!enumType.IsEnum)
            throw new ArgumentException(null, nameof(enumType));

        return enumType.IsDefined(typeof(FlagsAttribute), true);
    }

    public static string? GetNullifiedValue(this JsonElement element, string jsonPath) => element.GetValue<string>(jsonPath, null).Nullify();
    public static string? GetNullifiedValue(this IDictionary<string, JsonElement> element, string key, string? defaultValue = null, IFormatProvider? provider = null)
    {
        ArgumentNullException.ThrowIfNull(key);

        if (element == null)
            return defaultValue.Nullify();

        if (!element.TryGetValue(key, out var o))
            return defaultValue;

        return ChangeType(o, defaultValue, provider).Nullify();
    }

    public static T? GetValue<T>(this JsonElement element, string jsonPath, T? defaultValue = default)
    {
        if (!element.TryGetValue<T>(jsonPath, out var value))
            return defaultValue;

        return value;
    }

    public static bool TryGetValue<T>(this JsonElement element, string jsonPath, out T? value)
    {
        if (!element.TryGetObjectValue(jsonPath, out var obj))
        {
            value = default;
            return false;
        }

        return TryChangeType(obj, out value);
    }

    public static bool TryGetObjectValue(this JsonElement element, string jsonPath, out object? value)
    {
        ArgumentNullException.ThrowIfNull(jsonPath);

        if (element.TryGetProperty(jsonPath, out var directElement))
        {
            value = directElement.ToObject();
            return true;
        }

        value = null;
        var segments = jsonPath.Split('.');
        var current = element;
        for (var i = 0; i < segments.Length; i++)
        {
            var segment = segments[i].Nullify();
            if (segment == null)
                return false;

            if (!current.TryGetProperty(segment, out var newElement))
                return false;

            if (i == segments.Length - 1)
            {
                value = newElement.ToObject();
                return true;
            }
            current = newElement;
        }
        return false;
    }

    public static object? ToObject(this JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Null:
                return null;

            case JsonValueKind.Object:
                var dic = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
                foreach (var child in element.EnumerateObject())
                {
                    dic[child.Name] = child.Value.ToObject();
                }
                return dic;

            case JsonValueKind.Array:
                var objects = new object?[element.GetArrayLength()];
                var i = 0;
                foreach (var child in element.EnumerateArray())
                {
                    objects[i++] = child.ToObject();
                }
                return objects;

            case JsonValueKind.String:
                var str = element.ToString();
                if (DateTime.TryParseExact(str, ["o", "r", "s"], null, DateTimeStyles.None, out var dt))
                    return dt;

                return str;

            case JsonValueKind.Number:
                if (element.TryGetInt32(out var i32))
                    return i32;

                if (element.TryGetInt32(out var i64))
                    return i64;

                if (element.TryGetDecimal(out var dec))
                    return dec;

                if (element.TryGetDouble(out var dbl))
                    return dbl;

                throw new NotSupportedException();

            case JsonValueKind.True:
                return true;

            case JsonValueKind.False:
                return false;

            default:
                throw new NotSupportedException();
        }
    }
}
