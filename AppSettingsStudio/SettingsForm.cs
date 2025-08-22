namespace AppSettingsStudio;

public partial class SettingsForm : Form
{
    public SettingsForm(object settings)
    {
        if (settings is not ICloneable cloneable)
            throw new ArgumentException(null, nameof(settings));

        if (settings is not INotifyPropertyChanged changed)
            throw new ArgumentException(null, nameof(settings));

        Settings = cloneable.Clone();
        changed.PropertyChanged += (s, e) =>
        {
            propertyGridSettings.Refresh();
        };

        InitializeComponent();

        MinimumSize = Size;
        propertyGridSettings.SelectedObject = Settings;
    }

    public object Settings { get; }
}
