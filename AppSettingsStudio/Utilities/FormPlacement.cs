namespace AppSettingsStudio.Utilities;

public class FormPlacement
{
    public WindowPlacement? WindowPlacement { get; set; }

    public static FormPlacement SavePlacement(Form form)
    {
        ArgumentNullException.ThrowIfNull(form);
        var placement = new FormPlacement { WindowPlacement = Utilities.WindowPlacement.GetPlacement(form.Handle) };
        return placement;
    }

    public bool RestorePlacement(Form form)
    {
        ArgumentNullException.ThrowIfNull(form);
        var saved = WindowPlacement;
        if (saved == null)
            return false;

        // ensure some global contraints are enforced
        var pos = saved.Value.NormalPosition;
        if (pos.left <= WinformsUtilities.WHERE_NOONE_CAN_SEE_ME || pos.top <= WinformsUtilities.WHERE_NOONE_CAN_SEE_ME)
            return false;

        if (pos.Width < 100 || pos.Height < 100)
            return false;

        if (saved.Value.ShowCmd == SW.SW_SHOWMINIMIZED)
        {
            form.WindowState = FormWindowState.Minimized;
        }
        else if (saved.Value.ShowCmd == SW.SW_SHOWMAXIMIZED)
        {
            form.WindowState = FormWindowState.Maximized;
        }
        else
        {
            form.WindowState = FormWindowState.Normal;
        }

        // ensure the form is within all monitor bounds
        var allScreensBound = Rectangle.Empty;
        foreach (var screen in Screen.AllScreens)
        {
            allScreensBound = Rectangle.Union(allScreensBound, screen.Bounds);
        }

        if (!allScreensBound.Contains(pos.Rectangle))
        {
            // else try to stick to closest window from left top of form
            var closestScreen = Screen.AllScreens.FirstOrDefault(s => s.Bounds.Contains(pos.LeftTop));
            if (closestScreen == null)
                return false;

            pos.bottom = Math.Min(pos.bottom, closestScreen.Bounds.Bottom);
            pos.right = Math.Min(pos.right, closestScreen.Bounds.Right);
        }

        form.StartPosition = FormStartPosition.Manual;
        form.Location = pos.LeftTop;
        form.Size = pos.Size;
        return true;
    }
}
