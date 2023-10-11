using System.Collections.Generic;

public class NumberPlacer
{
    private List<GridCell> _mineCells;

    public void Place(List<GridCell> gridCells, List<GridCell> mineCells)
    {
        _mineCells = mineCells;
        
        foreach (GridCell mineCell in _mineCells)
        {
            mineCell.GetSurroundCells(gridCells).ForEach((surroundCell) =>
            {
                if (surroundCell.CellType == CellType.Empty)
                {
                    surroundCell.SetCellType(CellType.Number);
                }

                surroundCell.IncrementMinesAroundCount();
            });
        }
    }
}
