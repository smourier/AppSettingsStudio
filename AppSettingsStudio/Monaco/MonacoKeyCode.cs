namespace AppSettingsStudio.Monaco;

// from https://github.com/microsoft/vscode/blob/main/src/vs/monaco.d.ts
public enum MonacoKeyCode
{
    DependsOnKbLayout = -1,
    /**
     * Placed first to cover the 0 value of the enum.
     */
    Unknown = 0,
    Backspace = 1,
    Tab = 2,
    Enter = 3,
    Shift = 4,
    Ctrl = 5,
    Alt = 6,
    PauseBreak = 7,
    CapsLock = 8,
    Escape = 9,
    Space = 10,
    PageUp = 11,
    PageDown = 12,
    End = 13,
    Home = 14,
    LeftArrow = 15,
    UpArrow = 16,
    RightArrow = 17,
    DownArrow = 18,
    Insert = 19,
    Delete = 20,
    Digit0 = 21,
    Digit1 = 22,
    Digit2 = 23,
    Digit3 = 24,
    Digit4 = 25,
    Digit5 = 26,
    Digit6 = 27,
    Digit7 = 28,
    Digit8 = 29,
    Digit9 = 30,
    KeyA = 31,
    KeyB = 32,
    KeyC = 33,
    KeyD = 34,
    KeyE = 35,
    KeyF = 36,
    KeyG = 37,
    KeyH = 38,
    KeyI = 39,
    KeyJ = 40,
    KeyK = 41,
    KeyL = 42,
    KeyM = 43,
    KeyN = 44,
    KeyO = 45,
    KeyP = 46,
    KeyQ = 47,
    KeyR = 48,
    KeyS = 49,
    KeyT = 50,
    KeyU = 51,
    KeyV = 52,
    KeyW = 53,
    KeyX = 54,
    KeyY = 55,
    KeyZ = 56,
    Meta = 57,
    ContextMenu = 58,
    F1 = 59,
    F2 = 60,
    F3 = 61,
    F4 = 62,
    F5 = 63,
    F6 = 64,
    F7 = 65,
    F8 = 66,
    F9 = 67,
    F10 = 68,
    F11 = 69,
    F12 = 70,
    F13 = 71,
    F14 = 72,
    F15 = 73,
    F16 = 74,
    F17 = 75,
    F18 = 76,
    F19 = 77,
    NumLock = 78,
    ScrollLock = 79,
    /**
     * Used for miscellaneous characters; it can vary by keyboard.
     * For the US standard keyboard, the ';:' key
     */
    Semicolon = 80,
    /**
     * For any country/region, the '+' key
     * For the US standard keyboard, the '=+' key
     */
    Equal = 81,
    /**
     * For any country/region, the ',' key
     * For the US standard keyboard, the ',<' key
     */
    Comma = 82,
    /**
     * For any country/region, the '-' key
     * For the US standard keyboard, the '-_' key
     */
    Minus = 83,
    /**
     * For any country/region, the '.' key
     * For the US standard keyboard, the '.>' key
     */
    Period = 84,
    /**
     * Used for miscellaneous characters; it can vary by keyboard.
     * For the US standard keyboard, the '/?' key
     */
    Slash = 85,
    /**
     * Used for miscellaneous characters; it can vary by keyboard.
     * For the US standard keyboard, the '`~' key
     */
    Backquote = 86,
    /**
     * Used for miscellaneous characters; it can vary by keyboard.
     * For the US standard keyboard, the '[{' key
     */
    BracketLeft = 87,
    /**
     * Used for miscellaneous characters; it can vary by keyboard.
     * For the US standard keyboard, the '\|' key
     */
    Backslash = 88,
    /**
     * Used for miscellaneous characters; it can vary by keyboard.
     * For the US standard keyboard, the ']}' key
     */
    BracketRight = 89,
    /**
     * Used for miscellaneous characters; it can vary by keyboard.
     * For the US standard keyboard, the ''"' key
     */
    Quote = 90,
    /**
     * Used for miscellaneous characters; it can vary by keyboard.
     */
    OEM_8 = 91,
    /**
     * Either the angle bracket key or the backslash key on the RT 102-key keyboard.
     */
    IntlBackslash = 92,
    Numpad0 = 93,
    Numpad1 = 94,
    Numpad2 = 95,
    Numpad3 = 96,
    Numpad4 = 97,
    Numpad5 = 98,
    Numpad6 = 99,
    Numpad7 = 100,
    Numpad8 = 101,
    Numpad9 = 102,
    NumpadMultiply = 103,
    NumpadAdd = 104,
    NUMPAD_SEPARATOR = 105,
    NumpadSubtract = 106,
    NumpadDecimal = 107,
    NumpadDivide = 108,
    /**
     * Cover all key codes when IME is processing input.
     */
    KEY_IN_COMPOSITION = 109,
    ABNT_C1 = 110,
    ABNT_C2 = 111,
    AudioVolumeMute = 112,
    AudioVolumeUp = 113,
    AudioVolumeDown = 114,
    BrowserSearch = 115,
    BrowserHome = 116,
    BrowserBack = 117,
    BrowserForward = 118,
    MediaTrackNext = 119,
    MediaTrackPrevious = 120,
    MediaStop = 121,
    MediaPlayPause = 122,
    LaunchMediaPlayer = 123,
    LaunchMail = 124,
    LaunchApp2 = 125,
    /**
     * VK_CLEAR, 0x0C, CLEAR key
     */
    Clear = 126,
    /**
     * Placed last to cover the length of the enum.
     * Please do not depend on this value!
     */
    MAX_VALUE = 127
}
