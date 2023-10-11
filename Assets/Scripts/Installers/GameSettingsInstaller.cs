using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameSettingsInstaller : MonoInstaller<GameSettingsInstaller>
{
    [Header("Count parameters")]
    [SerializeField] private int _minesCount;
    [SerializeField] private int _widthCellsCount;
    [SerializeField] private int _heightCellsCount;

    [Header("Prefabs")]
    [SerializeField] private GridCellView _gridCellViewPrefab;

    [Header("Misc")]
    [SerializeField] private GridLayoutGroup _gridCellsLayoutGroup;

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        
        DeclareGameSignals();
        DeclareCellSignals();
        DeclareTimerSignals();

        BindViews();
        BindFactories();
        BindControllers();
    }

    #region Signals

    private void DeclareGameSignals()
    {
        Container.DeclareSignal<GameStartedSignal>().RunAsync();
        Container.DeclareSignal<GameEndedSignal>().RunAsync();
        Container.DeclareSignal<GameRestartRequestedSignal>().RunAsync();
    }
    
    private void DeclareCellSignals()
    {
        Container.DeclareSignal<CellLeftClickedSignal>();
        Container.DeclareSignal<CellRightClickedSignal>();
        Container.DeclareSignal<CellOpenedSignal>();
        Container.DeclareSignal<CellMarkedSignal>();
        Container.DeclareSignal<MineCellClickedSignal>();
    }

    private void DeclareTimerSignals()
    {
        Container.DeclareSignal<TimerTickedSignal>().RunAsync();
        Container.DeclareSignal<TimerEndedSignal>().RunAsync();
    }

    #endregion

    #region Bindings

    private void BindViews()
    {
        Container
            .BindInstance(_minesCount)
            .WhenInjectedInto<MinesCountView>();
    }

    private void BindFactories()
    {
        Container
            .Bind<GridCellView.Factory>()
            .AsSingle();
        Container
            .BindIFactory<GridCellView, GridCellView.Factory>()
            .AsSingle();
    }

    private void BindControllers()
    {
        Container
            .Bind<GridBuilder>()
            .AsSingle()
            .WithArguments(_gridCellsLayoutGroup, _gridCellViewPrefab, _widthCellsCount, _heightCellsCount);

        Container
            .Bind<MinePlacer>()
            .AsSingle()
            .WithArguments(_minesCount);

        Container
            .Bind<NumberPlacer>()
            .AsSingle();

        Container
            .BindInterfacesAndSelfTo<ClickHandler>()
            .AsSingle();

        Container
            .BindInterfacesAndSelfTo<GameResultSolver>()
            .AsSingle()
            .WithArguments(_minesCount);

        Container
            .BindInterfacesAndSelfTo<GameTimerController>()
            .AsSingle();

        Container
            .BindInterfacesAndSelfTo<GameInitializer>()
            .AsSingle();
    }
    
    #endregion
}