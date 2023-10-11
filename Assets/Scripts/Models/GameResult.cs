public class GameResult
{
    private readonly bool _isWin;
    private readonly int _correctMinesMarkedCount;
    private readonly int _totalMinesCount;

    public bool IsWin => _isWin;
    public int CorrectMinesMarkedCount => _correctMinesMarkedCount;
    public int TotalMinesCount => _totalMinesCount;

    public GameResult(bool isWin, int correctMinesMarkedCount, int totalMinesCount)
    {
        _isWin = isWin;
        _correctMinesMarkedCount = correctMinesMarkedCount;
        _totalMinesCount = totalMinesCount;
    }

}