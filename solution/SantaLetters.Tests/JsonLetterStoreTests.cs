using SantaLetters.Core.Models;
using SantaLetters.Core.Services;

namespace SantaLetters.Tests;

public sealed class JsonLetterStoreTests : IDisposable
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), "santa-tests-" + Guid.NewGuid());

    public void Dispose()
    {
        if (Directory.Exists(_directory))
            Directory.Delete(_directory, recursive: true);
    }

    [Fact]
    public async Task Empty_store_returns_no_letters()
    {
        var store = new JsonLetterStore(_directory);

        Assert.Empty(await store.GetAllAsync());
    }

    [Fact]
    public async Task Saved_letters_survive_a_new_store_instance_newest_first()
    {
        var older = new ChristmasLetter { ChildName = "Holly", SentAt = DateTimeOffset.Now.AddHours(-1), Verdict = NiceVerdict.Nice };
        var newer = new ChristmasLetter
        {
            ChildName = "Noel",
            SentAt = DateTimeOffset.Now,
            Presents = [new("bike", "Bicycle", "🚲", PresentCategory.Outdoors)],
            Answers = new() { ["kind"] = true }
        };

        await new JsonLetterStore(_directory).SaveAsync(older);
        await new JsonLetterStore(_directory).SaveAsync(newer);

        var letters = await new JsonLetterStore(_directory).GetAllAsync();

        Assert.Equal(["Noel", "Holly"], letters.Select(l => l.ChildName));
        Assert.Equal("bike", letters[0].Presents.Single().Id);
        Assert.True(letters[0].Answers["kind"]);
        Assert.Equal(NiceVerdict.Nice, letters[1].Verdict);
    }

    [Fact]
    public async Task Saving_the_same_letter_twice_updates_it()
    {
        var store = new JsonLetterStore(_directory);
        var letter = new ChristmasLetter { ChildName = "Holly" };
        await store.SaveAsync(letter);

        letter.ChildName = "Holly Berry";
        await store.SaveAsync(letter);

        Assert.Equal("Holly Berry", (await store.GetAllAsync()).Single().ChildName);
    }

    [Fact]
    public async Task Delete_removes_only_that_letter()
    {
        var store = new JsonLetterStore(_directory);
        var keep = new ChristmasLetter { ChildName = "Keep" };
        var remove = new ChristmasLetter { ChildName = "Remove" };
        await store.SaveAsync(keep);
        await store.SaveAsync(remove);

        await store.DeleteAsync(remove.Id);

        Assert.Equal(keep.Id, (await store.GetAllAsync()).Single().Id);
    }
}
