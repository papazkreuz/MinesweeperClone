using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GridCellView : MonoBehaviour
{
    [SerializeField] private LeftRightPointerClickHandler _button;
    [SerializeField] private TextMeshProUGUI _numberText;
    [SerializeField] private Image _foregroundImage;
    [SerializeField] private Image _markImage;

    [Inject] private SignalBus _signalBus;
    
    private GridCell _gridCell;

    private void Start()
    {
        _signalBus.Subscribe<GameStartedSignal>(OnGameStarted);
        _signalBus.Subscribe<CellOpenedSignal>(OnCellOpened);
        _signalBus.Subscribe<CellMarkedSignal>(OnCellMarked);

        _button.Initialize(OnLeftClick, OnRightClick);
    }

    private void OnDestroy()
    {
        _signalBus?.Unsubscribe<GameStartedSignal>(OnGameStarted);
        _signalBus?.Unsubscribe<CellOpenedSignal>(OnCellOpened);
        _signalBus?.Unsubscribe<CellMarkedSignal>(OnCellMarked);
        
        _button.Dispose();
    }
    
    public void Init(GridCell gridCell)
    {
        _gridCell = gridCell;
    }

    private void SetMark(bool state)
    {
        _markImage.enabled = state;
    }
    
    private void SetNumberText()
    {
        if (_gridCell.CellType == CellType.Number)
            _numberText.text = $"{_gridCell.MinesAroundCount}";
    }
    
    private void OnLeftClick()
    {
        _signalBus?.Fire(new CellLeftClickedSignal(_gridCell));
    }
    
    private void OnRightClick()
    {
        _signalBus?.Fire(new CellRightClickedSignal(_gridCell));
    }

    private void OnCellOpened(CellOpenedSignal signalInfo)
    {
        if (signalInfo.GridCell.Equals(_gridCell))
        {
            _foregroundImage.enabled = false;
        }
    }

    private void OnCellMarked(CellMarkedSignal signalInfo)
    {
        if (signalInfo.GridCell.Equals(_gridCell))
        {
            SetMark(signalInfo.GridCell.IsMarked);
        }
    }
    
    private void OnGameStarted()
    {
        SetNumberText();
    }

    public class Factory : PrefabFactory<GridCellView>
    {
    }
}