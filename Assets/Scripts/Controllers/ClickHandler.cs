using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

public class ClickHandler : IInitializable, IDisposable
{
    [Inject] private readonly SignalBus _signalBus;
    
    private List<GridCell> _gridCells;

    public void Initialize()
    {
        _signalBus.Subscribe<CellLeftClickedSignal>(OnCellLeftClicked);
        _signalBus.Subscribe<CellRightClickedSignal>(OnCellRightClicked);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<CellLeftClickedSignal>(OnCellLeftClicked);
        _signalBus.Unsubscribe<CellRightClickedSignal>(OnCellRightClicked);
    }

    public void Init(List<GridCell> gridCells)
    {
        _gridCells = gridCells;
    }
    
    private void OnCellLeftClicked(CellLeftClickedSignal signalInfo)
    {
        HandleLeftClick(signalInfo.GridCell);
    }

    private void OnCellRightClicked(CellRightClickedSignal signalInfo)
    {
        HandleRightClick(signalInfo.GridCell);
    }
    
    private void HandleLeftClick(GridCell gridCell)
    {
        if (gridCell.IsMarked)
        {
            return;
        }

        if (gridCell.IsOpened)
        {
            HandleOpenedCellClick(gridCell);
            return;
        }
        
        switch (gridCell.CellType)
        {
            case CellType.Empty:
                HandleEmptyCellClick(gridCell);
                break;
            case CellType.Number:
                HandleNumberCellClick(gridCell);
                break;
            case CellType.Mine:
                HandleMineCellClick(gridCell);
                break;
        }
    }

    private void HandleRightClick(GridCell gridCell)
    {
        if (gridCell.IsOpened)
        {
            return;
        }
            
        gridCell.SetMark(!gridCell.IsMarked);
        
        _signalBus.Fire(new CellMarkedSignal(gridCell));
    }

    private void HandleOpenedCellClick(GridCell gridCell)
    {
        List<GridCell> surroundCells = gridCell.GetSurroundCells(_gridCells);
        int marksAroundCount = surroundCells.Count((cell) => cell.IsMarked);
            
        if (gridCell.MinesAroundCount == marksAroundCount)
        {
            surroundCells.ForEach((cell) =>
            {
                if (cell.IsOpened == false)
                {
                    HandleLeftClick(cell);
                }
            });
        }
    }
    
    private void HandleEmptyCellClick(GridCell gridCell)
    {
        gridCell.Open();
        
        _signalBus.Fire(new CellOpenedSignal(gridCell));
        
        gridCell.GetSurroundCells(_gridCells).ForEach((cell) =>
        {
            if (cell.CellType != CellType.Mine)
            {
                HandleLeftClick(cell);
            }
        });
    }

    private void HandleNumberCellClick(GridCell gridCell)
    {
        gridCell.Open();
        
        _signalBus.Fire(new CellOpenedSignal(gridCell));
    }

    private void HandleMineCellClick(GridCell gridCell)
    {
        _signalBus.Fire(new MineCellClickedSignal(gridCell));
    }
}
