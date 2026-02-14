namespace DesktopBuddy.Services;

using System.Runtime.InteropServices;
using System.Drawing; // Required for the Point class

public class MouseService
{
    // Imports the SetCursorPos function from user32.dll
    [DllImport("user32.dll")]
    public static extern bool SetCursorPos(int X, int Y);

    // Imports the mouse_event function from user32.dll
    [System.Runtime.InteropServices.DllImport("user32.dll")]
    public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

    // Define constants for mouse events
    private const int MOUSEEVENTF_LEFTDOWN = 0x02;
    private const int MOUSEEVENTF_LEFTUP = 0x04;
    private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
    private const int MOUSEEVENTF_RIGHTUP = 0x10;

    /// <summary>
    /// Moves the cursor to a specific position and performs a left click.
    /// </summary>
    public static void MouseDownAtPosition(int x, int y)
    {
        SetCursorPos(x, y); // Set the cursor position
        mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0); // Simulate left button down
    }

    public static void MouseUpAtPosition(int x, int y)
    {
        SetCursorPos(x, y); // Set the cursor position
        mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0); // Simulate left button up
    }
}