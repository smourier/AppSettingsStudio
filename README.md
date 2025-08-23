# AppSettings Studio
A graphical interface for centralized management of .NET appsettings.json files. It offers a unified view to edit, validate, and apply configuration changes across multiple projects or executables—including WSL on Windows—with support for live updates.


<img width="1080" height="518" alt="AppSettings Studio" src="https://github.com/user-attachments/assets/65e7db1c-77b6-4fd3-abb6-0b34dd6e2ea6" />


AppSettings Studio builds on `Microsoft.Extensions.Logging.Configuration` and specifically leverages the JSON provider.

Although primarily designed for .NET developers during development and testing, there’s nothing preventing its use in production. It supports all types of .NET applications—Web, Console, etc.—running on both Windows and Linux.

The solution consists of two main components:

1. **AppSettingsStudio.Configuration** assembly – a standard [.NET Configuration Provider](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration#configuration-providers). This must be included in any application that should integrate with the AppSettings Studio application.

2. **AppSettings Studio** application (WinForms) – a desktop tool that enables developers to configure all registered applications.

The solution also contains a **Sample.ConsoleApp** console application that demonstrate how it works.
 
## Enabling an application
To be able to use AppSettings Studio, an application must be "enabled". For that, all is needed is to inject the AppSettingsStudio ConfigurationProvider service into the application using standard .NET dependency injection, something like that:

    using var host = Host.CreateDefaultBuilder(args)
    ...
    .ConfigureAppConfiguration((context, config) =>
    {
        config
        // add AppSettingsStudio configuration somewhere around here
        .AddAppSettingsStudio(config =>
        {
            // define AppSettingsStudio options
            config.Options |= SettingsOptions.GatherAppSettingsFile | SettingsOptions.MonitorChanges;
        });
    })
    .Build();
    ...

`GatherAppSettingsFile` tells the system to gather an appsettings.json file in AppSettings Studio. This gathering step needs to be performed at least once but can be skipped afterward. By default, if no option is provided, it only runs in DEBUG builds—preventing accidental execution in production.

The first time the application is ran, and if everyhing went well, the application should appear in AppSettings Studio. For example, since AppSettings Studio is itself enabled, the first time you run it, this is what you should see:

<img width="1109" height="841" alt="AppSettings Studio" src="https://github.com/user-attachments/assets/654de972-19da-477c-b1fc-6b99914f96d9" />

AppSettings Studio automatically uses the application’s icon if available. For better readability—especially when managing multiple applications—it’s recommended to provide one. All .NET executables, including web applications, can have an icon assigned.
