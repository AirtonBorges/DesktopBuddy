using System;
using System.Diagnostics;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows.Forms;
using DesktopBuddy.Interfaces;

namespace DesktopBuddy.Services;

public class GoingAfterMouseService: IStateService<EState>
{
    private Vector2 _target;
    private double _milissecondsToNextFrame = 1;
    private double _pixelsPerFrame = 2;
    private int _timeToCatchMouse = 5000;

    private readonly Random _random = new();
    private readonly WindowService _windowService;

    public GoingAfterMouseService(WindowService windowService)
    {
        _windowService = windowService;
    }

    public async Task Tick()
    {
        await GoAfterMouse();
    }

    private async Task GoAfterMouse()
    {
        var xDistance = GetDistanceToTarget();
        var xStopWatch = Stopwatch.StartNew();

        while (xDistance > 5)
        {
            if (xStopWatch.ElapsedMilliseconds > _timeToCatchMouse)
            {
                xStopWatch.Stop();
                break;
            }

            GetMousePosition();
            var xPosition = _windowService.GetPosition();
            xDistance = GoTowardsTarget(xPosition, xDistance);
            await Task.Delay((int)_milissecondsToNextFrame);
        }

        if (xDistance >= 5)
        {
            return;
        }

        await WalkWithMouse();
    }

    private async Task WalkWithMouse()
    {
        PickRandomTarget();
        var xPosition = _windowService.GetPosition();
        var xDistance = GetDistanceToTarget();

        MouseService.MouseDownAtPosition((int)xPosition.X, (int)xPosition.Y);
        while (true)
        {
            xDistance = GoTowardsTarget(xPosition, xDistance);
            xPosition = _windowService.GetPosition();

            var x = (int)xPosition.X + (int)_windowService.Width + 10;
            var y = (int)xPosition.Y + (int)_windowService.Height / 2;

            MouseService.SetCursorPos(x, y);

            await Task.Delay((int)_milissecondsToNextFrame);
            if (!(xDistance <= 5))
                continue;

            MouseService.MouseUpAtPosition(x, y);
            break;
        }
    }

    private float GoTowardsTarget(Vector2 xPosition, float xDistance)
    {
        var xDirectionX = (_target.X - xPosition.X) / xDistance;
        var xDirectionY = (_target.Y - xPosition.Y) / xDistance;
        var xStepX = (int)(xPosition.X + xDirectionX * _pixelsPerFrame);
        var xStepY = (int)(xPosition.Y + xDirectionY * _pixelsPerFrame);

        _windowService.MoveTo(xStepX, xStepY);
        var xReturns = GetDistanceToTarget();

        return xReturns;
    }

    private void GetMousePosition()
    {
        var xMousePosition = Control.MousePosition;
        _target.X = xMousePosition.X;
        _target.Y = xMousePosition.Y;
    }

    public Task<EState> GetState()
    {
        return Task.FromResult(EState.Idle);
    }

    private void PickRandomTarget()
    {
        var xScreen = Screen.PrimaryScreen!;
        var xWorkingArea = xScreen.WorkingArea;
        
        var xTargetX = _random.Next(xWorkingArea.Left, xWorkingArea.Right - (int)_windowService.Width);
        var xTargetY = _random.Next(xWorkingArea.Top, xWorkingArea.Bottom - (int)_windowService.Height);

        _target.X = xTargetX;
        _target.Y = xTargetY;
    }

    private float GetDistanceToTarget()
    {
        var xPosition = _windowService.GetPosition();
        var xPowX = Math.Pow(_target.X - xPosition.X, 2);
        var xPowY = Math.Pow(_target.Y - xPosition.Y, 2);

        var xReturn = (float)Math.Sqrt(xPowX + xPowY);
        return xReturn;
    }
}