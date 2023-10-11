using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridCell
{
    private readonly int _widthIndex;
    private readonly int _heightIndex;
    
    public CellType CellType { get; private set; }
    public int MinesAroundCount { get; private set; }
    public bool IsOpened { get; private set; }
    public bool IsMarked { get; private set; }

    private List<GridCell> _surroundCells;

    public GridCell(int widthIndex, int heightIndex)
    {
        _widthIndex = widthIndex;
        _heightIndex = heightIndex;
    }

    public override bool Equals(object obj)
    {
        GridCell gridCell = obj as GridCell;

        if (gridCell == null)
        {
            return false;
        }

        return gridCell._widthIndex == _widthIndex &&
               gridCell._heightIndex == _heightIndex;
    }

    public override int GetHashCode()
    {
        return _widthIndex.GetHashCode() ^ _heightIndex.GetHashCode();
    }
    
    public void IncrementMinesAroundCount()
    {
        MinesAroundCount++;
    }
    
    public void SetCellType(CellType cellType)
    {
        CellType = cellType;
    }

    public void Open()
    {
        if (IsOpened)
        {
            return;
        }

        IsOpened = true;
    }
    
    public void SetMark(bool state)
    {
        IsMarked = state;
    }

    public List<GridCell> GetSurroundCells(List<GridCell> allCells)
    {
        if (_surroundCells == null)
        {
            _surroundCells = allCells.Where((cell) => cell.IsNear(this)).ToList();
        }

        return _surroundCells; 
    }
    
    private bool IsNear(GridCell gridCell)
    {
        int widthRange = Mathf.Abs(_widthIndex - gridCell._widthIndex);
        int heightRange = Mathf.Abs(_heightIndex - gridCell._heightIndex);
        
        return Mathf.Max(widthRange, heightRange) == 1;
    }
}