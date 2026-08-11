namespace AppSettingsStudio.Utilities;

public sealed class LocalizedCategoryAttribute(string resourceKey)
    : CategoryAttribute(Res.ResourceManager.GetString(resourceKey + "Category") ?? resourceKey)
{
}
