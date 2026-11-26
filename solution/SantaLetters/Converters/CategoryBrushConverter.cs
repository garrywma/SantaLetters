using System.Globalization;
using SantaLetters.Core.Models;

namespace SantaLetters.Converters;

/// <summary>Gives each present category its own softly-coloured gift-wrap gradient.</summary>
public sealed class CategoryBrushConverter : IValueConverter
{
    private static readonly Dictionary<PresentCategory, Brush> Brushes = new()
    {
        [PresentCategory.Toys] = Wrap("#FBE3DA", "#E9B2A3"),
        [PresentCategory.Games] = Wrap("#ECE3F4", "#C8B3E0"),
        [PresentCategory.Outdoors] = Wrap("#E3F0DC", "#B3D3A4"),
        [PresentCategory.Creative] = Wrap("#FDF0D5", "#F0C67E"),
        [PresentCategory.Books] = Wrap("#F1E6D2", "#D3B88E"),
        [PresentCategory.Tech] = Wrap("#DEEAF3", "#A6C2DA"),
    };

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is PresentCategory category && Brushes.TryGetValue(category, out var brush)
            ? brush
            : Brushes[PresentCategory.Books];

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();

    private static LinearGradientBrush Wrap(string top, string bottom) => new(
        [new GradientStop(Color.FromArgb(top), 0f), new GradientStop(Color.FromArgb(bottom), 1f)],
        new Point(0, 0), new Point(1, 1));
}
