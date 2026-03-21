using TaskManagement.Core.Domain;
using DomainTaskStatus = TaskManagement.Core.Domain.TaskStatus;

namespace TaskManagement.Core.Patterns.Creational;

/// <summary>
/// Builder pattern — constructs a TaskItem step-by-step with a fluent API.
/// Ensures the domain object is fully valid before being returned.
/// </summary>
public class TaskBuilder
{
    private Guid _id = Guid.NewGuid();
    private string _title = "Untitled";
    private string _description = string.Empty;
    private TaskPriority _priority = TaskPriority.Medium;
    private DomainTaskStatus _status = DomainTaskStatus.Todo;
    private string? _assignedTo;
    private DateTime _createdAt = DateTime.UtcNow;
    private DateTime? _dueDate;
    private string _taskType = "Feature";

    public TaskBuilder WithTitle(string title)        { _title = title;           return this; }
    public TaskBuilder WithDescription(string desc)   { _description = desc;      return this; }
    public TaskBuilder WithPriority(TaskPriority p)   { _priority = p;            return this; }
    public TaskBuilder WithStatus(DomainTaskStatus s)  { _status = s;            return this; }
    public TaskBuilder AssignedTo(string user)        { _assignedTo = user;       return this; }
    public TaskBuilder WithDueDate(DateTime d)        { _dueDate = d;             return this; }
    public TaskBuilder WithTaskType(string type)      { _taskType = type;         return this; }
    public TaskBuilder WithId(Guid id)                { _id = id;                 return this; }

    public TaskItem Build()
    {
        if (string.IsNullOrWhiteSpace(_title))
            throw new InvalidOperationException("Task title cannot be empty.");

        return new TaskItem(
            _id, _title, _description,
            _priority, _status, _assignedTo,
            _createdAt, _dueDate, _taskType);
    }
}
