using System.Collections.Generic;
using Zenject;

public class GameInitializer : IInitializable
{
    [Inject] private readonly GridBuilder _gridBuilder;
    [Inject] private readonly MinePlacer _minePlacer;
    [Inject] private readonly NumberPlacer _numberPlacer;
    [Inject] private readonly ClickHandler _clickHandler;
    [Inject] private readonly GameResultSolver _gameResultSolver;
    [Inject] private readonly SignalBus _signalBus;

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
