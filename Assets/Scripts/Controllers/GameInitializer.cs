using System.Collections.Generic;
using Zenject;

public class GameInitializer : IInitializable
{
    private readonly GridBuilder _gridBuilder;
    private readonly MinePlacer _minePlacer;
    private readonly NumberPlacer _numberPlacer;
    private readonly ClickHandler _clickHandler;
    private readonly GameResultSolver _gameResultSolver;

    [Inject] private SignalBus _signalBus;
    
    public GameInitializer(
        GridBuilder gridBuilder, 
        MinePlacer minePlacer, 
        NumberPlacer numberPlacer, 
        ClickHandler clickHandler,
        GameResultSolver gameResultSolver
        )
    {
        _gridBuilder = gridBuilder;
        _minePlacer = minePlacer;
        _numberPlacer = numberPlacer;
        _clickHandler = clickHandler;
        _gameResultSolver = gameResultSolver;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<GameRestartRequestedSignal>(Restart);
        
        Start();
    }

    private void Start()
    {
        List<GridCell> gridCells = _gridBuilder.Build();
        List<GridCell> mineCells = _minePlacer.Place(gridCells);
        _numberPlacer.Place(gridCells, mineCells);
        _clickHandler.Init(gridCells);
        
        _signalBus.Fire(new GameStartedSignal());
    }

    private void Restart()
    {
        _gridBuilder.ClearContainer();
        _gameResultSolver.Clear();

        Start();
    }
}
