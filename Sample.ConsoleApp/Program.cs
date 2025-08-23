using AppSettingsStudio.Configuration; // for the AddAppSettingsStudio extension method
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Sample.ConsoleApp;

internal class Program
{
    static async Task Main(string[] args)
    {
        using var host = Host.CreateDefaultBuilder(args)
        .ConfigureServices((context, services) =>
        {
            services
                // add the PositionOptions and bind it to the "Position" section of the configuration
                .Configure<PositionOptions>(context.Configuration.GetSection(PositionOptions.Position))
                .AddLogging(loggingBuilder =>
                {
                    // set MaxQueueLength = 1 otherwise we may loose messages
                    loggingBuilder.AddConsole(config => config.MaxQueueLength = 1).AddSimpleConsole(config => config.SingleLine = true);
                });
        })
        .ConfigureAppConfiguration((context, config) =>
        {
            config
            // add AppSettingsStudio configuration somewhere around here
            .AddAppSettingsStudio(config =>
            {
                config.Options |= SettingsOptions.GatherAppSettingsFile | SettingsOptions.MonitorChanges;
            });
        })
        .Build();

        // get a loggger from DI
        var logger = host.Services.GetRequiredService<ILogger<Program>>();

        // get the a dynamic (can be changed during exection) PositionOptions monitor from DI
        var options = host.Services.GetRequiredService<IOptionsMonitor<PositionOptions>>();
        while (true)
        {
            logger.LogInformation("Hello {Name}", options.CurrentValue.Name);
            await Task.Delay(500);
        }
    }
}
