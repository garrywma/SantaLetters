using SantaLetters.Core.Models;

namespace SantaLetters.Core.Services;

public interface IQuestionBank
{
    IReadOnlyList<NiceQuestion> Questions { get; }
}

public sealed class QuestionBank : IQuestionBank
{
    public IReadOnlyList<NiceQuestion> Questions { get; } =
    [
        new("tidy", "Have you tidied your bedroom when asked?", "🧹", NiceAnswer: true),
        new("share", "Have you shared your toys with others?", "🤝", NiceAnswer: true),
        new("veg", "Have you eaten your vegetables?", "🥦", NiceAnswer: true),
        new("bedtime", "Have you gone to bed on time?", "🛏️", NiceAnswer: true),
        new("kind", "Have you been kind to your family and friends?", "💖", NiceAnswer: true, Weight: 2),
        new("help", "Have you helped with jobs around the house?", "🧺", NiceAnswer: true),
        new("teeth", "Have you brushed your teeth every day?", "🦷", NiceAnswer: true),
        new("tantrum", "Have you had lots of tantrums this year?", "😤", NiceAnswer: false),
        new("fib", "Have you told any fibs?", "🤥", NiceAnswer: false, Weight: 2),
    ];
}
