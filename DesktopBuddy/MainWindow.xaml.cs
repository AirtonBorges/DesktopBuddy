// csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DesktopBuddy.Services;
using MessageBox = System.Windows.MessageBox;

namespace DesktopBuddy;

public partial class MainWindow
{
    private readonly StateMachineService _stateMachineService;
    private readonly CancellationTokenSource _cancellationToken = new();
    private Thread _workerThread;

    public MainWindow(StateMachineService stateMachineService)
    {
        _stateMachineService = stateMachineService;
        InitializeComponent();
        _workerThread = new Thread(() => Tick(_cancellationToken.Token).GetAwaiter().GetResult());
        _workerThread.Start();
    }

    private async Task Tick(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await _stateMachineService.Tick().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Dispatcher.Invoke(() =>
                    MessageBox.Show(e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error));
                break;
            }
        }
    }

    protected override void OnClosed(EventArgs e)
    {
        _cancellationToken.Cancel();
        base.OnClosed(e);
    }
}