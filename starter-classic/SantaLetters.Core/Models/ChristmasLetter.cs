namespace SantaLetters.Core.Models;

public sealed class ChristmasLetter
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string ChildName { get; set; } = string.Empty;
    public int Age { get; set; }
    public List<Present> Presents { get; set; } = [];
    public string ExtraWishes { get; set; } = string.Empty;
    public Dictionary<string, bool> Answers { get; set; } = [];
    public int NiceScore { get; set; }
    public NiceVerdict Verdict { get; set; }
    public DateTimeOffset SentAt { get; set; }
}
