namespace AppSettingsStudio.Utilities;

public class CollectionListTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType) => destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (destinationType == typeof(string))
        {
            if (value is IEnumerable enumerable)
                return string.Join(",", enumerable.Cast<object>());

            return string.Empty;
        }
        return base.ConvertTo(context, culture, value, destinationType);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string str && context != null && context.PropertyDescriptor != null && context.PropertyDescriptor.PropertyType != null)
        {
            var itemType = Conversions.GetEnumeratedType(context.PropertyDescriptor.PropertyType);
            if (itemType != null)
            {
                var mi = typeof(Conversions).GetMethod(nameof(Conversions.SplitToList))!;
                var split = mi.MakeGenericMethod(itemType);
                var list = split.Invoke(null, [str, new object[] { ',' }, null, int.MaxValue, StringSplitOptions.RemoveEmptyEntries]);
                return list;
            }
        }
        return base.ConvertFrom(context, culture, value);
    }
}
