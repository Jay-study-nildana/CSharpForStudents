using TaskManagement.Core.Domain;
using TaskManagement.Core.Interfaces;
using DomainTaskStatus = TaskManagement.Core.Domain.TaskStatus;

namespace TaskManagement.Core.Patterns.Behavioral;

// ─── Template Method pattern ──────────────────────────────────────────────────
// TaskProcessor defines an invariant algorithm skeleton.
// Subclasses override specific steps without changing the overall flow.

public abstract class TaskProcessor
{
    // Template method — the invariant algorithm skeleton
    public async Task ProcessAsync(TaskItem task)
    {
        if (!ValidateTask(task))
        {
            OnValidationFailed(task);
            return;
        }

        await PreProcessAsync(task);
        await DoWorkAsync(task);
        await PostProcessAsync(task);
        LogCompletion(task);
    }

    // Overridable steps
    protected virtual bool ValidateTask(TaskItem task) => task.Status != DomainTaskStatus.Cancelled;
    protected virtual void OnValidationFailed(TaskItem task) =>
        Console.WriteLine($"  [Processor] Task '{task.Title}' skipped (validation failed).");
    protected virtual Task PreProcessAsync(TaskItem task) => Task.CompletedTask;
    protected abstract Task DoWorkAsync(TaskItem task);
    protected virtual Task PostProcessAsync(TaskItem task) => Task.CompletedTask;
    protected virtual void LogCompletion(TaskItem task) =>
        Console.WriteLine($"  [Processor] Completed processing '{task.Title}'.");
}

/// <summary>Processes a Bug task — enforces high-priority logging.</summary>
public class BugTaskProcessor : TaskProcessor
{
    private readonly IAppLogger _logger;
    public BugTaskProcessor(IAppLogger logger) => _logger = logger;

    protected override Task PreProcessAsync(TaskItem task)
    {
        _logger.Log($"[BugProcessor] Starting bug triage for: {task.Title}");
        return Task.CompletedTask;
    }

    protected override Task DoWorkAsync(TaskItem task)
    {
        Console.WriteLine($"  [BugProcessor] Triaging bug: {task.Title}");
        task.UpdateStatus(DomainTaskStatus.InProgress);
        return Task.CompletedTask;
    }
}

/// <summary>Processes a Feature task — notifies stakeholders.</summary>
public class FeatureTaskProcessor : TaskProcessor
{
    private readonly IAppLogger _logger;
    public FeatureTaskProcessor(IAppLogger logger) => _logger = logger;

    protected override Task DoWorkAsync(TaskItem task)
    {
        Console.WriteLine($"  [FeatureProcessor] Scheduling feature: {task.Title}");
        task.UpdateStatus(DomainTaskStatus.InProgress);
        return Task.CompletedTask;
    }

    protected override Task PostProcessAsync(TaskItem task)
    {
        _logger.Log($"[FeatureProcessor] Feature '{task.Title}' moved to InProgress.");
        return Task.CompletedTask;
    }
}
