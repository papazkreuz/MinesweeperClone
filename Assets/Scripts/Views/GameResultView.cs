using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameResultView : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _resultText;
    [SerializeField] private TextMeshProUGUI _resultDescriptionText;
    [SerializeField] private Button _restartButton;

    [Inject] private readonly SignalBus _signalBus;

    private TaskCompletionSource<GameResult> _gameResultReadyTaskCompletionSource;
    private TaskCompletionSource<int> _finalTimeReadyTaskCompletionSource;

    private void Start()
    {
        ResetTaskCompletionSources();
        SetState(false, 0f);
        
        _signalBus.Subscribe<GameEndedSignal>(OnGameEnded);
        _signalBus.Subscribe<TimerEndedSignal>(OnTimerEnded);
        
        _restartButton.onClick.AddListener(OnRestartButtonPressed);
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<GameEndedSignal>(OnGameEnded);
        _signalBus.Unsubscribe<TimerEndedSignal>(OnTimerEnded);
        
        _restartButton.onClick.RemoveListener(OnRestartButtonPressed);
    }

    private async Task Init()
    {
        await Task.WhenAll(_gameResultReadyTaskCompletionSource.Task, _finalTimeReadyTaskCompletionSource.Task);

        GameResult gameResult = _gameResultReadyTaskCompletionSource.Task.Result;
        int finalTime = _finalTimeReadyTaskCompletionSource.Task.Result;
        
        _resultText.text = gameResult.IsWin ? "Win! :)" : "Lose! :(";
        _resultDescriptionText.text = $"You've found {gameResult.CorrectMinesMarkedCount} mines out of {gameResult.TotalMinesCount} in {finalTime} seconds";
        
        ResetTaskCompletionSources();
    }
    
    private void SetState(bool state, float duration = 0.2f)
    {
        _canvasGroup.DOFade(state ? 1f : 0f, duration).onComplete += () => _canvasGroup.blocksRaycasts = state;
    }

    private void ResetTaskCompletionSources()
    {
        _gameResultReadyTaskCompletionSource = new TaskCompletionSource<GameResult>();
        _finalTimeReadyTaskCompletionSource = new TaskCompletionSource<int>();
    }
    
    private void OnRestartButtonPressed()
    { 
        _signalBus.Fire(new GameRestartRequestedSignal());
        SetState(false);
    }

    private async void OnGameEnded(GameEndedSignal signalInfo)
    {
        _gameResultReadyTaskCompletionSource.SetResult(signalInfo.GameResult);

        await Init();
        SetState(true);
    }

    private void OnTimerEnded(TimerEndedSignal signalInfo)
    {
        _finalTimeReadyTaskCompletionSource.SetResult(signalInfo.CurrentTime);
    }
}
