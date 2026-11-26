using SantaLetters.ViewModels;

namespace SantaLetters.Views;

public partial class LetterPage : ContentPage
{
    private readonly LetterViewModel _vm;

    public LetterPage(LetterViewModel vm)
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
