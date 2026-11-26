using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SantaLetters.Core.Services;
using SantaLetters.Services;
using SantaLetters.Views;

namespace SantaLetters.ViewModels;

public partial class LetterViewModel(LetterDraft draft) : ObservableObject
{
    public int MaxLength => LetterValidator.MaxWishesLength;

    public string SignOff => $"Love from {draft.ChildName} ❤️";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CharactersLeftText))]
    public partial string ExtraWishes { get; set; } = string.Empty;

    public string CharactersLeftText => $"{MaxLength - (ExtraWishes?.Length ?? 0)} letters left";

    public void Load()
    {
        ExtraWishes = draft.ExtraWishes;
        OnPropertyChanged(nameof(SignOff));
    }

    [RelayCommand]
    private async Task NextAsync()
    {
        draft.ExtraWishes = ExtraWishes ?? string.Empty;
        await Shell.Current.GoToAsync(nameof(QuestionnairePage));
    }

    [RelayCommand]
    private Task BackAsync()
    {
        draft.ExtraWishes = ExtraWishes ?? string.Empty;
        return Shell.Current.GoToAsync("..");
    }
}
