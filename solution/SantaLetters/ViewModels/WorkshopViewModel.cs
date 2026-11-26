using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SantaLetters.Core.Models;
using SantaLetters.Core.Services;

namespace SantaLetters.ViewModels;

public sealed class LetterCard
{
    public LetterCard(ChristmasLetter letter, Func<LetterCard, Task> onDelete)
    {
        Letter = letter;
        DeleteCommand = new AsyncRelayCommand(() => onDelete(this));
    }

    public ChristmasLetter Letter { get; }
    public IAsyncRelayCommand DeleteCommand { get; }
    public string Title => $"{Letter.ChildName} (age {Letter.Age})";
    public string SentText => $"Sent {Letter.SentAt.LocalDateTime:ddd d MMM HH:mm}";
    public string PresentsText => string.Join("   ", Letter.Presents.Select(p => $"{p.Emoji} {p.Name}"));
    public string ExtraWishes => Letter.ExtraWishes;
    public bool HasExtraWishes => !string.IsNullOrWhiteSpace(Letter.ExtraWishes);
    public string VerdictText => $"{VerdictEmoji} {Letter.Verdict} ({Letter.NiceScore}%)";
    public bool IsNice => Letter.Verdict is NiceVerdict.Nice or NiceVerdict.SuperNice;

    private string VerdictEmoji => Letter.Verdict switch
    {
        NiceVerdict.SuperNice => "🌟",
        NiceVerdict.Nice => "😇",
        NiceVerdict.AlmostNice => "🤔",
        _ => "😈"
    };
}

public sealed record ProductionItem(string Emoji, string Name, int Count);

public partial class WorkshopViewModel(ILetterStore store) : ObservableObject
{
    public ObservableCollection<LetterCard> Letters { get; } = [];
    public ObservableCollection<ProductionItem> Production { get; } = [];

    [ObservableProperty]
    public partial string StatsText { get; set; } = string.Empty;

    public async Task LoadAsync()
    {
        var letters = await store.GetAllAsync();

        Letters.Clear();
        foreach (var letter in letters)
            Letters.Add(new LetterCard(letter, DeleteAsync));

        // Only nice children's presents go on the elves' production list.
        Production.Clear();
        foreach (var item in letters
                     .Where(l => l.Verdict is NiceVerdict.Nice or NiceVerdict.SuperNice)
                     .SelectMany(l => l.Presents)
                     .GroupBy(p => p.Id)
                     .Select(g => new ProductionItem(g.First().Emoji, g.First().Name, g.Count()))
                     .OrderByDescending(i => i.Count)
                     .ThenBy(i => i.Name))
        {
            Production.Add(item);
        }

        var nice = letters.Count(l => l.Verdict is NiceVerdict.Nice or NiceVerdict.SuperNice);
        StatsText = $"✉️ {letters.Count} {(letters.Count == 1 ? "letter" : "letters")}   😇 {nice} nice   😈 {letters.Count - nice} naughty";
    }

    private async Task DeleteAsync(LetterCard card)
    {
        var confirmed = await Shell.Current.DisplayAlertAsync(
            "Throw letter in the fire? 🔥",
            $"Delete {card.Letter.ChildName}'s letter?",
            "Delete",
            "Keep");

        if (!confirmed)
            return;

        await store.DeleteAsync(card.Letter.Id);
        await LoadAsync();
    }

    [RelayCommand]
    private Task BackAsync() => Shell.Current.GoToAsync("..");
}
