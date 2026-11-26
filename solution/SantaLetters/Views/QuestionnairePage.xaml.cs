using System.ComponentModel;
using SantaLetters.Controls;
using SantaLetters.ViewModels;

namespace SantaLetters.Views;

public partial class QuestionnairePage : StoryPage
{
    private readonly QuestionnaireViewModel _vm;

    public QuestionnairePage(QuestionnaireViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
        _vm.PropertyChanged += OnViewModelPropertyChanged;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.Load();
    }

    // Each new question slides in like turning to the next card.
    private async void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(QuestionnaireViewModel.Index))
        {
            return;
        }

        QuestionCard.Opacity = 0;
        QuestionCard.TranslationX = 80;
        await Task.WhenAll(
            QuestionCard.FadeToAsync(1, 260, Easing.CubicOut),
            QuestionCard.TranslateToAsync(0, 0, 260, Easing.CubicOut));
    }
}
