public class AbstractTimerSignal
{
    private readonly int _currentTime;

    public int CurrentTime => _currentTime;

    protected AbstractTimerSignal(int currentTime)
    {
        _currentTime = currentTime;
    }
}