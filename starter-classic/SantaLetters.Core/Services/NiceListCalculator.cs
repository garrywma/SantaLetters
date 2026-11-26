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
        // TODO (Kata 1): make the tests in NiceListCalculatorTests pass.
        //  - A question is "nice" when the child's answer matches question.NiceAnswer.
        //  - Score = nice weight / total weight * 100, rounded to a whole number.
        //  - Don't divide by zero!
        throw new NotImplementedException("Mrs Claus says: the Nice List calculator needs writing!");
    }

    public static NiceVerdict VerdictFor(int score)
    {
        // TODO (Kata 1): use the thresholds above to pick a verdict.
        throw new NotImplementedException();
    }
}
