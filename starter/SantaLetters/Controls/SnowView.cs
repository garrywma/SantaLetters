namespace SantaLetters.Controls;

/// <summary>Gently falling snow. Input-transparent, so it can sit on top of anything.</summary>
public sealed class SnowView : AnimatedGraphicsView
{
    public static readonly BindableProperty FlakeCountProperty =
        BindableProperty.Create(nameof(FlakeCount), typeof(int), typeof(SnowView), 60);

    private readonly SnowDrawable _drawable = new();

    public SnowView() => Drawable = _drawable;

    public int FlakeCount
    {
        get => (int)GetValue(FlakeCountProperty);
        set => SetValue(FlakeCountProperty, value);
    }

    protected override void OnTick(double seconds, double deltaSeconds)
    {
        _drawable.FlakeCount = FlakeCount;
        _drawable.Advance(seconds, deltaSeconds);
    }

    private sealed class SnowDrawable : IDrawable
    {
        private readonly Random _random = new();
        private Flake[] _flakes = [];
        private float _width, _height;
        private double _seconds, _delta;

        public int FlakeCount { get; set; }

        public void Advance(double seconds, double delta)
        {
            _seconds = seconds;
            _delta = delta;
        }

        public void Draw(ICanvas canvas, RectF rect)
        {
            if (rect.Width <= 0 || rect.Height <= 0)
            {
                return;
            }

            if (_flakes.Length != FlakeCount || rect.Width != _width || rect.Height != _height)
            {
                _width = rect.Width;
                _height = rect.Height;
                _flakes = [.. Enumerable.Range(0, FlakeCount).Select(_ => NewFlake(randomY: true))];
            }

            for (var i = 0; i < _flakes.Length; i++)
            {
                ref var f = ref _flakes[i];
                f.Y += (float)(f.Speed * _delta);
                f.X += (float)(Math.Sin(_seconds * f.Wobble + f.Phase) * 18 * _delta);
                if (f.Y - f.Radius > _height)
                {
                    f = NewFlake(randomY: false);
                }

                canvas.FillColor = Colors.White.WithAlpha(f.Alpha);
                canvas.FillCircle(f.X, f.Y, f.Radius);
            }

            // Each frame only moves the flakes once, even if the view is redrawn for another reason.
            _delta = 0;
        }

        private Flake NewFlake(bool randomY)
        {
            var radius = 1.5f + (float)_random.NextDouble() * 3.5f;
            return new Flake
            {
                X = (float)_random.NextDouble() * _width,
                Y = randomY ? (float)_random.NextDouble() * _height : -radius,
                Radius = radius,
                // Bigger flakes are "closer", so they fall faster and look brighter.
                Speed = 18 + radius * 9 + (float)_random.NextDouble() * 10,
                Alpha = 0.45f + radius / 10f,
                Wobble = 0.6 + _random.NextDouble(),
                Phase = _random.NextDouble() * Math.PI * 2,
            };
        }

        private struct Flake
        {
            public float X, Y, Radius, Speed, Alpha;
            public double Wobble, Phase;
        }
    }
}
