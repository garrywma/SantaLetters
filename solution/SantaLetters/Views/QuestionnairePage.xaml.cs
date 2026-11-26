using SantaLetters.ViewModels;

namespace SantaLetters.Views;

public partial class QuestionnairePage : ContentPage
{
    private readonly QuestionnaireViewModel _vm;

    public QuestionnairePage(QuestionnaireViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.Load();
    }
}
