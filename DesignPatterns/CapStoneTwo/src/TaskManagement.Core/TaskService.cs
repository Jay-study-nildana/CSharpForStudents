using TaskManagement.Core.Domain;
using TaskManagement.Core.Interfaces;
using TaskManagement.Core.Patterns.Behavioral;
using TaskManagement.Core.Patterns.Creational;

namespace TaskManagement.Core;

/// <summary>
/// Core task service implementation — coordinates the repository via UoW
/// and uses the Strategy pattern to sort tasks.
/// Decorated by Logging and Validation decorators at composition root.
/// </summary>
public class TaskService : ITaskService
{
    private readonly IUnitOfWork _uow;
    private IPriorityStrategy _sortStrategy;

    public TaskService(IUnitOfWork uow, IPriorityStrategy sortStrategy)
    {
        _uow           = uow;
        _sortStrategy  = sortStrategy;
    }

    public void SetSortStrategy(IPriorityStrategy strategy) => _sortStrategy = strategy;

    public async Task<TaskItem> CreateTaskAsync(string title, string description,
        TaskPriority priority, string taskType, DateTime? dueDate = null)
    {
        var factory = TaskFactoryResolver.Resolve(taskType);
        var task    = factory.CreateTask(title, description, priority, dueDate);
        await _uow.Tasks.AddAsync(task);
        await _uow.SaveChangesAsync();
        return task;
    }

    public async Task AssignTaskAsync(Guid taskId, string userName)
    {
        var task = await _uow.Tasks.GetByIdAsync(taskId)
            ?? throw new KeyNotFoundException($"Task {taskId} not found.");
        task.Assign(userName);
        await _uow.Tasks.UpdateAsync(task);
        await _uow.SaveChangesAsync();
    }

    public async Task CompleteTaskAsync(Guid taskId)
    {
        var task = await _uow.Tasks.GetByIdAsync(taskId)
            ?? throw new KeyNotFoundException($"Task {taskId} not found.");
        task.Complete();
        await _uow.Tasks.UpdateAsync(task);
        await _uow.SaveChangesAsync();
    }

    public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        => await _uow.Tasks.GetAllAsync();

    public async Task<IEnumerable<TaskItem>> GetSortedTasksAsync()
    {
        var tasks = await _uow.Tasks.GetAllAsync();
        return _sortStrategy.Sort(tasks);
    }
}
