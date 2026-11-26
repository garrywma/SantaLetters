using SantaLetters.Core.Models;
using SantaLetters.Core.Services;

namespace SantaLetters.Tests;

public class LetterValidatorTests
{
    private static readonly NiceQuestion[] Questions = [new("kind", "Kind?", "💖", NiceAnswer: true)];
    private static readonly Present Teddy = new("teddy", "Teddy Bear", "🧸", PresentCategory.Toys);

    private static ChristmasLetter ValidLetter() => new()
    {
        ChildName = "Holly",
        Age = 7,
        Presents = [Teddy],
        Answers = new() { ["kind"] = true }
    };

    [Fact]
    public void Valid_letter_has_no_errors() =>
        Assert.Empty(LetterValidator.Validate(ValidLetter(), Questions));

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Name_is_required(string name)
    {
        var letter = ValidLetter();
        letter.ChildName = name;

        Assert.Contains("Please tell Santa your name.", LetterValidator.Validate(letter, Questions));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(17)]
    public void Age_must_be_in_range(int age)
    {
        var letter = ValidLetter();
        letter.Age = age;

        Assert.Single(LetterValidator.Validate(letter, Questions));
    }

    [Fact]
    public void At_least_one_present_is_required()
    {
        var letter = ValidLetter();
        letter.Presents.Clear();

        Assert.Contains("Please choose at least one present.", LetterValidator.Validate(letter, Questions));
    }

    [Fact]
    public void Too_many_presents_is_rejected()
    {
        var letter = ValidLetter();
        letter.Presents = Enumerable.Range(0, LetterValidator.MaxPresents + 1)
            .Select(i => new Present($"p{i}", $"Present {i}", "🎁", PresentCategory.Toys))
            .ToList();

        Assert.Single(LetterValidator.Validate(letter, Questions));
    }

    [Fact]
    public void Duplicate_presents_are_rejected()
    {
        var letter = ValidLetter();
        letter.Presents = [Teddy, Teddy];

        Assert.Contains("You have picked the same present twice.", LetterValidator.Validate(letter, Questions));
    }

    [Fact]
    public void Overlong_wishes_are_rejected()
    {
        var letter = ValidLetter();
        letter.ExtraWishes = new string('x', LetterValidator.MaxWishesLength + 1);

        Assert.Single(LetterValidator.Validate(letter, Questions));
    }

    [Fact]
    public void All_questions_must_be_answered()
    {
        var letter = ValidLetter();
        letter.Answers.Clear();

        Assert.Contains("Please answer all of the Nice List questions.", LetterValidator.Validate(letter, Questions));
    }
}
