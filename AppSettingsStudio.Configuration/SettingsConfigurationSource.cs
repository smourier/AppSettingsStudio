namespace AppSettingsStudio.Configuration;

public class SettingsConfigurationSource : IConfigurationSource
{
    public virtual string? ApplicationName { get; set; }
    public virtual string? RootPath { get; set; }
    public virtual SettingsOptions Options { get; set; }
#if DEBUG
        = SettingsOptions.GatherAppSettingsFile;
#endif

    public IConfigurationProvider Build(IConfigurationBuilder builder) => new SettingsConfigurationProvider(this);
}
