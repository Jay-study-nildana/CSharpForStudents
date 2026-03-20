using System.Text.Json;
using System.Text.Json.Serialization;
using ComicBookShop.Core.Entities;
using ComicBookShop.Core.Interfaces;

namespace ComicBookShop.Infrastructure.Persistence;

/// <summary>
/// Generic JSON-file-backed repository with async I/O and thread safety.
/// Demonstrates generics (Day 12), async file I/O (Day 20), SemaphoreSlim (Day 25),
/// and the repository pattern (Day 26).
/// </summary>
public class JsonRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly JsonSerializerOptions _jsonOptions;
    private List<T> _items = new();
    private bool _isLoaded;

    public JsonRepository(string dataDirectory)
    {
        Directory.CreateDirectory(dataDirectory);
        var fileName = $"{typeof(T).Name.ToLowerInvariant()}s.json";
        _filePath = Path.Combine(dataDirectory, fileName);

        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };
    }

    // ── Private helpers ─────────────────────────────────────────────────

    private async Task EnsureLoadedAsync()
    {
        if (_isLoaded) return;

        if (File.Exists(_filePath))
        {
            var json = await File.ReadAllTextAsync(_filePath);
            _items = JsonSerializer.Deserialize<List<T>>(json, _jsonOptions) ?? new List<T>();
        }

        _isLoaded = true;
    }

    private async Task PersistAsync()
    {
        var json = JsonSerializer.Serialize(_items, _jsonOptions);
        await File.WriteAllTextAsync(_filePath, json);
    }

    // ── IRepository<T> implementation ───────────────────────────────────

    public async Task<T?> GetByIdAsync(Guid id)
    {
        await _lock.WaitAsync();
        try
        {
            await EnsureLoadedAsync();
            return _items.FirstOrDefault(x => x.Id == id);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        await _lock.WaitAsync();
        try
        {
            await EnsureLoadedAsync();
            return _items.AsReadOnly();
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task AddAsync(T entity)
    {
        await _lock.WaitAsync();
        try
        {
            await EnsureLoadedAsync();
            _items.Add(entity);
            await PersistAsync();
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task UpdateAsync(T entity)
    {
        await _lock.WaitAsync();
        try
        {
            await EnsureLoadedAsync();
            var index = _items.FindIndex(x => x.Id == entity.Id);
            if (index < 0)
                throw new InvalidOperationException($"Entity {typeof(T).Name} with Id {entity.Id} not found.");

            _items[index] = entity;
            await PersistAsync();
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
            await EnsureLoadedAsync();
            var removed = _items.RemoveAll(x => x.Id == id);
            if (removed == 0)
                throw new InvalidOperationException($"Entity {typeof(T).Name} with Id {id} not found.");

            await PersistAsync();
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<IReadOnlyList<T>> FindAsync(Func<T, bool> predicate)
    {
        await _lock.WaitAsync();
        try
        {
            await EnsureLoadedAsync();
            return _items.Where(predicate).ToList().AsReadOnly();
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<int> CountAsync()
    {
        await _lock.WaitAsync();
        try
        {
            await EnsureLoadedAsync();
            return _items.Count;
        }
        finally
        {
            _lock.Release();
        }
    }
}
