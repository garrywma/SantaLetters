using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SantaLetters.Core.Models;
using SantaLetters.Core.Services;
using SantaLetters.Services;
using SantaLetters.Views;

namespace SantaLetters.ViewModels;

public partial class QuestionnaireViewModel(LetterDraft draft, IQuestionBank questionBank) : ObservableObject
{
    private IReadOnlyList<NiceQuestion> Questions => questionBank.Questions;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CurrentQuestion), nameof(ProgressText), nameof(Progress))]
    public partial int Index { get; set; }

    public NiceQuestion CurrentQuestion => Questions[Index];

    public string ProgressText => $"Question {Index + 1} of {Questions.Count}";

    public double Progress => (double)(Index + 1) / Questions.Count;

    public void Load() => Index = 0;

    [RelayCommand]
    private Task YesAsync() => AnswerAsync(true);

    [RelayCommand]
    private Task NoAsync() => AnswerAsync(false);

    private async Task AnswerAsync(bool answer)
    {
        draft.Answers[CurrentQuestion.Id] = answer;

        if (Index < Questions.Count - 1)
            Index++;
        else
            await Shell.Current.GoToAsync(nameof(SummaryPage));
    }

    [RelayCommand]
    private async Task BackAsync()
    {
        if (Index > 0)
            Index--;
        else
            await Shell.Current.GoToAsync("..");
    }
}
