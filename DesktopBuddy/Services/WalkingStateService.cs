using System;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows.Forms;
using DesktopBuddy.Interfaces;

namespace DesktopBuddy.Services;

public class WalkingStateService: IStateService<EState>
{
    private Vector2 _target;
    private double _milissecondsToNextFrame = 1;
    private double _pixelsPerFrame = 2;

    private readonly Random _random = new();
    private readonly WindowService _windowService;

    public WalkingStateService(WindowService windowService)
    {
        _windowService = windowService;
    }

    public async Task Tick()
    {
        PickRandomTarget();
        var xPosition = _windowService.GetPosition();

        var xDistance = GetDistanceToTarget();
        while (xDistance > 5)
        {
            var xDirectionX = (_target.X - xPosition.X) / xDistance;
            var xDirectionY = (_target.Y - xPosition.Y) / xDistance;
            var xStepX = (int)(xPosition.X + xDirectionX * _pixelsPerFrame);
            var xStepY = (int)(xPosition.Y + xDirectionY * _pixelsPerFrame);

            _windowService.MoveTo(xStepX, xStepY);
            xPosition = _windowService.GetPosition();
            xDistance = GetDistanceToTarget();

            await Task.Delay((int)_milissecondsToNextFrame);
        }
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