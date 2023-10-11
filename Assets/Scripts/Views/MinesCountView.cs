using TMPro;
using UnityEngine;
using Zenject;

public class MinesCountView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _minesCountText;

    [Inject] private SignalBus _signalBus;
    [Inject] private int _minesCount;

    private int _markedCellsCount;
    
    private void Start()
    {
        UpdateMinesCountText();
        
        _signalBus.Subscribe<CellMarkedSignal>(OnCellMarked);
        _signalBus.Subscribe<GameEndedSignal>(OnGameEnded);
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<CellMarkedSignal>(OnCellMarked);
        _signalBus.Unsubscribe<GameEndedSignal>(OnGameEnded);
    }

    private void UpdateMinesCountText()
    {
        _minesCountText.text = $"Mines: {_markedCellsCount} / {_minesCount}";
    }
    
    private void OnCellMarked(CellMarkedSignal signalInfo)
    {
        if (signalInfo.GridCell.IsMarked)
            _markedCellsCount++;
        else
            _markedCellsCount--;
        
        UpdateMinesCountText();
    }

    private void OnGameEnded()
    {
        _markedCellsCount = 0;
        
        UpdateMinesCountText();
    }
}
