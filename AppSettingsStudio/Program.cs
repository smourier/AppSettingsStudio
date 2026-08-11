namespace AppSettingsStudio;

internal class Program
{
    internal static Task _monacoInstalledTask = null!;
    private static string _webView2DownloadUrl = "https://go.microsoft.com/fwlink/p/?LinkId=2124703";

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
            MessageBoxUtilities.ShowError(null, string.Format(Res.WebView2CheckError, Environment.NewLine, _webView2DownloadUrl, ex.GetInterestingExceptionMessage()));
            return;
        }

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
            // we're self hosted as a demonstration, note however this is not how AppSettingsStudio is configured
            .AddAppSettingsStudio();
        })
        .Build();

        Application.Run(new Main());
    }
}