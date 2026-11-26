using SantaLetters.Core.Models;

namespace SantaLetters.Core.Services;

public interface ILetterStore
{
    Task SaveAsync(ChristmasLetter letter);
    Task<IReadOnlyList<ChristmasLetter>> GetAllAsync();
    Task DeleteAsync(Guid id);
}

/// <summary>
/// Stores letters on the device so they survive the app being closed.
/// </summary>
public sealed class JsonLetterStore(string directory) : ILetterStore
{
    // TODO (Kata 3): make the tests in JsonLetterStoreTests pass.
    //  - Keep every letter in one JSON file inside `directory` (System.Text.Json is built in).
    //  - Saving a letter with an existing Id replaces it.
    //  - GetAllAsync returns the newest letters first.
    //  - In the app, `directory` is FileSystem.AppDataDirectory.
    private readonly string _path = Path.Combine(directory, "letters-to-santa.json");

    public Task SaveAsync(ChristmasLetter letter) => throw new NotImplementedException();

    public Task<IReadOnlyList<ChristmasLetter>> GetAllAsync() => throw new NotImplementedException();

    public Task DeleteAsync(Guid id) => throw new NotImplementedException();
}
