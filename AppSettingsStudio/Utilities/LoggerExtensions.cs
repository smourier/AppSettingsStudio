namespace AppSettingsStudio.Utilities;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA2254 // Template should be a static expression
public static class LoggerExtensions
{
    public static void LogError<T>(this ILoggable<T>? loggable, object? value, [CallerMemberName] string? methodName = null) => loggable.Log(LogLevel.Error, value, methodName);
    public static void LogError(this ILoggable? loggable, object? value, [CallerMemberName] string? methodName = null) => loggable.Log(LogLevel.Error, value, methodName);
    public static void LogError(this ILogger? logger, Exception value, [CallerMemberName] string? methodName = null)
    {
        if (logger == null || value == null)
            return;

        var msg = "(" + methodName + "): " + value;
        logger.Log(LogLevel.Error, msg);
    }

    public static void LogError(this ILogger? logger, object value, [CallerMemberName] string? methodName = null)
    {
        if (logger == null || value == null)
            return;

        var msg = "(" + methodName + "): " + value;
        logger.Log(LogLevel.Error, msg);
    }

    public static void LogInformation<T>(this ILoggable<T>? loggable, object? value, [CallerMemberName] string? methodName = null) => loggable.Log(LogLevel.Information, value, methodName);
    public static void LogInformation(this ILoggable? loggable, object? value, [CallerMemberName] string? methodName = null) => loggable.Log(LogLevel.Information, value, methodName);
    public static void LogInformation(this ILogger? logger, object value, [CallerMemberName] string? methodName = null)
    {
        if (logger == null || value == null)
            return;

        var msg = "(" + methodName + "): " + value;
        logger.Log(LogLevel.Information, msg);
    }

    public static void LogWarning<T>(this ILoggable<T> loggable, object? value, [CallerMemberName] string? methodName = null) => loggable.Log(LogLevel.Warning, value, methodName);
    public static void LogWarning(this ILoggable? loggable, object? value, [CallerMemberName] string? methodName = null) => loggable.Log(LogLevel.Warning, value, methodName);
    public static void LogWarning(this ILogger? logger, object value, [CallerMemberName] string? methodName = null)
    {
        if (logger == null || value == null)
            return;

        var msg = "(" + methodName + "): " + value;
        logger.Log(LogLevel.Warning, msg);
    }

    public static void LogDebug<T>(this ILoggable<T>? loggable, object? value, [CallerMemberName] string? methodName = null) => loggable.Log(LogLevel.Debug, value, methodName);
    public static void LogDebug(this ILoggable? loggable, object? value, [CallerMemberName] string? methodName = null) => loggable.Log(LogLevel.Debug, value, methodName);
    public static void LogDebug(this ILogger? logger, object value, [CallerMemberName] string? methodName = null)
    {
        if (logger == null || value == null)
            return;

        var msg = "(" + methodName + "): " + value;
        logger.Log(LogLevel.Debug, msg);
    }

    public static void LogCritical<T>(this ILoggable<T>? loggable, object? value, [CallerMemberName] string? methodName = null) => loggable.Log(LogLevel.Critical, value, methodName);
    public static void LogCritical(this ILoggable? loggable, object? value, [CallerMemberName] string? methodName = null) => loggable.Log(LogLevel.Critical, value, methodName);
    public static void LogCritical(this ILogger? logger, object value, [CallerMemberName] string? methodName = null)
    {
        if (logger == null || value == null)
            return;

        var msg = "(" + methodName + "): " + value;
        logger.Log(LogLevel.Critical, msg);
    }

    public static void LogTrace<T>(this ILoggable<T>? loggable, object? value, [CallerMemberName] string? methodName = null) => loggable.Log(LogLevel.Trace, value, methodName);
    public static void LogTrace(this ILoggable? loggable, object? value, [CallerMemberName] string? methodName = null) => loggable.Log(LogLevel.Trace, value, methodName);
    public static void LogTrace(this ILogger? logger, object value, [CallerMemberName] string? methodName = null)
    {
        if (logger == null || value == null)
            return;

        var msg = "(" + methodName + "): " + value;
        logger.Log(LogLevel.Trace, msg);
    }

    public static void Log(this ILoggable? loggable, LogLevel level, object? value, [CallerMemberName] string? methodName = null)
    {
        var logger = loggable?.Logger;
        if (logger == null)
            return;

        var msg = methodName != null ? "(" + methodName + "): " + value : value?.ToString();
        logger.Log(level, msg);
    }

    public static void Log<T>(this ILoggable<T>? loggable, LogLevel level, object? value, [CallerMemberName] string? methodName = null)
    {
        var logger = loggable?.Logger;
        if (logger == null)
            return;

        var msg = methodName != null ? "(" + methodName + "): " + value : value?.ToString();
        logger.Log(level, msg);
    }
}
#pragma warning restore CA2254
#pragma warning restore IDE0079 // Remove unnecessary suppression
