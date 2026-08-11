namespace AppSettingsStudio.Utilities;

public sealed class LocalizedDisplayNameAttribute(string resourceKey)
    : DisplayNameAttribute(Res.ResourceManager.GetString(resourceKey + "DisplayName") ?? resourceKey)
{
}
