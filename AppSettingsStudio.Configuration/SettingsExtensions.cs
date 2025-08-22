namespace AppSettingsStudio.Configuration;

public static class SettingsExtensions
{
    public static IConfigurationBuilder AddSettingsManager(this IConfigurationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Add(new SettingsConfigurationSource());
        return builder;
    }

    public static IConfigurationBuilder AddSettingsManager(this IConfigurationBuilder builder, string projectName)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Add(new SettingsConfigurationSource { ApplicationName = projectName });
        return builder;
    }

    public static IConfigurationBuilder AddSettingsManager(this IConfigurationBuilder builder, Action<SettingsConfigurationSource>? configureSource)
    {
        ArgumentNullException.ThrowIfNull(builder);
        return builder.Add(configureSource);
    }
}
