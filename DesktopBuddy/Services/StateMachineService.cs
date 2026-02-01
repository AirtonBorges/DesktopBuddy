using System;
using System.Threading.Tasks;
using DesktopBuddy.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DesktopBuddy.Services;

public enum EState
{
    Idle,
    Walking,
    GoingAfterMouse
}

public class StateMachineService
{
    private EState StateAtual { get; set; }
    private readonly IServiceProvider _pProvider;

    public StateMachineService(IServiceProvider pProvider)
    {
        _pProvider = pProvider;
    }

    public async Task Tick()
    {
        var estado = _pProvider.GetKeyedService<IStateService<EState>>(StateAtual.ToString());

        if (estado != null)
        {
            await estado.Tick();
            StateAtual = await estado.GetState();
        };
    }
}
