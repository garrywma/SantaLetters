using SantaLetters.Core.Models;

namespace SantaLetters.Core.Services;

public static class LetterValidator
{
    public const int MinAge = 1;
    public const int MaxAge = 16;
    public const int MaxPresents = 5;
    public const int MaxWishesLength = 500;

    public static IReadOnlyList<string> Validate(ChristmasLetter letter, IEnumerable<NiceQuestion> questions)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(letter.ChildName))
            errors.Add("Please tell Santa your name.");

        if (letter.Age is < MinAge or > MaxAge)
            errors.Add($"Your age must be between {MinAge} and {MaxAge}.");

        if (letter.Presents.Count == 0)
            errors.Add("Please choose at least one present.");
        else if (letter.Presents.Count > MaxPresents)
            errors.Add($"Santa's sleigh is full! Choose up to {MaxPresents} presents.");

        if (letter.Presents.Select(p => p.Id).Distinct().Count() != letter.Presents.Count)
            errors.Add("You have picked the same present twice.");

        if (letter.ExtraWishes.Length > MaxWishesLength)
            errors.Add($"Your letter is too long - keep it under {MaxWishesLength} letters.");

        if (questions.Any(q => !letter.Answers.ContainsKey(q.Id)))
            errors.Add("Please answer all of the Nice List questions.");

        return errors;
    }
}
