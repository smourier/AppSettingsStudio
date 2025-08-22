namespace AppSettingsStudio.Configuration;

public class SettingsVariable(string name, string value)
{
    public virtual string Name { get; } = name;
    public virtual string Value { get; set; } = value;

    public override string ToString() => $"{Name}: {Value}";
}
