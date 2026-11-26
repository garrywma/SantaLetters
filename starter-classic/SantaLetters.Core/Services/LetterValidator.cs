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

        // TODO (Kata 2): make the tests in LetterValidatorTests pass. Santa needs to know:
        //  - the child's name ("Please tell Santa your name.")
        //  - an age between MinAge and MaxAge
        //  - between 1 and MaxPresents presents ("Please choose at least one present."), with no duplicates
        //    ("You have picked the same present twice.")
        //  - extra wishes no longer than MaxWishesLength
        //  - every Nice List question answered ("Please answer all of the Nice List questions.")

        return errors;
    }
}
