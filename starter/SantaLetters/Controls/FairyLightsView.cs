namespace SantaLetters.Controls;

/// <summary>A swagged string of twinkling Christmas lights.</summary>
public sealed class FairyLightsView : AnimatedGraphicsView
{
    private readonly FairyLightsDrawable _drawable = new();

    public FairyLightsView() => Drawable = _drawable;

    protected override void OnTick(double seconds, double deltaSeconds) => _drawable.Seconds = seconds;

    private sealed class FairyLightsDrawable : IDrawable
    {
        private static readonly Color[] Bulbs =
        [
            Color.FromArgb("#FF4D4D"),
            Color.FromArgb("#FFD45C"),
            Color.FromArgb("#5CE08A"),
            Color.FromArgb("#5CB8FF"),
            Color.FromArgb("#FFF3D1"),
        ];

        private static readonly Color Wire = Color.FromArgb("#1C2A1E");

        public double Seconds { get; set; }

        public void Draw(ICanvas canvas, RectF rect)
        {
            if (rect.Width <= 0 || rect.Height <= 0)
            {
                return;
            }

            const int bulbsPerSwag = 4;
            var swags = Math.Max(3, (int)(rect.Width / 240));
            var swagWidth = rect.Width / swags;
            var top = rect.Height * 0.12f;
            var sag = rect.Height * 0.36f;

            var wire = new PathF();
            wire.MoveTo(rect.Left, top);
            for (var s = 0; s < swags; s++)
            {
                var x0 = rect.Left + s * swagWidth;
                // A quadratic control point at 2x the sag puts the lowest point of the curve at the sag.
                wire.QuadTo(x0 + swagWidth / 2, top + sag * 2, x0 + swagWidth, top);
            }

            canvas.StrokeColor = Wire;
            canvas.StrokeSize = 3;
            canvas.DrawPath(wire);

            var index = 0;
            for (var s = 0; s < swags; s++)
            {
                var x0 = rect.Left + s * swagWidth;
                for (var b = 0; b < bulbsPerSwag; b++, index++)
                {
                    var t = (b + 0.5f) / bulbsPerSwag;
                    var x = x0 + swagWidth * t;
                    var y = (1 - t) * (1 - t) * top + 2 * (1 - t) * t * (top + sag * 2) + t * t * top;
                    DrawBulb(canvas, x, y, Bulbs[index % Bulbs.Length], index);
                }
            }
        }

        private void DrawBulb(ICanvas canvas, float x, float y, Color colour, int index)
        {
            // Each bulb twinkles at its own speed and phase so the string never pulses in unison.
            var speed = 1.6 + (index * 37 % 10) / 8.0;
            var glow = (float)(0.5 + 0.5 * Math.Sin(Seconds * speed + index * 1.9));

            canvas.FillColor = colour.WithAlpha(0.1f + 0.25f * glow);
            canvas.FillCircle(x, y + 14, 7 + 8 * glow);

            canvas.FillColor = Wire;
            canvas.FillRoundedRectangle(x - 4, y - 1, 8, 7, 2);

            canvas.FillColor = colour.WithAlpha(0.55f + 0.45f * glow);
            canvas.FillEllipse(x - 6, y + 5, 12, 19);

            canvas.FillColor = Colors.White.WithAlpha(0.35f + 0.4f * glow);
            canvas.FillEllipse(x - 3, y + 8, 4, 7);
        }
    }
}
