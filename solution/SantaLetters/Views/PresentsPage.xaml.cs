using SantaLetters.ViewModels;

namespace SantaLetters.Views;

public partial class PresentsPage : ContentPage
{
    private readonly PresentsViewModel _vm;

    public PresentsPage(PresentsViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
        _vm.SleighFull += async (_, _) =>
        {
            await SleighBanner.ScaleToAsync(1.15, 120, Easing.CubicOut);
            await SleighBanner.ScaleToAsync(1.0, 200, Easing.BounceOut);
        };
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.Load();
    }
}
