namespace AppSettingsStudio.Monaco;

public class MonacoKeyEventArgs(MonacoEventType type, string? json) : MonacoEventArgs(type, json)
{
    public int KeyCode => RootElement.GetValue("keyCode", 0);
    public string? Code => RootElement.GetNullifiedValue("code");
    public bool Alt => RootElement.GetValue("altKey", false);
    public bool AltGraph => RootElement.GetValue("altGraphKey", false);
    public bool Ctrl => RootElement.GetValue("ctrlKey", false);
    public bool Meta => RootElement.GetValue("metaKey", false);
    public bool Shift => RootElement.GetValue("shiftKey", false);

    public Keys Keys
    {
        get
        {
            var c = From((MonacoKeyCode)KeyCode);
            if (Alt || AltGraph)
            {
                c |= Keys.Alt;
            }

            if (Ctrl)
            {
                c |= Keys.Control;
            }

            if (Shift)
            {
                c |= Keys.Shift;
            }
            return c;
        }
    }

    public static Keys From(MonacoKeyCode keyCode)
    {
        switch (keyCode)
        {
            case MonacoKeyCode.Backspace:
                return Keys.Back;
            case MonacoKeyCode.Tab:
                return Keys.Tab;
            case MonacoKeyCode.Enter:
                return Keys.Enter;
            case MonacoKeyCode.Shift:
                return Keys.Shift;
            case MonacoKeyCode.Ctrl:
                return Keys.Control;
            case MonacoKeyCode.Alt:
                return Keys.Alt;
            case MonacoKeyCode.PauseBreak:
                return Keys.Pause;
            case MonacoKeyCode.CapsLock:
                return Keys.CapsLock;
            case MonacoKeyCode.Escape:
                return Keys.Escape;
            case MonacoKeyCode.Space:
                return Keys.Space;
            case MonacoKeyCode.PageUp:
                return Keys.PageUp;
            case MonacoKeyCode.PageDown:
                return Keys.PageDown;
            case MonacoKeyCode.End:
                return Keys.End;
            case MonacoKeyCode.Home:
                return Keys.Home;
            case MonacoKeyCode.LeftArrow:
                return Keys.Left;
            case MonacoKeyCode.UpArrow:
                return Keys.Up;
            case MonacoKeyCode.RightArrow:
                return Keys.Right;
            case MonacoKeyCode.DownArrow:
                return Keys.Down;
            case MonacoKeyCode.Insert:
                return Keys.Insert;
            case MonacoKeyCode.Delete:
                return Keys.Delete;
            case MonacoKeyCode.Digit0:
                return Keys.D0;
            case MonacoKeyCode.Digit1:
                return Keys.D1;
            case MonacoKeyCode.Digit2:
                return Keys.D2;
            case MonacoKeyCode.Digit3:
                return Keys.D3;
            case MonacoKeyCode.Digit4:
                return Keys.D4;
            case MonacoKeyCode.Digit5:
                return Keys.D5;
            case MonacoKeyCode.Digit6:
                return Keys.D6;
            case MonacoKeyCode.Digit7:
                return Keys.D7;
            case MonacoKeyCode.Digit8:
                return Keys.D8;
            case MonacoKeyCode.Digit9:
                return Keys.D9;
            case MonacoKeyCode.KeyA:
                return Keys.A;
            case MonacoKeyCode.KeyB:
                return Keys.B;
            case MonacoKeyCode.KeyC:
                return Keys.C;
            case MonacoKeyCode.KeyD:
                return Keys.D;
            case MonacoKeyCode.KeyE:
                return Keys.E;
            case MonacoKeyCode.KeyF:
                return Keys.F;
            case MonacoKeyCode.KeyG:
                return Keys.G;
            case MonacoKeyCode.KeyH:
                return Keys.H;
            case MonacoKeyCode.KeyI:
                return Keys.I;
            case MonacoKeyCode.KeyJ:
                return Keys.J;
            case MonacoKeyCode.KeyK:
                return Keys.K;
            case MonacoKeyCode.KeyL:
                return Keys.L;
            case MonacoKeyCode.KeyM:
                return Keys.M;
            case MonacoKeyCode.KeyN:
                return Keys.N;
            case MonacoKeyCode.KeyO:
                return Keys.O;
            case MonacoKeyCode.KeyP:
                return Keys.P;
            case MonacoKeyCode.KeyQ:
                return Keys.Q;
            case MonacoKeyCode.KeyR:
                return Keys.R;
            case MonacoKeyCode.KeyS:
                return Keys.S;
            case MonacoKeyCode.KeyT:
                return Keys.T;
            case MonacoKeyCode.KeyU:
                return Keys.U;
            case MonacoKeyCode.KeyV:
                return Keys.V;
            case MonacoKeyCode.KeyW:
                return Keys.W;
            case MonacoKeyCode.KeyX:
                return Keys.X;
            case MonacoKeyCode.KeyY:
                return Keys.Y;
            case MonacoKeyCode.KeyZ:
                return Keys.Z;
            case MonacoKeyCode.Meta:
                return Keys.LWin;
            case MonacoKeyCode.ContextMenu:
                return Keys.RWin;
            case MonacoKeyCode.F1:
                return Keys.F1;
            case MonacoKeyCode.F2:
                return Keys.F2;
            case MonacoKeyCode.F3:
                return Keys.F3;
            case MonacoKeyCode.F4:
                return Keys.F4;
            case MonacoKeyCode.F5:
                return Keys.F5;
            case MonacoKeyCode.F6:
                return Keys.F6;
            case MonacoKeyCode.F7:
                return Keys.F7;
            case MonacoKeyCode.F8:
                return Keys.F8;
            case MonacoKeyCode.F9:
                return Keys.F9;
            case MonacoKeyCode.F10:
                return Keys.F10;
            case MonacoKeyCode.F11:
                return Keys.F11;
            case MonacoKeyCode.F12:
                return Keys.F12;
            case MonacoKeyCode.F13:
                return Keys.F13;
            case MonacoKeyCode.F14:
                return Keys.F14;
            case MonacoKeyCode.F15:
                return Keys.F15;
            case MonacoKeyCode.F16:
                return Keys.F16;
            case MonacoKeyCode.F17:
                return Keys.F17;
            case MonacoKeyCode.F18:
                return Keys.F18;
            case MonacoKeyCode.F19:
                return Keys.F19;
            case MonacoKeyCode.NumLock:
                return Keys.NumLock;
            case MonacoKeyCode.ScrollLock:
                return Keys.Scroll;
            case MonacoKeyCode.Numpad0:
                return Keys.NumPad0;
            case MonacoKeyCode.Numpad1:
                return Keys.NumPad1;
            case MonacoKeyCode.Numpad2:
                return Keys.NumPad2;
            case MonacoKeyCode.Numpad3:
                return Keys.NumPad3;
            case MonacoKeyCode.Numpad4:
                return Keys.NumPad4;
            case MonacoKeyCode.Numpad5:
                return Keys.NumPad5;
            case MonacoKeyCode.Numpad6:
                return Keys.NumPad6;
            case MonacoKeyCode.Numpad7:
                return Keys.NumPad7;
            case MonacoKeyCode.Numpad8:
                return Keys.NumPad8;
            case MonacoKeyCode.Numpad9:
                return Keys.NumPad9;
            case MonacoKeyCode.NumpadSubtract:
                return Keys.Subtract;
            case MonacoKeyCode.NumpadAdd:
                return Keys.Add;
            case MonacoKeyCode.NumpadDecimal:
                return Keys.Decimal;
            case MonacoKeyCode.NumpadMultiply:
                return Keys.Multiply;
            case MonacoKeyCode.NumpadDivide:
                return Keys.Divide;
            case MonacoKeyCode.AudioVolumeMute:
                return Keys.VolumeMute;
            case MonacoKeyCode.AudioVolumeUp:
                return Keys.VolumeUp;
            case MonacoKeyCode.AudioVolumeDown:
                return Keys.VolumeDown;
            case MonacoKeyCode.BrowserSearch:
                return Keys.BrowserSearch;
            case MonacoKeyCode.BrowserHome:
                return Keys.BrowserHome;
            case MonacoKeyCode.BrowserBack:
                return Keys.BrowserBack;
            case MonacoKeyCode.BrowserForward:
                return Keys.BrowserForward;
            case MonacoKeyCode.MediaTrackNext:
                return Keys.MediaNextTrack;
            case MonacoKeyCode.MediaTrackPrevious:
                return Keys.MediaPreviousTrack;
            case MonacoKeyCode.MediaStop:
                return Keys.MediaStop;
            case MonacoKeyCode.MediaPlayPause:
                return Keys.MediaPlayPause;
            case MonacoKeyCode.Clear:
                return Keys.Clear;
            case MonacoKeyCode.OEM_8:
                return Keys.Oem8;
            case MonacoKeyCode.Comma:
                return Keys.Oemcomma;
            case MonacoKeyCode.Semicolon:
                return Keys.OemSemicolon;
            case MonacoKeyCode.Slash:
                return Keys.OemQuestion;
            case MonacoKeyCode.BracketLeft:
                return Keys.OemOpenBrackets;
            case MonacoKeyCode.Backslash:
                return Keys.OemBackslash;
            case MonacoKeyCode.BracketRight:
                return Keys.OemCloseBrackets;
            case MonacoKeyCode.Quote:
                return Keys.OemQuotes;
            case MonacoKeyCode.IntlBackslash:
                return Keys.OemBackslash;

            //???
            case MonacoKeyCode.Equal:
            case MonacoKeyCode.Minus:
            case MonacoKeyCode.Period:
            case MonacoKeyCode.Backquote:
            case MonacoKeyCode.NUMPAD_SEPARATOR:
            case MonacoKeyCode.KEY_IN_COMPOSITION:
            case MonacoKeyCode.ABNT_C1:
            case MonacoKeyCode.ABNT_C2:
                break;
        }
        //Trace.WriteLine($"Unmapped keycode '{keyCode}'");
        return 0;
    }
}
