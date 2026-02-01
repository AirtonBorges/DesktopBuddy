using System;
using System.Windows;
using DesktopBuddy.Interfaces;
using DesktopBuddy.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DesktopBuddy
{
    public partial class App : Application
    {
        public IServiceProvider Services { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();

            services.AddSingleton<MainWindow>();
            services.AddSingleton<WindowService>();
            services.AddSingleton<StateMachineService>();
            services.AddKeyedScoped<IStateService<EState>, WalkingStateService>(nameof(EState.Walking));
            services.AddKeyedScoped<IStateService<EState>, IdleStateService>(nameof(EState.Idle));

            Services = services.BuildServiceProvider();
            
            var mainWindow = (MainWindow)Services.GetRequiredService(typeof(MainWindow));
            mainWindow.Show();
        }
    }
}
