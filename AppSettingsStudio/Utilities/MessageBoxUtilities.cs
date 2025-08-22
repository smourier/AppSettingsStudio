namespace AppSettingsStudio.Utilities;

public static class MessageBoxUtilities
{
    public static string ApplicationName => AssemblyUtilities.GetProduct() ?? Path.GetFileNameWithoutExtension(Environment.ProcessPath) ?? string.Empty;
    public static string? ApplicationVersion => Assembly.GetExecutingAssembly().GetFileVersion();
    public static string ApplicationTitle => ApplicationName + " V" + ApplicationVersion;

    public static void ShowMessage(this IWin32Window? owner, string text) => MessageBox.Show(owner, text, ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
    public static DialogResult ShowConfirm(this IWin32Window? owner, string text) => MessageBox.Show(owner, text, ApplicationTitle + " - " + Res.Confirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
    public static DialogResult ShowQuestion(this IWin32Window? owner, string text) => MessageBox.Show(owner, text, ApplicationTitle + " - " + Res.Confirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
    public static void ShowError(this IWin32Window? owner, string text) => MessageBox.Show(owner, text, ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
    public static void ShowWarning(this IWin32Window? owner, string text) => MessageBox.Show(owner, text, ApplicationTitle + " - " + Res.Warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
}
