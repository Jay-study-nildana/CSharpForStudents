using TaskManagement.Core.Domain;
using TaskManagement.Core.Interfaces;

namespace TaskManagement.Core.Patterns.Behavioral;

// ─── Chain of Responsibility pattern ─────────────────────────────────────────
// Each handler validates one aspect of a TaskItem.
// A handler either short-circuits (returns error) or passes to the next handler.

public abstract class ValidationHandlerBase : IValidationHandler
{
    private IValidationHandler? _next;

    public IValidationHandler SetNext(IValidationHandler next)
    {
        _next = next;
        return next;  // Enables fluent chaining: a.SetNext(b).SetNext(c)
    }

    public (bool IsValid, string? Error) Handle(TaskItem task)
    {
        var result = Validate(task);
        if (!result.IsValid) return result;
        return _next?.Handle(task) ?? (true, null);
    }

    protected abstract (bool IsValid, string? Error) Validate(TaskItem task);
}

public class TitleValidationHandler : ValidationHandlerBase
{
    protected override (bool IsValid, string? Error) Validate(TaskItem task)
    {
        if (string.IsNullOrWhiteSpace(task.Title))
            return (false, "Task title cannot be empty.");
        if (task.Title.Length > 120)
            return (false, "Task title exceeds 120 characters.");
        return (true, null);
    }
}

public class DueDateValidationHandler : ValidationHandlerBase
{
    protected override (bool IsValid, string? Error) Validate(TaskItem task)
    {
        if (task.DueDate.HasValue && task.DueDate.Value < task.CreatedAt.AddMinutes(-1))
            return (false, "Due date cannot be in the past relative to creation time.");
        return (true, null);
    }
}

public class PriorityConsistencyHandler : ValidationHandlerBase
{
    protected override (bool IsValid, string? Error) Validate(TaskItem task)
    {
        // Critical priority bugs must have a due date
        if (task.Priority == TaskPriority.Critical &&
            task.TaskType == "Bug" &&
            task.DueDate is null)
            return (false, "Critical bugs must have a due date set.");
        return (true, null);
    }
}

/// <summary>Builds and returns the default validation chain.</summary>
public static class ValidationChainBuilder
{
    public static IValidationHandler Build()
    {
        var title    = new TitleValidationHandler();
        var dueDate  = new DueDateValidationHandler();
        var priority = new PriorityConsistencyHandler();

        title.SetNext(dueDate).SetNext(priority);
        return title;   // Return the head of the chain
    }
}
