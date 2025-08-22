namespace AppSettingsStudio.Utilities;

public partial struct WindowPlacement
{
    public int Length { get; set; }
    public int Flags { get; set; }
    public SW ShowCmd { get; set; }
    public POINT MinPosition { get; set; }
    public POINT MaxPosition { get; set; }
    public RECT NormalPosition { get; set; }

    public bool SetPlacement(nint handle) => SetWindowPlacement(handle, ref this);
    public static WindowPlacement? GetPlacement(nint handle, bool throwOnError = false)
    {
        if (handle == 0)
            return null;

        var wp = new WindowPlacement { Length = Marshal.SizeOf<WindowPlacement>() };
        if (!GetWindowPlacement(handle, ref wp))
        {
            if (throwOnError)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            return null;
        }
        return wp;
    }

    [LibraryImport("user32", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetWindowPlacement(nint hWnd, ref WindowPlacement lpwndpl);

    [LibraryImport("user32", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetWindowPlacement(nint hWnd, ref WindowPlacement lpwndpl);
}
