using System.Text.Json;
using System.Text.Json.Serialization;
using TaskManagement.Core.Domain;
using TaskManagement.Core.Interfaces;
using TaskManagement.Core.Patterns.Creational;
using DomainTaskStatus = TaskManagement.Core.Domain.TaskStatus;

namespace TaskManagement.Infrastructure.Persistence;

// ─── Repository pattern ───────────────────────────────────────────────────────
// JSON file used as a simple persistent store — easily swappable for EF Core/DB.

/// <summary>
/// Serialisation-friendly DTO that mirrors TaskItem's internal state.
/// Avoids coupling the JSON schema to the domain object.
/// </summary>
internal record TaskDto(
    Guid Id, string Title, string Description,
    TaskPriority Priority, DomainTaskStatus Status,
    string? AssignedTo, DateTime CreatedAt,
    DateTime? DueDate, string TaskType);

public class JsonFileTaskRepository : ITaskRepository
{
    private readonly string _filePath;
    private Dictionary<Guid, TaskItem> _cache = new();
    private bool _loaded;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        Converters    = { new JsonStringEnumConverter() }
    };

    public JsonFileTaskRepository(string filePath)
    {
        _filePath = filePath;
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
    }

    private async Task EnsureLoadedAsync()
    {
        if (_loaded) return;
        if (File.Exists(_filePath))
        {
            var json = await File.ReadAllTextAsync(_filePath);
            var dtos = JsonSerializer.Deserialize<List<TaskDto>>(json, _jsonOptions) ?? new();
            _cache = dtos.ToDictionary(d => d.Id, d => DtoToTask(d));
        }
        _loaded = true;
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id)
    {
        await EnsureLoadedAsync();
        return _cache.TryGetValue(id, out var t) ? t : null;
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        await EnsureLoadedAsync();
        return _cache.Values.ToList();
    }

    public async Task AddAsync(TaskItem task)
    {
        await EnsureLoadedAsync();
        _cache[task.Id] = task;
    }

    public async Task UpdateAsync(TaskItem task)
    {
        await EnsureLoadedAsync();
        _cache[task.Id] = task;
    }

    public async Task DeleteAsync(Guid id)
    {
        await EnsureLoadedAsync();
        _cache.Remove(id);
    }

    internal async Task FlushAsync()
    {
        var dtos = _cache.Values.Select(TaskToDto).ToList();
        var json = JsonSerializer.Serialize(dtos, _jsonOptions);
        await File.WriteAllTextAsync(_filePath, json);
    }

    private static TaskItem DtoToTask(TaskDto d) =>
        new TaskBuilder()
            .WithId(d.Id)
            .WithTitle(d.Title)
            .WithDescription(d.Description)
            .WithPriority(d.Priority)
            .WithStatus(d.Status)
            .AssignedTo(d.AssignedTo ?? string.Empty)
            .WithDueDate(d.DueDate ?? DateTime.UtcNow.AddDays(7))
            .WithTaskType(d.TaskType)
            .Build();

    private static TaskDto TaskToDto(TaskItem t) =>
        new(t.Id, t.Title, t.Description, t.Priority, t.Status,
            t.AssignedTo, t.CreatedAt, t.DueDate, t.TaskType);
}
