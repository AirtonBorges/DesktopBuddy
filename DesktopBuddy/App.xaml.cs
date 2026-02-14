using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using DesktopBuddy.Interfaces;
using DesktopBuddy.Services;
using Microsoft.Extensions.DependencyInjection;
using Application = System.Windows.Application;

namespace DesktopBuddy
{
    public partial class App : Application
    {
        public IServiceProvider Services { get; private set; } = null!;
        private NotifyIcon _notifyIcon;

        protected override void OnStartup(StartupEventArgs e)
        {
            ConfigureTrayIcon();
            ConfigureIoC();
        }

        private void ConfigureTrayIcon()
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Icon = new Icon("assets/mac.ico"); 
            _notifyIcon.Visible = true;
            _notifyIcon.Text = "Desktop Kitty";

            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Sair", null, (s, e) => Shutdown());
            _notifyIcon.ContextMenuStrip = contextMenu;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (_notifyIcon != null)
            {
                _notifyIcon.Dispose();
            }
            base.OnExit(e);
        }

        private void ConfigureIoC()
        {
            var services = new ServiceCollection();

            services.AddSingleton<MainWindow>();
            services.AddSingleton<WindowService>();
            services.AddSingleton<StateMachineService>();
            services.AddKeyedScoped<IStateService<EState>, WalkingStateService>(nameof(EState.Walking));
            services.AddKeyedScoped<IStateService<EState>, GoingAfterMouseService>(nameof(EState.GoingAfterMouse));
            services.AddKeyedScoped<IStateService<EState>, IdleStateService>(nameof(EState.Idle));

            Services = services.BuildServiceProvider();
            
            var mainWindow = (MainWindow)Services.GetRequiredService(typeof(MainWindow));
            mainWindow.Show();
        }
    }
}
