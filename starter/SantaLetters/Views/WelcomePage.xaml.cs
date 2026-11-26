using SantaLetters.Controls;
using SantaLetters.ViewModels;

namespace SantaLetters.Views;

public partial class WelcomePage : StoryPage
{
    private readonly WelcomeViewModel _vm;

    public WelcomePage(WelcomeViewModel vm)
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
