using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SantaLetters.Core.Models;
using SantaLetters.Core.Services;
using SantaLetters.Services;
using SantaLetters.Views;

namespace SantaLetters.ViewModels;

public partial class PresentItemViewModel(Present present) : ObservableObject
{
    public Present Present { get; } = present;

    [ObservableProperty]
    public partial bool IsSelected { get; set; }
}

public partial class PresentsViewModel(LetterDraft draft, IPresentCatalog catalog) : ObservableObject
{
    public ObservableCollection<PresentItemViewModel> Presents { get; } = [];

    public string Greeting => $"What would you like for Christmas, {draft.ChildName}?";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CountText))]
    [NotifyCanExecuteChangedFor(nameof(NextCommand))]
    public partial int SelectedCount { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasStatusMessage))]
    public partial string StatusMessage { get; set; } = string.Empty;

    public bool HasStatusMessage => !string.IsNullOrEmpty(StatusMessage);

    public event EventHandler? SleighFull;

    public string CountText => $"🎁 {SelectedCount} of {LetterValidator.MaxPresents} presents chosen";

    // The CollectionView's single selection acts as a "tap": toggle the present, then clear the
    // selection so the same tile can be tapped again. Works for touch, mouse and accessibility.
    [ObservableProperty]
    public partial PresentItemViewModel? TappedPresent { get; set; }

    partial void OnTappedPresentChanged(PresentItemViewModel? value)
    {
        if (value is null)
        {
            return;
        }

        Toggle(value);
        MainThread.BeginInvokeOnMainThread(() => TappedPresent = null);
    }

    public void Load()
    {
        Presents.Clear();
        foreach (var present in catalog.All)
        {
            Presents.Add(new PresentItemViewModel(present)
            {
                IsSelected = draft.Presents.Any(p => p.Id == present.Id)
            });
        }
        SelectedCount = Presents.Count(p => p.IsSelected);
        StatusMessage = string.Empty;
        OnPropertyChanged(nameof(Greeting));
    }

    private void Toggle(PresentItemViewModel item)
    {
        if (!item.IsSelected && SelectedCount >= LetterValidator.MaxPresents)
        {
            StatusMessage = "🛷 Sleigh full! Untick one to swap.";
            SleighFull?.Invoke(this, EventArgs.Empty);
            return;
        }

        item.IsSelected = !item.IsSelected;
        SelectedCount = Presents.Count(p => p.IsSelected);
        StatusMessage = string.Empty;
    }

    private bool CanGoNext() => SelectedCount > 0;

    [RelayCommand(CanExecute = nameof(CanGoNext))]
    private async Task NextAsync()
    {
        draft.Presents.Clear();
        draft.Presents.AddRange(Presents.Where(p => p.IsSelected).Select(p => p.Present));
        await Shell.Current.GoToAsync(nameof(LetterPage));
    }

    [RelayCommand]
    private Task BackAsync() => Shell.Current.GoToAsync("..");
}
