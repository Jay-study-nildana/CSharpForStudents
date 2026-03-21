using TaskManagement.Core.Domain;
using TaskManagement.Core.Interfaces;
using DomainTaskStatus = TaskManagement.Core.Domain.TaskStatus;

namespace TaskManagement.Core.Patterns.Structural;

// ─── Facade pattern ───────────────────────────────────────────────────────────
// WorkflowFacade provides a single, simple entry point that internally
// coordinates the task service, notifications, logging, and event bus.

public class WorkflowFacade
{
    private readonly ITaskService _taskService;
    private readonly INotificationSender _notifier;
    private readonly IAppLogger _logger;
    private readonly Behavioral.TaskEventBus _eventBus;

    public WorkflowFacade(
        ITaskService taskService,
        INotificationSender notifier,
        IAppLogger logger,
        Behavioral.TaskEventBus eventBus)
    {
        _taskService = taskService;
        _notifier    = notifier;
        _logger      = logger;
        _eventBus    = eventBus;
    }

    /// <summary>
    /// One-shot: create a task, notify assignee, publish event, log it.
    /// Callers don't need to know about any of the subsystems.
    /// </summary>
    public async Task<TaskItem> CreateAndAssignAsync(
        string title, string description,
        TaskPriority priority, string taskType,
        string assignee, DateTime? dueDate = null)
    {
        var task = await _taskService.CreateTaskAsync(title, description, priority, taskType, dueDate);
        _eventBus.PublishCreated(task);

        await _taskService.AssignTaskAsync(task.Id, assignee);
        _eventBus.PublishAssigned(task, assignee);

        _notifier.Send(assignee,
            $"New task assigned: {task.Title}",
            $"Type: {task.TaskType} | Priority: {task.Priority} | Due: {task.DueDate?.ToShortDateString() ?? "N/A"}");

        _logger.Log($"[Facade] Created+Assigned '{task.Title}' to {assignee}");
        return task;
    }
}

// ─── Bridge pattern ───────────────────────────────────────────────────────────
// ReportGenerator (Abstraction) is decoupled from IReportRenderer (Implementation).
// You can add new report types or new renderers independently.

/// <summary>
/// Abstraction — knows WHAT to report, delegates HOW to render to IReportRenderer.
/// </summary>
public class TaskReportGenerator
{
    private readonly IReportRenderer _renderer;   // Bridge to implementation

    public TaskReportGenerator(IReportRenderer renderer) => _renderer = renderer;

    public void GenerateStatusReport(IEnumerable<TaskItem> tasks)
    {
        var list = tasks.ToList();
        _renderer.RenderHeader("Task Status Report");
        foreach (var t in list)
            _renderer.RenderRow(t.ToString());
        _renderer.RenderFooter(
            $"Total: {list.Count}  Done: {list.Count(t => t.Status == DomainTaskStatus.Done)}");
    }
}

/// <summary>Implementation: renders to the console.</summary>
public class ConsoleReportRenderer : IReportRenderer
{
    public void RenderHeader(string title)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"\n{'═',50}");
        Console.WriteLine($"  {title}");
        Console.WriteLine($"{'═',50}");
        Console.ResetColor();
    }

    public void RenderRow(string line) => Console.WriteLine($"  {line}");

    public void RenderFooter(string summary)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine($"{'─',50}");
        Console.WriteLine($"  {summary}");
        Console.WriteLine($"{'═',50}\n");
        Console.ResetColor();
    }
}

/// <summary>Implementation: renders to CSV (written to console for demo).</summary>
public class CsvReportRenderer : IReportRenderer
{
    public void RenderHeader(string title) =>
        Console.WriteLine("Id,Title,Type,Priority,Status,AssignedTo,DueDate");

    public void RenderRow(string line) => Console.WriteLine(line.Replace("  ", ","));

    public void RenderFooter(string summary) => Console.WriteLine($"# {summary}");
}

// ─── Proxy pattern ────────────────────────────────────────────────────────────
// LazyTaskServiceProxy defers expensive initialization until first use.

public class LazyTaskServiceProxy : ITaskService
{
    private readonly Func<ITaskService> _factory;
    private ITaskService? _real;
    private ITaskService Real => _real ??= _factory();

    public LazyTaskServiceProxy(Func<ITaskService> factory) => _factory = factory;

    public Task<TaskItem> CreateTaskAsync(string title, string description,
        TaskPriority priority, string taskType, DateTime? dueDate = null) =>
        Real.CreateTaskAsync(title, description, priority, taskType, dueDate);

    public Task AssignTaskAsync(Guid taskId, string userName) =>
        Real.AssignTaskAsync(taskId, userName);

    public Task CompleteTaskAsync(Guid taskId) =>
        Real.CompleteTaskAsync(taskId);

    public Task<IEnumerable<TaskItem>> GetAllTasksAsync()    => Real.GetAllTasksAsync();
    public Task<IEnumerable<TaskItem>> GetSortedTasksAsync() => Real.GetSortedTasksAsync();
}
