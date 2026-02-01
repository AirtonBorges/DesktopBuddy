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
        var xWaitTime = _random.Next(_minimumWaitTime, _maximumWaitTime);
        Thread.Sleep(xWaitTime);

        return Task.CompletedTask;
    }

    public Task<EState> GetState()
    {
        return Task.FromResult(EState.Walking);
    }
}