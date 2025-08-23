# AppSettings Studio
A graphical interface for centralized management of .NET `appsettings.json` files. It offers a unified view to edit, validate, and apply configuration changes across multiple projects or executables—including WSL on Windows—with support for live updates.


<img width="1080" height="518" alt="AppSettings Studio" src="https://github.com/user-attachments/assets/65e7db1c-77b6-4fd3-abb6-0b34dd6e2ea6" />


AppSettings Studio builds on `Microsoft.Extensions.Logging.Configuration` and specifically leverages the JSON provider.

Although primarily designed for .NET developers during development and testing, there’s nothing preventing its use in production. It supports all types of .NET applications—Web, Console, etc.—running on both Windows and Linux.

Possible use cases:

* Centralized management of `appsettings.json` files
* Enable per-developper app settings configuration
* Avoid storing per-developer `appsettings.json` files in source control repository
* Avoid storing secrets in source control repository (`appsettings.json` files can just contain comments and placeholders)
* Simplify settings management for the same application deployed or used differently (debug vs release, v1 vs v2, WSL vs Windows, git submodules, multiple repo clones, etc.)
* Easy support for dynamic settings changes
* Easy settings change using with a syntax coloring enabled Json editor
* etc.

The solution consists of two main components:

1. **AppSettingsStudio.Configuration** assembly – a standard [.NET Configuration Provider](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration#configuration-providers). This must be included in any application that should integrate with the AppSettings Studio application.

2. **AppSettings Studio** application (WinForms) – a desktop tool that enables developers to configure all registered applications. Note it requires Microsoft's WebView2 to be installed.

The solution also contains a **Sample.ConsoleApp** console application that demonstrate how it works.
 
## Enabling an application
To be able to use AppSettings Studio, an application must be "enabled". For that, all is needed is to inject AppSettings Studio's `ConfigurationProvider` service into the application using standard .NET dependency injection, something like that:

    // add a reference to AppSettingsStudio.Configuration.dll
    ...
    using var host = Host.CreateDefaultBuilder(args)
    ...
    .ConfigureAppConfiguration((context, config) =>
    {
        config
        // add AppSettingsStudio configuration provider (singleton) somewhere around here
        .AddAppSettingsStudio(config =>
        {
            // define AppSettingsStudio options
            config.Options |= SettingsOptions.GatherAppSettingsFile | SettingsOptions.MonitorChanges;
        });
    })
    .Build();
    ...

`GatherAppSettingsFile` tells the system to *gather* an `appsettings.json` file in AppSettings Studio. This gathering step needs to be performed at least once but can be skipped afterward. By default, if no option is provided, it only runs in DEBUG builds—preventing accidental execution in production.

`MonitorChanges` tells the system to reload the settings each time it's changed in AppSettings Studio.

## Configuring an application with "virtual settings"
The first time the application is ran, and if it was configured to gather it's `appsettings.json` file, the application should appear in AppSettings Studio. For example, since AppSettings Studio is itself enabled, the first time you run it, this is what you should see:

<img width="1109" height="841" alt="AppSettings Studio" src="https://github.com/user-attachments/assets/654de972-19da-477c-b1fc-6b99914f96d9" />

AppSettings Studio automatically uses the application’s icon if available. For better readability—especially when managing multiple applications—it’s recommended to provide one. All .NET executables, including web applications, can have an icon assigned.

When you select an application's `appsettings.json` file in the left tree view, you can see on the right pane a json editor (powered by the [Monaco Editor](https://microsoft.github.io/monaco-editor/)). For all originally gathered `appsettings.json` files from all enabled application, the editor is in read-only mode:

<img width="684" height="255" alt="Read Only Editor" src="https://github.com/user-attachments/assets/36f6b576-441d-48ce-a5b5-39774bab85b3" />

To be able to change an app's settings, you just need to create a "Virtual Settings", so right click on an `appsettings.json` node in the tree view, select "Add Virtual Settings...":

<img width="377" height="241" alt="Virtual Settings" src="https://github.com/user-attachments/assets/3226a81c-8756-4984-bbb2-747b645172b1" />

Choose a name (it must start with *appsettings* and be a *.json* file):

<img width="460" height="108" alt="Link Name" src="https://github.com/user-attachments/assets/b8143bbf-a8b3-43f4-b6a2-3f4ace65f293" />

Now, this setting's json, with a content initialized from the gathered `appsettings.json`'s content, is editable, and you can save the change (CTRL+S as a shortcut):

<img width="669" height="327" alt="Editable Settings" src="https://github.com/user-attachments/assets/61889a41-4619-4b50-a020-67c7c788a2d0" />

Now, when you restart the .NET application, it will use the new settings. If the application supports it, changes can be reflected dynamically (see next chapter).

You can configure more than one settings per application, but only one is considered as *active*: the one that has the icon with a small green "O" overlay:

<img width="257" height="232" alt="Active Settings" src="https://github.com/user-attachments/assets/33338fe2-f094-4785-98f0-6c4d5c2f1266" />

You can change the active settings by right clicking on a settings node and selecting "Make Active"

<img width="303" height="245" alt="Make Active" src="https://github.com/user-attachments/assets/be0c2361-2649-4f99-b7f3-a4e1688cc87e" />

You can also import an existing settings, from a fellow developer for example, by right clicking on an application's instance node and selecting "Import Virtual Settings..."

<img width="327" height="256" alt="Import Settings" src="https://github.com/user-attachments/assets/c1392830-512f-4b1d-aa40-aa4109110896" />


## Dynamic settings change
You can enable an application's settings to change at the same time you edit it AppSettings Studio. There are two requirements:

1) the application must follow .NET Configurations's [IOptionsMonitor pattern](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options#ioptionsmonitor)
2) the application must be configured with the `SettingsOptions.MonitorChanges` option (see above)

The Sample.ConsoleApp project demonstrates this.

What it does is continuously display the value of one of it's custom settings property. Just run it and change the settings in AppSettings Studio at the same time. Each time you save, the application reflects it. Here we have change the Position's Name to "Joe Smith Senior" and undo:

<img width="1128" height="630" alt="Settings Live Change Windows" src="https://github.com/user-attachments/assets/05a32e91-d064-45c3-8587-3e058548fa7c" />

This is the same exact .NET app running in WSL Ubuntu (see following chapter on how to configure this):

<img width="1124" height="619" alt="Settings Live Change WSL" src="https://github.com/user-attachments/assets/1fb784cc-e435-4ac9-b1cf-4b3e995bad72" />

## Enabling an application running on WSL
AppSettings Studio has the concept of "root paths". They are configured directories which contain gathered `appsettings.json` files pointers, virtual ones as well as links.

By default, only one root path exists: `%USERPROFILE%\.AppSettingsStudio`, so for example `C:\Users\<your login>\.AppSettingsStudio`, but you can add others.

This can be used to share settings among multiple developers, but this is also how you can configure .NET apps running in WSL.

Once you have started an enabled .NET application in WSL at least once, open AppSettings Studio and select the "File" / "Root Paths" menu

<img width="285" height="208" alt="Root Paths" src="https://github.com/user-attachments/assets/1b6d9135-9aee-4cb9-a4af-0a737084cad9" />

Select the .AppSettingsStudio directory that has been created in the WSL volume:

<img width="1260" height="721" alt="WSL Root Path" src="https://github.com/user-attachments/assets/c7a63015-c7ce-4d8e-8ca3-dfa97eadc9a4" />

Two root paths are now configured:

<img width="1086" height="521" alt="Two Root Paths" src="https://github.com/user-attachments/assets/10d0c031-edb3-4983-b81e-28274b864f57" />

AppSettings Studio shows you the .NET app running in WSL:

<img width="872" height="452" alt="WSL dotnet" src="https://github.com/user-attachments/assets/89f18e46-dc35-464c-b61f-f349f166c81e" />

Note: all WSL apps appear under a "dotnet" tree item because they are all lauched by same "dotnet" executable.

## Same application, multiple instances
The same .NET application can be deployed multiple times for different reasons : Debug vs Relese, Testing environment vs Developer environment, versioning, etc. An application is by default identified by the name of it's executable, not by the directory it exists in.

AppSettings Studio allows centralization of all these settings in one place, for example if the Sample.ConsoleApp is only run in Debug, this is what we'll see:

<img width="287" height="338" alt="Debug Only" src="https://github.com/user-attachments/assets/ebb4a6d6-08a0-4c12-a0bd-9bf3edfce838" />

But once it's been also run in Release, there are now two child nodes under the application node, prefixed by Debug and Release:

<img width="269" height="329" alt="image" src="https://github.com/user-attachments/assets/3a51b42e-879f-4583-bdef-bdcb1af4c23d" />

Note: AppSettings Studio automatically generates a unique, concise name for each application version to help distinguish them.

## "Virtual Settings" links
To avoid copying settings for applications that can share them (like in the Debug vs Release case), you can create a "virtual settings" *link* to a "virtual settings".

<img width="304" height="253" alt="Link" src="https://github.com/user-attachments/assets/10470d66-e04b-4d22-a2a4-d09d7963daac" />

You then can choose a source settings to link to:

<img width="1040" height="554" alt="Browse Links" src="https://github.com/user-attachments/assets/162b0479-5102-4c66-a8ee-50e94eacf1e6" />

Now, each time you change a source setting using the JSON editor, it will be reflected in all linked virtual settings.
<img width="1080" height="518" alt="Link" src="https://github.com/user-attachments/assets/c40e1bc8-8f4d-49d9-89e2-56c8d463157e" />

Note: creating links between Windows and WSL is not supported.
