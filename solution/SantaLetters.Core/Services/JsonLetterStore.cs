using System.Text.Json;
using System.Text.Json.Serialization;
using SantaLetters.Core.Models;

namespace SantaLetters.Core.Services;

public interface ILetterStore
{
    Task SaveAsync(ChristmasLetter letter);
    Task<IReadOnlyList<ChristmasLetter>> GetAllAsync();
    Task DeleteAsync(Guid id);
}

/// <summary>Stores all letters in a single JSON file inside the given directory.</summary>
public sealed class JsonLetterStore(string directory) : ILetterStore
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly string _path = Path.Combine(directory, "letters-to-santa.json");

    public async Task SaveAsync(ChristmasLetter letter)
    {
        await _lock.WaitAsync();
        try
        {
            var letters = await ReadAsync();
            letters.RemoveAll(l => l.Id == letter.Id);
            letters.Add(letter);
            await WriteAsync(letters);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<IReadOnlyList<ChristmasLetter>> GetAllAsync()
    {
        await _lock.WaitAsync();
        try
        {
            return (await ReadAsync()).OrderByDescending(l => l.SentAt).ToList();
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        await _lock.WaitAsync();
        try
        {
            var letters = await ReadAsync();
            if (letters.RemoveAll(l => l.Id == id) > 0)
                await WriteAsync(letters);
        }
        finally
        {
            _lock.Release();
        }
    }

    private async Task<List<ChristmasLetter>> ReadAsync()
    {
        if (!File.Exists(_path))
            return [];

        await using var stream = File.OpenRead(_path);
        return await JsonSerializer.DeserializeAsync<List<ChristmasLetter>>(stream, Options) ?? [];
    }

    private async Task WriteAsync(List<ChristmasLetter> letters)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
        var temp = _path + ".tmp";
        await using (var stream = File.Create(temp))
        {
            await JsonSerializer.SerializeAsync(stream, letters, Options);
        }
        File.Move(temp, _path, overwrite: true);
    }
}
