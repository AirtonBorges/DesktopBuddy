using System.Numerics;
using System.Windows.Threading;

namespace DesktopBuddy.Services;

public class WindowService
{
    private readonly MainWindow _window;

    public WindowService(MainWindow window)
    {
        _window = window;
    }

    public double Width => _window.Dispatcher.Invoke(() => _window.Width);
    public double Height => _window.Dispatcher.Invoke(() => _window.Height);

    public void MoveTo(float x, float y)
    {
        _window.Dispatcher.Invoke(() =>
        {
            _window.Left = x;
            _window.Top = y;
        });
    }
    
    public void MoveTo(double x, double y)
    {
        _window.Dispatcher.Invoke(() =>
        {
            _window.Left = x;
            _window.Top = y;
        });
    }

    public Vector2 GetPosition()
    {
        var returns = new Vector2();

        _window.Dispatcher.Invoke(() =>
        {
            returns = new Vector2
            {
                X = (float)_window.Left,
                Y = (float)_window.Top
            };
        });

        return returns;
    }
    
}