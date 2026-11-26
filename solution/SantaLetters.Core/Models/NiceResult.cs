namespace SantaLetters.Core.Models;

public enum NiceVerdict
{
    Naughty,
    AlmostNice,
    Nice,
    SuperNice
}

public sealed record NiceResult(int Score, NiceVerdict Verdict)
{
    public string ChildMessage => Verdict switch
    {
        NiceVerdict.SuperNice => "Wow! You're at the very top of Santa's Nice List! 🌟",
        NiceVerdict.Nice => "Hooray! You're on Santa's Nice List! 🎉",
        NiceVerdict.AlmostNice => "Nearly there! Be extra kind before Christmas Eve! 🦌",
        _ => "Uh oh! Santa's elves are watching... there's still time to be good! 🧝"
    };
}
