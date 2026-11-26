using SantaLetters.ViewModels;

namespace SantaLetters.Views;

public partial class SummaryPage : ContentPage
{
    private readonly SummaryViewModel _vm;

    public SummaryPage(SummaryViewModel vm)
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
