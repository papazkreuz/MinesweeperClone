public class GameEndedSignal
{
    private readonly GameResult _gameResult;

    public GameResult GameResult => _gameResult;
    
    public GameEndedSignal(GameResult gameResult)
    {
        _gameResult = gameResult;
    }
}