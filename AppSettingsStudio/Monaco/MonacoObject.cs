namespace AppSettingsStudio.Monaco;

#pragma warning disable IDE1006 // Naming Styles

[ComVisible(true)]
public partial class MonacoObject
{
    public event EventHandler<MonacoLoadEventArgs>? Load;
    public event EventHandler<MonacoEventArgs>? Event;

    public object getOptions() => JsonSerializer.Serialize(new
    {
        automaticLayout = true,
        //language = "plaintext",
        fontSize = Settings.Current.JsonFontSize.ToString(CultureInfo.InvariantCulture) + "px",
        dragAndDrop = false,
        mouseWheelZoom = true,
        contextmenu = false,
        theme = Settings.Current.JsonTheme,
    });

    public string? load()
    {
        try
        {
            var e = new MonacoLoadEventArgs();
            Load?.Invoke(this, e);
            return e.DocumentText;
        }
        catch (Exception ex)
        {
            Trace.WriteLine("Error: " + ex);
            throw;
        }
    }

    public void onEvent(MonacoEventType type, string? json = null)
    {
        var handler = Event;
        if (handler == null)
            return;

        try
        {
            var e = type switch
            {
                MonacoEventType.KeyDown or MonacoEventType.KeyUp => new MonacoKeyEventArgs(type, json),
                MonacoEventType.ConfigurationChanged => new MonacoConfigurationChangedEventArgs(json),
                _ => new MonacoEventArgs(type, json),
            };
            handler?.Invoke(this, e);
        }
        catch (Exception ex)
        {
            Trace.WriteLine("Error: " + ex);
            throw;
        }
    }
}
#pragma warning restore IDE1006 // Naming Styles
