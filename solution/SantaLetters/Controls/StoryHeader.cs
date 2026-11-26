namespace SantaLetters.Controls;

/// <summary>
/// The storybook page header: twinkling lights along the top (safely under the Portal's system
/// pills), a hand-lettered title, optional subtitle/accessory on the right and a gold rule.
/// </summary>
public sealed class StoryHeader : ContentView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title), typeof(string), typeof(StoryHeader), string.Empty,
        propertyChanged: (b, _, n) => ((StoryHeader)b)._title.Text = (string)n);

    public static readonly BindableProperty SubtitleProperty = BindableProperty.Create(
        nameof(Subtitle), typeof(string), typeof(StoryHeader), string.Empty,
        propertyChanged: (b, _, n) =>
        {
            var header = (StoryHeader)b;
            header._subtitle.Text = (string)n;
            header._subtitle.IsVisible = !string.IsNullOrEmpty((string)n);
        });

    public static readonly BindableProperty AccessoryProperty = BindableProperty.Create(
        nameof(Accessory), typeof(View), typeof(StoryHeader),
        propertyChanged: (b, _, n) => ((StoryHeader)b)._accessory.Content = (View?)n);

    public static readonly BindableProperty BandProperty = BindableProperty.Create(
        nameof(Band), typeof(Brush), typeof(StoryHeader),
        propertyChanged: (b, _, n) => ((StoryHeader)b)._band.Background = (Brush?)n);

    private readonly Grid _band;
    private readonly Label _title;
    private readonly Label _subtitle;
    private readonly ContentView _accessory = new() { VerticalOptions = LayoutOptions.Center };

    public StoryHeader()
    {
        var resources = Application.Current!.Resources;

        _title = new Label
        {
            Style = (Style)resources["HeaderTitle"],
            VerticalOptions = LayoutOptions.Center,
            LineBreakMode = LineBreakMode.TailTruncation,
            Shadow = new Shadow { Brush = new SolidColorBrush(Colors.Black), Offset = new Point(2, 3), Radius = 4, Opacity = 0.5f },
        };
        SemanticProperties.SetHeadingLevel(_title, SemanticHeadingLevel.Level1);

        _subtitle = new Label { Style = (Style)resources["HeaderSubtitle"], IsVisible = false };

        var titleRow = new Grid
        {
            ColumnDefinitions = { new(GridLength.Star), new(GridLength.Auto) },
            ColumnSpacing = 24,
            Padding = new Thickness(40, 0, 40, 12),
        };
        titleRow.Add(_title);
        titleRow.Add(new HorizontalStackLayout { Spacing = 24, Children = { _subtitle, _accessory } }, 1);

        _band = new Grid
        {
            RowDefinitions =
            {
                new(new GridLength((double)resources["PortalTopInset"])),
                new(GridLength.Auto),
                new(new GridLength(5)),
            },
            Background = (Brush)resources["HeaderBrush"],
        };
        var snow = new SnowView { FlakeCount = 24 };
        Grid.SetRowSpan(snow, 2);
        _band.Add(snow);
        _band.Add(new FairyLightsView(), 0, 0);
        _band.Add(titleRow, 0, 1);
        _band.Add(new BoxView { Background = (Brush)resources["GoldLeafBrush"] }, 0, 2);

        Content = _band;
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Subtitle
    {
        get => (string)GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }

    public View? Accessory
    {
        get => (View?)GetValue(AccessoryProperty);
        set => SetValue(AccessoryProperty, value);
    }

    /// <summary>Optional override for the header background (defaults to berry red).</summary>
    public Brush? Band
    {
        get => (Brush?)GetValue(BandProperty);
        set => SetValue(BandProperty, value);
    }
}
