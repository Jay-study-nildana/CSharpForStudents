using TaskManagement.Core.Domain;
using TaskManagement.Core.Interfaces;

namespace TaskManagement.Core.Patterns.Structural;

// ─── Decorator pattern ────────────────────────────────────────────────────────
// Wraps ITaskService to add cross-cutting concerns without touching core logic.
// Decorators are stackable: Validation → Logging → Core service.

/// <summary>
/// Logging decorator — records every service call to the app logger.
/// </summary>
public class LoggingTaskServiceDecorator : ITaskService
{
    private readonly ITaskService _inner;
    private readonly IAppLogger   _logger;

    public LoggingTaskServiceDecorator(ITaskService inner, IAppLogger logger)
    {
        _inner  = inner;
        _logger = logger;
    }

    public async Task<TaskItem> CreateTaskAsync(string title, string description,
        TaskPriority priority, string taskType, DateTime? dueDate = null)
    {
        _logger.Log($"[LogDec] CreateTask called: '{title}' type={taskType}");
        var task = await _inner.CreateTaskAsync(title, description, priority, taskType, dueDate);
        _logger.Log($"[LogDec] CreateTask done: id={task.Id}");
        return task;
    }

    public async Task AssignTaskAsync(Guid taskId, string userName)
    {
        _logger.Log($"[LogDec] AssignTask {taskId} → {userName}");
        await _inner.AssignTaskAsync(taskId, userName);
    }

    public async Task CompleteTaskAsync(Guid taskId)
    {
        _logger.Log($"[LogDec] CompleteTask {taskId}");
        await _inner.CompleteTaskAsync(taskId);
    }

    public Task<IEnumerable<TaskItem>> GetAllTasksAsync()   => _inner.GetAllTasksAsync();
    public Task<IEnumerable<TaskItem>> GetSortedTasksAsync() => _inner.GetSortedTasksAsync();
}

/// <summary>
/// Validation decorator — runs the CoR validation chain before create/update.
/// </summary>
public class ValidationTaskServiceDecorator : ITaskService
{
    private readonly ITaskService _inner;
    private readonly IValidationHandler _validator;

    public ValidationTaskServiceDecorator(ITaskService inner, IValidationHandler validator)
    {
        _inner     = inner;
        _validator = validator;
    }

    public async Task<TaskItem> CreateTaskAsync(string title, string description,
        TaskPriority priority, string taskType, DateTime? dueDate = null)
    {
        // Build a probe task to validate before actually persisting
        var probe = new Patterns.Creational.TaskBuilder()
            .WithTitle(title)
            .WithDescription(description)
            .WithPriority(priority)
            .WithTaskType(taskType)
            .WithDueDate(dueDate ?? DateTime.UtcNow.AddDays(7))
            .Build();

        var (isValid, error) = _validator.Handle(probe);
        if (!isValid)
            throw new InvalidOperationException($"Validation failed: {error}");

        return await _inner.CreateTaskAsync(title, description, priority, taskType, dueDate);
    }

    public Task AssignTaskAsync(Guid taskId, string userName) => _inner.AssignTaskAsync(taskId, userName);
    public Task CompleteTaskAsync(Guid taskId)                => _inner.CompleteTaskAsync(taskId);
    public Task<IEnumerable<TaskItem>> GetAllTasksAsync()    => _inner.GetAllTasksAsync();
    public Task<IEnumerable<TaskItem>> GetSortedTasksAsync() => _inner.GetSortedTasksAsync();
}
