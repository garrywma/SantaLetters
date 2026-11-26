using SantaLetters.Controls;
using SantaLetters.ViewModels;

namespace SantaLetters.Views;

public partial class WorkshopPage : StoryPage
{
    private readonly WorkshopViewModel _vm;

    public WorkshopPage(WorkshopViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadAsync();
    }
}
