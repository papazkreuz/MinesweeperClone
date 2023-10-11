using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

public class MinePlacer
{
    private readonly int _minesCount;
    private List<GridCell> _gridCells;
    
    public MinePlacer(int minesCount)
    {
        _minesCount = minesCount;
    }

    public List<GridCell> Place(List<GridCell> gridCells)
    {
        _gridCells = gridCells;
        
        Random random = new Random();
        List<GridCell> mineCells = _gridCells.OrderBy((_) => random.Next()).Take(_minesCount).ToList();

        foreach (GridCell gridCell in mineCells)
        {
            gridCell.SetCellType(CellType.Mine);
        }

        return mineCells;
    }
}
