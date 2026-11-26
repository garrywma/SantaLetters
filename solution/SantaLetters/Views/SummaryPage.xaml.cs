using System.ComponentModel;
using SantaLetters.Controls;
using SantaLetters.ViewModels;

namespace SantaLetters.Views;

public partial class SummaryPage : StoryPage
{
    private readonly SummaryViewModel _vm;

    public SummaryPage(SummaryViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
        _vm.PropertyChanged += OnViewModelPropertyChanged;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ResetSendAnimation();
        _vm.Load();
    }

    private async void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SummaryViewModel.IsSent) && _vm.IsSent)
        {
            await PlaySendAnimationAsync();
        }
    }

    private void ResetSendAnimation()
    {
        SentOverlay.Opacity = 0;
        LetterSheet.TranslationY = -200;
        Flap.RotationX = 180;
        Flap.ZIndex = 1;
        Seal.Opacity = 0;
        Seal.Scale = 1;
        Envelope.TranslationX = Envelope.TranslationY = Envelope.Rotation = 0;
        Envelope.Scale = Envelope.Opacity = 1;
        SentTitle.Opacity = SentSubtitle.Opacity = 0;
        Sleigh.Opacity = 0;
    }

    // Roughly 3.5 seconds; SummaryViewModel waits long enough for this to finish before going home.
    private async Task PlaySendAnimationAsync()
    {
        ResetSendAnimation();
        await SentOverlay.FadeToAsync(1, 250);

        // Slide the letter into the envelope, then fold the flap over it.
        await LetterSheet.TranslateToAsync(0, 0, 550, Easing.CubicInOut);
        Flap.ZIndex = 4;
        await Flap.RotateXToAsync(0, 450, Easing.CubicOut);

        // Stamp the wax seal down with a little thud.
        Seal.Scale = 2.6;
        await Task.WhenAll(Seal.FadeToAsync(1, 160), Seal.ScaleToAsync(1, 300, Easing.CubicIn));
        await Envelope.TranslateToAsync(0, 8, 60);
        await Envelope.TranslateToAsync(0, 0, 100);
        await Task.Delay(250);

        // Off it goes, with the reindeer leading the way.
        Sleigh.TranslationX = -320;
        Sleigh.TranslationY = Height * 0.22;
        Sleigh.Opacity = 1;
        _ = Sleigh.TranslateToAsync(Width + 80, Height * 0.04, 1500, Easing.SinInOut);

        await Task.WhenAll(
            Envelope.TranslateToAsync(Width * 0.45, -Height * 0.55, 900, Easing.CubicIn),
            Envelope.RotateToAsync(-18, 900),
            Envelope.ScaleToAsync(0.3, 900, Easing.CubicIn),
            Envelope.FadeToAsync(0, 900, Easing.CubicIn));

        await Task.WhenAll(SentTitle.FadeToAsync(1, 400), SentSubtitle.FadeToAsync(1, 700));
    }
}
