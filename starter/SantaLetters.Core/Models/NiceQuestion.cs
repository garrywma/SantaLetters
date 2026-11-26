namespace SantaLetters.Core.Models;

/// <param name="NiceAnswer">The answer a nice child would give (some questions are phrased "naughtily").</param>
/// <param name="Weight">How much the question counts towards the nice score.</param>
public sealed record NiceQuestion(string Id, string Text, string Emoji, bool NiceAnswer, int Weight = 1);
