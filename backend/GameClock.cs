using System;
using System.Timers;

namespace NathanColeman.IndieGameDev.Backend;

public class GameClock
{
    private const double TimerFrequencyInSeconds = 1;
    private const double SecondsPerDay = 4;

    private DateOnly _baseDate;
    private double _secondsElapsed = 0;
    private readonly Timer? _timer;

    public GameSpeed Speed { get; set; } = GameSpeed.Normal;
    public DateOnly Date
    {
        get
        {
            var daysElapsed = (int)(_secondsElapsed / SecondsPerDay);

            return _baseDate.AddDays(daysElapsed);
        }
        set
        {
            _secondsElapsed = 0;
            _baseDate = value;
        }
    }

    public GameClock(DateOnly startDate, bool useInternalTimer)
    {
        _baseDate = startDate;
        if (useInternalTimer)
        {
            _timer = new Timer(TimerFrequencyInSeconds * 1000);
            _timer.AutoReset = true;
            _timer.Elapsed += (_, _) => Process(TimerFrequencyInSeconds);
            _timer.Start();
        }
    }

    public event Action<DateOnly>? DateChanged;

    public void Start() => _timer?.Start();
    public void Stop() => _timer?.Stop();

    public void Process(double delta)
    {
        var oldDate = Date;
        _secondsElapsed += delta * (double)Speed;
        var newDate = Date;

        if (newDate != oldDate) DateChanged?.Invoke(newDate);
    }
}
