namespace AppSettingsStudio.Configuration;

[Flags]
public enum SettingsOptions
{
    None = 0x0,

    // this is needed at least once to gather the initial appsettings file
    // otherwise nothing will be shown in the AppSettings Studio UI.
    GatherAppSettingsFile = 0x1,

    MonitorChanges = 0x2,
}
