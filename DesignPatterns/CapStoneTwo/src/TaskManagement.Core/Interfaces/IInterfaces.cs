using TaskManagement.Core.Domain;

namespace TaskManagement.Core.Interfaces;

// ─── Repository interfaces (Repository + Unit of Work patterns) ──────────────

public interface ITaskRepository
{
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task<IEnumerable<TaskItem>> GetAllAsync();
    Task AddAsync(TaskItem task);
    Task UpdateAsync(TaskItem task);
    Task DeleteAsync(Guid id);
}

public interface IUnitOfWork : IDisposable
{
    ITaskRepository Tasks { get; }
    Task<int> SaveChangesAsync();
}

// ─── Service interfaces ───────────────────────────────────────────────────────

public interface ITaskService
{
    Task<TaskItem> CreateTaskAsync(string title, string description,
        TaskPriority priority, string taskType, DateTime? dueDate = null);
    Task AssignTaskAsync(Guid taskId, string userName);
    Task CompleteTaskAsync(Guid taskId);
    Task<IEnumerable<TaskItem>> GetAllTasksAsync();
    Task<IEnumerable<TaskItem>> GetSortedTasksAsync();
}

// ─── Logging abstraction (Singleton) ─────────────────────────────────────────

public interface IAppLogger
{
    void Log(string message);
    void LogError(string message);
    IReadOnlyList<string> GetHistory();
}

// ─── Notification abstraction (Abstract Factory) ─────────────────────────────

public interface INotificationSender
{
    void Send(string recipient, string subject, string body);
}

public interface INotificationProviderFactory
{
    INotificationSender CreateSender();
    string ProviderName { get; }
}

// ─── Report rendering (Bridge) ───────────────────────────────────────────────

public interface IReportRenderer
{
    void RenderHeader(string title);
    void RenderRow(string line);
    void RenderFooter(string summary);
}

// ─── Priority strategy (Strategy) ────────────────────────────────────────────

public interface IPriorityStrategy
{
    string Name { get; }
    IEnumerable<TaskItem> Sort(IEnumerable<TaskItem> tasks);
}

// ─── Task event observer (Observer) ─────────────────────────────────────────

public interface ITaskEventListener
{
    void OnTaskCreated(TaskItem task);
    void OnTaskAssigned(TaskItem task, string user);
    void OnTaskCompleted(TaskItem task);
}

// ─── Command (Command + undo) ────────────────────────────────────────────────

public interface ICommand
{
    string Description { get; }
    Task ExecuteAsync();
    Task UndoAsync();
}

// ─── Validation handler (Chain of Responsibility) ────────────────────────────

public interface IValidationHandler
{
    IValidationHandler SetNext(IValidationHandler next);
    (bool IsValid, string? Error) Handle(TaskItem task);
}
