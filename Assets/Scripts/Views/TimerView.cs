using TMPro;
using UnityEngine;
using Zenject;

public class TimerView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;

    [Inject] private SignalBus _signalBus;
    
    private void Start()
    {
        _signalBus.Subscribe<TimerTickedSignal>(OnTimerTicked);
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<TimerTickedSignal>(OnTimerTicked);
    }

    private void UpdateTimerText(int seconds)
    {
        _timerText.text = $"Timer: {seconds}";
    }
    
    private void OnTimerTicked(TimerTickedSignal signalInfo)
    {
        UpdateTimerText(signalInfo.CurrentTime);
    }
}
