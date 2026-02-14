using System;
using System.Threading;
using System.Threading.Tasks;
using DesktopBuddy.Interfaces;

namespace DesktopBuddy.Services;

public class IdleStateService: IStateService<EState>
{
    private readonly Random _random = new();
    private readonly int _minimumWaitTime = 500;
    private readonly int _maximumWaitTime = 2000;

    public Task Tick()
    {
        var waitTime = _random.Next(_minimumWaitTime, _maximumWaitTime);
        Thread.Sleep(waitTime);

        return Task.CompletedTask;
    }

    public Task<EState> GetState()
    {
        const double changeOfGoingAfterMouse = 0.2;
        var random = _random.NextDouble();
        if (random < changeOfGoingAfterMouse)
        {
            return Task.FromResult(EState.GoingAfterMouse);
        }

        return Task.FromResult(EState.Walking);
    }
}