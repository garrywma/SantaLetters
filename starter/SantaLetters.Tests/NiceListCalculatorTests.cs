using SantaLetters.Core.Models;
using SantaLetters.Core.Services;

namespace SantaLetters.Tests;

public class NiceListCalculatorTests
{
    private static readonly NiceQuestion[] Questions =
    [
        new("kind", "Kind?", "💖", NiceAnswer: true, Weight: 2),
        new("tidy", "Tidy?", "🧹", NiceAnswer: true),
        new("fib", "Fibs?", "🤥", NiceAnswer: false),
    ];

    [Fact]
    public void All_nice_answers_score_100_and_super_nice()
    {
        var result = NiceListCalculator.Calculate(Questions, new Dictionary<string, bool>
        {
            ["kind"] = true, ["tidy"] = true, ["fib"] = false
        });

        Assert.Equal(100, result.Score);
        Assert.Equal(NiceVerdict.SuperNice, result.Verdict);
    }

    [Fact]
    public void Naughty_phrased_question_is_nice_when_answered_no()
    {
        var result = NiceListCalculator.Calculate(Questions, new Dictionary<string, bool>
        {
            ["kind"] = false, ["tidy"] = false, ["fib"] = false
        });

        Assert.Equal(25, result.Score);
    }

    [Fact]
    public void Weights_are_applied()
    {
        var result = NiceListCalculator.Calculate(Questions, new Dictionary<string, bool>
        {
            ["kind"] = true, ["tidy"] = false, ["fib"] = true
        });

        Assert.Equal(50, result.Score);
    }

    [Fact]
    public void Unanswered_questions_count_as_not_nice()
    {
        var result = NiceListCalculator.Calculate(Questions, new Dictionary<string, bool> { ["tidy"] = true });

        Assert.Equal(25, result.Score);
        Assert.Equal(NiceVerdict.Naughty, result.Verdict);
    }

    [Fact]
    public void No_questions_is_naughty_rather_than_divide_by_zero()
    {
        var result = NiceListCalculator.Calculate([], new Dictionary<string, bool>());

        Assert.Equal(NiceVerdict.Naughty, result.Verdict);
    }

    [Theory]
    [InlineData(100, NiceVerdict.SuperNice)]
    [InlineData(85, NiceVerdict.SuperNice)]
    [InlineData(84, NiceVerdict.Nice)]
    [InlineData(60, NiceVerdict.Nice)]
    [InlineData(59, NiceVerdict.AlmostNice)]
    [InlineData(35, NiceVerdict.AlmostNice)]
    [InlineData(34, NiceVerdict.Naughty)]
    [InlineData(0, NiceVerdict.Naughty)]
    public void Verdict_boundaries(int score, NiceVerdict expected) =>
        Assert.Equal(expected, NiceListCalculator.VerdictFor(score));
}
