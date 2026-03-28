using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using DesktopBuddy.Interfaces;
using DesktopBuddy.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace DesktopBuddy;

public partial class App
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
        var builder = WebApplication.CreateBuilder([]);

        builder.Services.AddSingleton<MainWindow>();
        builder.Services.AddSingleton<WindowService>();
        builder.Services.AddSingleton<StateMachineService>();
        builder.Services.AddKeyedScoped<IStateService<EState>, WalkingStateService>(nameof(EState.Walking));
        builder.Services.AddKeyedScoped<IStateService<EState>, GoingAfterMouseService>(nameof(EState.GoingAfterMouse));
        builder.Services.AddKeyedScoped<IStateService<EState>, IdleStateService>(nameof(EState.Idle));

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.WebHost.UseStaticWebAssets();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(
                Path.Combine(AppContext.BaseDirectory, "wwwroot")),
            RequestPath = ""
        });

        app.UseAntiforgery();

        app.MapRazorComponents<DesktopBuddy.FrontEnd.Components.App>()
            .AddInteractiveServerRenderMode();

        app.RunAsync();

        var mainWindow = (MainWindow)app.Services.GetRequiredService(typeof(MainWindow));
        mainWindow.Show();
    }
}