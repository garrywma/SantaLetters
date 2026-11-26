using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SantaLetters.Core.Services;
using SantaLetters.Services;
using SantaLetters.Views;

namespace SantaLetters.ViewModels;

public partial class WelcomeViewModel(LetterDraft draft) : ObservableObject
{
    [ObservableProperty]
    public partial string ChildName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial int Age { get; set; } = 6;

    [ObservableProperty]
    public partial string ErrorMessage { get; set; } = string.Empty;

    public void Load()
    {
        ChildName = draft.ChildName;
        Age = draft.Age;
        ErrorMessage = string.Empty;
    }

    [RelayCommand]
    private void IncreaseAge() => Age = Math.Min(LetterValidator.MaxAge, Age + 1);

    [RelayCommand]
    private void DecreaseAge() => Age = Math.Max(LetterValidator.MinAge, Age - 1);

    [RelayCommand]
    private async Task StartAsync()
    {
        if (string.IsNullOrWhiteSpace(ChildName))
        {
            ErrorMessage = "Please tell Santa your name! 🎅";
            return;
        }

        draft.ChildName = ChildName.Trim();
        draft.Age = Age;
        await Shell.Current.GoToAsync(nameof(PresentsPage));
    }

    [RelayCommand]
    private Task OpenWorkshopAsync() => Shell.Current.GoToAsync(nameof(WorkshopPage));
}
