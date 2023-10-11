using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

public class GameResultSolver : IInitializable, IDisposable
{
    private readonly int _minesCount;
    private readonly List<GridCell> _markedCells;

    [Inject] private SignalBus _signalBus;

    public GameResultSolver(int minesCount)
    {
        _minesCount = minesCount;
        _markedCells = new List<GridCell>();
    }
    
    public void Initialize()
    {
        _signalBus.Subscribe<CellMarkedSignal>(OnCellMarked);
        _signalBus.Subscribe<MineCellClickedSignal>(OnMineCellClicked);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<CellMarkedSignal>(OnCellMarked);
        _signalBus.Unsubscribe<MineCellClickedSignal>(OnMineCellClicked);
    }

    public void Clear()
    {
        _markedCells.Clear();
    }
    
    private void OnCellMarked(CellMarkedSignal signalInfo)
    {
        if (signalInfo.GridCell.IsMarked)
        {
            _markedCells.Add(signalInfo.GridCell);
        }
        else
        {
            _markedCells.Remove(signalInfo.GridCell);
        }
        
        if (_markedCells.Count != _minesCount)
            return;

        foreach (GridCell markedCell in _markedCells)
        {
            if (markedCell.CellType != CellType.Mine)
                return;
        }

        int correctMinesMarkedCount = _markedCells.Count((cell) => cell.CellType == CellType.Mine);
        
        _signalBus.Fire(new GameEndedSignal(new GameResult(true, correctMinesMarkedCount, _minesCount)));
    }

    private void OnMineCellClicked(MineCellClickedSignal signalInfo)
    {
        int correctMinesMarkedCount = _markedCells.Count((cell) => cell.CellType == CellType.Mine);
        
        _signalBus.Fire(new GameEndedSignal(new GameResult(false, correctMinesMarkedCount, _minesCount)));
    }
}
