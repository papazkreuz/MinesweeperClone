using System;
using System.Threading;
using System.Threading.Tasks;
using Zenject;

public class GameTimerController : IInitializable, IDisposable
{
    [Inject] private readonly SignalBus _signalBus;
    
    private int _currentTime;
    private CancellationTokenSource _cancellationTokenSource;

    public void Initialize()
    {
        _signalBus.Subscribe<GameStartedSignal>(OnGameStarted);
        _signalBus.Subscribe<GameEndedSignal>(OnGameEnded);
    }
    
    public void Dispose()
    {
        if (_cancellationTokenSource != null)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
        
        _signalBus.Unsubscribe<GameStartedSignal>(OnGameStarted);
        _signalBus.Unsubscribe<GameEndedSignal>(OnGameEnded);
    }
    
    private async void Start()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _currentTime = 0;

        _signalBus.Fire(new TimerTickedSignal(_currentTime));
        
        try
        {
            while (true)
            {
                await TickSecond();
            }
        }
        catch (TaskCanceledException)
        {
            //Task is cancelled here, when timer is stopped
        }
        
    }

    private async Task TickSecond()
    {
        await Task.Delay(1000, _cancellationTokenSource.Token);

        if (_cancellationTokenSource.IsCancellationRequested == false)
        {
            _currentTime++;
            
            _signalBus.Fire(new TimerTickedSignal(_currentTime));
        }
    }
    
    private void OnGameStarted()
    {
        Start();
    }
    
    private void OnGameEnded()
    {
        _cancellationTokenSource.Cancel();

        _signalBus.Fire(new TimerEndedSignal(_currentTime));
    }
}
