namespace AppSettingsStudio;

internal class Program
{
    internal static Task _monacoInstalledTask = null!;

    [AllowNull]
    public static IHost Host { get; private set; }

    [STAThread]
    static void Main(string[] args)
    {
        try
        {
            var version = CoreWebView2Environment.GetAvailableBrowserVersionString();
            if (string.IsNullOrWhiteSpace(version))
                throw new Exception("No WebView2 runtime found");
        }
        catch (Exception ex)
        {
            MessageBoxUtilities.ShowError(null, $"Failed to check for WebView2 runtime. Please ensure you have the latest version installed. Inner error: {ex.GetInterestingExceptionMessage()}");
            return;
        }

        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
        _monacoInstalledTask = MonacoResources.EnsureMonacoFilesAsync();
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

        Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
        .ConfigureServices((context, services) =>
        {
            services
            .AddLogging();
        })
        .ConfigureAppConfiguration((context, config) =>
        {
            config
            .AddSettingsManager()
            .AddEnvironmentVariables()
            .AddCommandLine(args);
        })
        .Build();

        Application.Run(new Main());
    }

    private static void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        var logger = Host?.Services.GetService<ILogger<Program>>();
        logger.LogError(e.Exception);
    }
}