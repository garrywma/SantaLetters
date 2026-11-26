using SantaLetters.Core.Models;

namespace SantaLetters.Core.Services;

public static class NiceListCalculator
{
    public const int SuperNiceThreshold = 85;
    public const int NiceThreshold = 60;
    public const int AlmostNiceThreshold = 35;

    /// <summary>
    /// Scores the answers from 0-100 by the weighted share of "nice" answers.
    /// Unanswered questions count as not nice.
    /// </summary>
    public static NiceResult Calculate(IEnumerable<NiceQuestion> questions, IReadOnlyDictionary<string, bool> answers)
    {
        var list = questions.ToList();
        var totalWeight = list.Sum(q => q.Weight);
        if (totalWeight <= 0)
        {
            return new NiceResult(0, NiceVerdict.Naughty);
        }

        var niceWeight = list
            .Where(q => answers.TryGetValue(q.Id, out var answer) && answer == q.NiceAnswer)
            .Sum(q => q.Weight);

        var score = (int)Math.Round(niceWeight * 100.0 / totalWeight, MidpointRounding.AwayFromZero);
        return new NiceResult(score, VerdictFor(score));
    }

    public static NiceVerdict VerdictFor(int score) => score switch
    {
        >= SuperNiceThreshold => NiceVerdict.SuperNice,
        >= NiceThreshold => NiceVerdict.Nice,
        >= AlmostNiceThreshold => NiceVerdict.AlmostNice,
        _ => NiceVerdict.Naughty
    };
}
