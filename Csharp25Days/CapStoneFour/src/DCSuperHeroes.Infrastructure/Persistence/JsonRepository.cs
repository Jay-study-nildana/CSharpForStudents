using System.Text.Json;
using System.Text.Json.Serialization;
using DCSuperHeroes.Core.Common;
using DCSuperHeroes.Core.Interfaces;

namespace DCSuperHeroes.Infrastructure.Persistence;

public class JsonRepository<T> : IRepository<T> where T : BaseEntity
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly string _filePath;
    private readonly SemaphoreSlim _gate = new(1, 1);

    public JsonRepository(string dataDirectory, string fileName)
    {
        Directory.CreateDirectory(dataDirectory);
        _filePath = Path.Combine(dataDirectory, fileName);

        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await _gate.WaitAsync(cancellationToken);
        try
        {
            return await LoadInternalAsync(cancellationToken);
        }
        finally
        {
            _gate.Release();
        }
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var items = await GetAllAsync(cancellationToken);
        return items.FirstOrDefault(item => item.Id == id);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _gate.WaitAsync(cancellationToken);
        try
        {
            var items = await LoadInternalAsync(cancellationToken);
            items.Add(entity);
            await SaveInternalAsync(items, cancellationToken);
        }
        finally
        {
            _gate.Release();
        }
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _gate.WaitAsync(cancellationToken);
        try
        {
            var items = await LoadInternalAsync(cancellationToken);
            var index = items.FindIndex(item => item.Id == entity.Id);
            if (index < 0)
            {
                throw new InvalidOperationException($"Entity {entity.Id} does not exist.");
            }

            items[index] = entity;
            await SaveInternalAsync(items, cancellationToken);
        }
        finally
        {
            _gate.Release();
        }
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _gate.WaitAsync(cancellationToken);
        try
        {
            var items = await LoadInternalAsync(cancellationToken);
            var removed = items.RemoveAll(item => item.Id == id);
            if (removed == 0)
            {
                throw new InvalidOperationException($"Entity {id} does not exist.");
            }

            await SaveInternalAsync(items, cancellationToken);
        }
        finally
        {
            _gate.Release();
        }
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        var items = await GetAllAsync(cancellationToken);
        return items.Count;
    }

    protected async Task<List<T>> LoadUnsafeAsync(CancellationToken cancellationToken = default) =>
        await LoadInternalAsync(cancellationToken);

    protected async Task SaveUnsafeAsync(List<T> items, CancellationToken cancellationToken = default) =>
        await SaveInternalAsync(items, cancellationToken);

    private async Task<List<T>> LoadInternalAsync(CancellationToken cancellationToken)
    {
        await using var stream = File.OpenRead(_filePath);
        var items = await JsonSerializer.DeserializeAsync<List<T>>(stream, SerializerOptions, cancellationToken);
        return items ?? [];
    }

    private async Task SaveInternalAsync(List<T> items, CancellationToken cancellationToken)
    {
        await using var stream = File.Create(_filePath);
        await JsonSerializer.SerializeAsync(stream, items, SerializerOptions, cancellationToken);
    }
}