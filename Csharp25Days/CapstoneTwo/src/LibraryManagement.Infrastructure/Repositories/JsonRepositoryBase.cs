using System.Text.Json;
using System.Text.Json.Serialization;
using LibraryManagement.Core.Interfaces;

namespace LibraryManagement.Infrastructure.Repositories;

/// <summary>
/// Generic JSON file-backed repository base class.
///
/// Curriculum topics demonstrated:
///   Day 12  – generics: one implementation works for any entity type T
///   Day 15  – async file I/O; atomic writes via temp-file rename
///   Day 25  – SemaphoreSlim for thread-safety (exclusive write access)
///   Day 26  – infrastructure layer separated from domain layer
/// </summary>
public abstract class JsonRepositoryBase<T> : IRepository<T> where T : class
{
    private readonly string         _filePath;
    private readonly SemaphoreSlim  _lock = new(1, 1);

    protected static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented          = true,
        PropertyNameCaseInsensitive = true,
        Converters             = { new JsonStringEnumConverter() }
    };

    protected JsonRepositoryBase(string filePath)
    {
        _filePath = filePath;
        // Ensure the data directory exists (fail-fast at startup, not at first use)
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
    }

    /// <summary>Derived classes expose the primary-key accessor so the base can compare entities.</summary>
    protected abstract Guid GetId(T entity);

    // ── IRepository<T> implementation ────────────────────────────────────────

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        await _lock.WaitAsync();
        try   { return await ReadDataAsync(); }
        finally { _lock.Release(); }
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        var all = await GetAllAsync();       // acquires and releases lock
        return all.FirstOrDefault(e => GetId(e) == id);
    }

    public async Task AddAsync(T entity)
    {
        await _lock.WaitAsync();
        try
        {
            var data = await ReadDataAsync();
            data.Add(entity);
            await WriteDataAsync(data);
        }
        finally { _lock.Release(); }
    }

    public async Task UpdateAsync(T entity)
    {
        await _lock.WaitAsync();
        try
        {
            var data  = await ReadDataAsync();
            var index = data.FindIndex(e => GetId(e) == GetId(entity));
            if (index < 0)
                throw new InvalidOperationException(
                    $"Entity {GetId(entity)} not found during update.");
            data[index] = entity;
            await WriteDataAsync(data);
        }
        finally { _lock.Release(); }
    }

    public async Task DeleteAsync(Guid id)
    {
        await _lock.WaitAsync();
        try
        {
            var data = await ReadDataAsync();
            data.RemoveAll(e => GetId(e) == id);
            await WriteDataAsync(data);
        }
        finally { _lock.Release(); }
    }

    // ── Private helpers (called only while _lock is held) ─────────────────────

    private async Task<List<T>> ReadDataAsync()
    {
        if (!File.Exists(_filePath))
            return new List<T>();

        var json = await File.ReadAllTextAsync(_filePath);
        return JsonSerializer.Deserialize<List<T>>(json, JsonOptions) ?? new List<T>();
    }

    /// <summary>
    /// Atomic write: serialise to a .tmp file then rename over the real file.
    /// Prevents data corruption if the process is killed mid-write (Day 15).
    /// </summary>
    private async Task WriteDataAsync(List<T> data)
    {
        var json     = JsonSerializer.Serialize(data, JsonOptions);
        var tempPath = _filePath + ".tmp";
        await File.WriteAllTextAsync(tempPath, json);
        File.Move(tempPath, _filePath, overwrite: true);
    }
}
