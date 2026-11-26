namespace SantaLetters.Core.Models;

public sealed record Present(string Id, string Name, string Emoji, PresentCategory Category);

public enum PresentCategory
{
    Toys,
    Games,
    Outdoors,
    Creative,
    Books,
    Tech
}
