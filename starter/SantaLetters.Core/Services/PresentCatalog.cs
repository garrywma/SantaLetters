using SantaLetters.Core.Models;

namespace SantaLetters.Core.Services;

public interface IPresentCatalog
{
    IReadOnlyList<Present> All { get; }
    Present? Find(string id);
}

// Portal devices run Android 9/10, so stick to emoji from Emoji 12.0 or earlier.
public sealed class PresentCatalog : IPresentCatalog
{
    public IReadOnlyList<Present> All { get; } =
    [
        new("teddy", "Teddy Bear", "🧸", PresentCategory.Toys),
        new("yoyo", "Yo-Yo", "🪀", PresentCategory.Toys),
        new("robot", "Robot", "🤖", PresentCategory.Toys),
        new("train", "Train Set", "🚂", PresentCategory.Toys),
        new("racecar", "Racing Car", "🏎️", PresentCategory.Toys),
        new("dino", "Dinosaur", "🦖", PresentCategory.Toys),
        new("babydoll", "Baby Doll", "👶", PresentCategory.Toys),
        new("dollshouse", "Dolls' House", "🏡", PresentCategory.Toys),
        new("unicorn", "Unicorn", "🦄", PresentCategory.Toys),
        new("puzzle", "Jigsaw Puzzle", "🧩", PresentCategory.Games),
        new("boardgame", "Board Game", "🎲", PresentCategory.Games),
        new("bike", "Bicycle", "🚲", PresentCategory.Outdoors),
        new("football", "Football", "⚽", PresentCategory.Outdoors),
        new("scooter", "Scooter", "🛴", PresentCategory.Outdoors),
        new("kite", "Kite", "🪁", PresentCategory.Outdoors),
        new("paints", "Paint Set", "🎨", PresentCategory.Creative),
        new("guitar", "Guitar", "🎸", PresentCategory.Creative),
        new("bricks", "Building Bricks", "🧱", PresentCategory.Creative),
        new("fairywings", "Fairy Wings", "🧚", PresentCategory.Creative),
        new("ballet", "Ballet Shoes", "🩰", PresentCategory.Creative),
        new("jewellery", "Jewellery Kit", "💍", PresentCategory.Creative),
        new("storybook", "Story Book", "📚", PresentCategory.Books),
        new("telescope", "Telescope", "🔭", PresentCategory.Tech),
        new("console", "Games Console", "🎮", PresentCategory.Tech),
    ];

    public Present? Find(string id) => All.FirstOrDefault(p => p.Id == id);
}
