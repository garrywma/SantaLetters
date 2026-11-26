using System.Diagnostics;

namespace SantaLetters.Controls;

/// <summary>
/// A GraphicsView that redraws itself on a timer while it is on screen.
/// The timer stops when the view is unloaded so hidden pages cost nothing.
/// </summary>
public abstract class AnimatedGraphicsView : GraphicsView
{
    private readonly Stopwatch _clock = new();
    private IDispatcherTimer? _timer;
    private double _lastSeconds;

    protected AnimatedGraphicsView()
    {
        InputTransparent = true;
        BackgroundColor = Colors.Transparent;
        Loaded += (_, _) => Start();
        Unloaded += (_, _) => Stop();
    }

    // ~30fps keeps the Portal's CPU (and battery) happy.
    protected virtual TimeSpan FrameInterval => TimeSpan.FromMilliseconds(33);

    protected abstract void OnTick(double seconds, double deltaSeconds);

    private void Start()
    {
        if (_timer is null)
        {
            _timer = Dispatcher.CreateTimer();
            _timer.Interval = FrameInterval;
            _timer.Tick += (_, _) =>
            {
                var now = _clock.Elapsed.TotalSeconds;
                OnTick(now, Math.Min(now - _lastSeconds, 0.1));
                _lastSeconds = now;
                Invalidate();
            };
        }

        _clock.Start();
        _lastSeconds = _clock.Elapsed.TotalSeconds;
        _timer.Start();
    }

    private void Stop()
    {
        _timer?.Stop();
        _clock.Stop();
    }
}
