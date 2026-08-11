namespace AppSettingsStudio;

// a ToolStrip that also fires on the click that activated its window instead of eating it.
// without this, the first click on a toolbar button of an inactive (modeless) window only activates the window, so two clicks are needed.
// WM_MOUSEACTIVATE is handled by the ToolStrip's own window, which is why the fix lives here, not on the form: turn MA_ACTIVATEANDEAT into MA_ACTIVATE so the same click both activates and reaches the button.
internal sealed class ClickThroughToolStrip : ToolStrip
{
    protected override void WndProc(ref Message m)
    {
        const int WM_MOUSEACTIVATE = 0x0021;
        const int MA_ACTIVATE = 1;
        const int MA_ACTIVATEANDEAT = 2;
        if (m.Msg == WM_MOUSEACTIVATE)
        {
            base.WndProc(ref m);
            if (m.Result == MA_ACTIVATEANDEAT)
            {
                m.Result = MA_ACTIVATE;
            }
            return;
        }

        base.WndProc(ref m);
    }
}
