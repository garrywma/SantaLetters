using SantaLetters.Core.Models;

namespace SantaLetters.Services;

/// <summary>The letter currently being written. Shared by every page in the flow.</summary>
public sealed class LetterDraft
{
    public string ChildName { get; set; } = string.Empty;
    public int Age { get; set; } = 6;
    public List<Present> Presents { get; } = [];
    public string ExtraWishes { get; set; } = string.Empty;
    public Dictionary<string, bool> Answers { get; } = [];

    public void Reset()
    {
        ChildName = string.Empty;
        Age = 6;
        Presents.Clear();
        ExtraWishes = string.Empty;
        Answers.Clear();
    }

    public ChristmasLetter ToLetter(NiceResult result, DateTimeOffset sentAt) => new()
    {
        ChildName = ChildName.Trim(),
        Age = Age,
        Presents = [.. Presents],
        ExtraWishes = ExtraWishes.Trim(),
        Answers = new(Answers),
        NiceScore = result.Score,
        Verdict = result.Verdict,
        SentAt = sentAt
    };
}
