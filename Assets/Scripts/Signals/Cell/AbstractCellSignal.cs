public abstract class AbstractCellSignal
{
    private readonly GridCell _gridCell;

    public GridCell GridCell => _gridCell;

    protected AbstractCellSignal(GridCell gridCell)
    {
        _gridCell = gridCell;
    }
}