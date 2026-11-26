using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SantaLetters.Core.Models;
using SantaLetters.Core.Services;
using SantaLetters.Services;

namespace SantaLetters.ViewModels;

public partial class SummaryViewModel(LetterDraft draft, IQuestionBank questionBank, ILetterStore store) : ObservableObject
{
    private NiceResult _result = new(0, NiceVerdict.Naughty);

    public string ChildName => draft.ChildName;
    public string AgeText => $"Age {draft.Age}";
    public IReadOnlyList<Present> Presents => [.. draft.Presents];
    public string ExtraWishes => string.IsNullOrWhiteSpace(draft.ExtraWishes) ? "(No extra wishes)" : draft.ExtraWishes;
    public string NiceMessage => _result.ChildMessage;

    [ObservableProperty]
    public partial string ErrorMessage { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SendCommand))]
    public partial bool IsSending { get; set; }

    [ObservableProperty]
    public partial bool IsSent { get; set; }

    public void Load()
    {
        _result = NiceListCalculator.Calculate(questionBank.Questions, draft.Answers);
        ErrorMessage = string.Empty;
        IsSent = false;
        OnPropertyChanged(string.Empty);
    }

    private bool CanSend() => !IsSending;

    [RelayCommand(CanExecute = nameof(CanSend))]
    private async Task SendAsync()
    {
        var letter = draft.ToLetter(_result, DateTimeOffset.Now);
        var errors = LetterValidator.Validate(letter, questionBank.Questions);
        if (errors.Count > 0)
        {
            ErrorMessage = string.Join(Environment.NewLine, errors);
            return;
        }

        IsSending = true;
        try
        {
            await store.SaveAsync(letter);
            IsSent = true;
            // Leave time for SummaryPage's seal-and-send animation (~3.5s) plus a moment to enjoy it.
            await Task.Delay(5000);
            draft.Reset();
            await Shell.Current.GoToAsync("//welcome");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Oh no, the reindeer dropped your letter! {ex.Message}";
        }
        finally
        {
            IsSending = false;
        }
    }

    [RelayCommand]
    private Task BackAsync() => Shell.Current.GoToAsync("..");
}
