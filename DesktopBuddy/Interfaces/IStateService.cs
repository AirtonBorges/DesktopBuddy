using System;
using System.Threading.Tasks;
using DesktopBuddy.Services;

namespace DesktopBuddy.Interfaces;

public interface IStateService<TState> where TState : Enum
{
    public Task Tick();
    Task<EState> GetState();
}