using TaskManagement.Core.Domain;
using TaskManagement.Core.Patterns.Creational;

namespace TaskManagement.Core.Patterns.Creational;

/// <summary>
/// Factory Method pattern — defines the interface for creating a task.
/// Each concrete factory knows how to create a specific task type with
/// sensible defaults, shielding callers from construction details.
/// </summary>
public abstract class TaskFactory
{
    // Factory method — subclasses override to produce the right type
    public abstract TaskItem CreateTask(string title, string description,
        TaskPriority priority, DateTime? dueDate = null);

    // Common post-creation hook
    protected virtual void OnTaskCreated(TaskItem task) { }
}

public class BugTaskFactory : TaskFactory
{
    public override TaskItem CreateTask(string title, string description,
        TaskPriority priority, DateTime? dueDate = null)
    {
        var task = new TaskBuilder()
            .WithTitle(title)
            .WithDescription(description)
            .WithPriority(priority)
            .WithTaskType("Bug")
            .WithDueDate(dueDate ?? DateTime.UtcNow.AddDays(2))
            .Build();
        OnTaskCreated(task);
        return task;
    }
}

public class FeatureTaskFactory : TaskFactory
{
    public override TaskItem CreateTask(string title, string description,
        TaskPriority priority, DateTime? dueDate = null)
    {
        var task = new TaskBuilder()
            .WithTitle(title)
            .WithDescription(description)
            .WithPriority(priority)
            .WithTaskType("Feature")
            .WithDueDate(dueDate ?? DateTime.UtcNow.AddDays(14))
            .Build();
        OnTaskCreated(task);
        return task;
    }
}

public class ChoreTaskFactory : TaskFactory
{
    public override TaskItem CreateTask(string title, string description,
        TaskPriority priority, DateTime? dueDate = null)
    {
        var task = new TaskBuilder()
            .WithTitle(title)
            .WithDescription(description)
            .WithPriority(priority)
            .WithTaskType("Chore")
            .Build();
        OnTaskCreated(task);
        return task;
    }
}

/// <summary>
/// Resolves the right TaskFactory by task type string — acts as a registry.
/// </summary>
public static class TaskFactoryResolver
{
    private static readonly Dictionary<string, TaskFactory> _factories = new()
    {
        ["bug"]     = new BugTaskFactory(),
        ["feature"] = new FeatureTaskFactory(),
        ["chore"]   = new ChoreTaskFactory()
    };

    public static TaskFactory Resolve(string taskType)
    {
        var key = taskType.ToLowerInvariant();
        return _factories.TryGetValue(key, out var factory)
            ? factory
            : throw new ArgumentException($"Unknown task type: '{taskType}'");
    }

    public static IEnumerable<string> SupportedTypes => _factories.Keys;
}
