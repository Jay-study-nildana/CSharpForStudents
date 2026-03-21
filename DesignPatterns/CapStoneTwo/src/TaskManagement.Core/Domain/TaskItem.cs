namespace TaskManagement.Core.Domain;

public enum TaskStatus
{
    Todo,
    InProgress,
    Done,
    Cancelled
}

public enum TaskPriority
{
    Low,
    Medium,
    High,
    Critical
}

/// <summary>
/// Core domain entity — immutable after construction via Builder or Factory.
/// </summary>
public class TaskItem
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public TaskPriority Priority { get; private set; }
    public TaskStatus Status { get; private set; }
    public string? AssignedTo { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? DueDate { get; private set; }
    public string TaskType { get; private set; }   // "Bug", "Feature", "Chore"

    // Used by Builder and JSON deserializer
#pragma warning disable CS8618
    private TaskItem() { }
#pragma warning restore CS8618

    internal TaskItem(
        Guid id, string title, string description,
        TaskPriority priority, TaskStatus status,
        string? assignedTo, DateTime createdAt,
        DateTime? dueDate, string taskType)
    {
        Id = id;
        Title = title;
        Description = description;
        Priority = priority;
        Status = status;
        AssignedTo = assignedTo;
        CreatedAt = createdAt;
        DueDate = dueDate;
        TaskType = taskType;
    }

    public void Assign(string user)
    {
        AssignedTo = user;
        if (Status == TaskStatus.Todo)
            Status = TaskStatus.InProgress;
    }

    public void Complete() => Status = TaskStatus.Done;
    public void Cancel()   => Status = TaskStatus.Cancelled;

    public void UpdateStatus(TaskStatus newStatus) => Status = newStatus;

    public override string ToString() =>
        $"[{Id.ToString()[..8]}] [{TaskType,-7}] [{Priority,-8}] [{Status,-11}] {Title}" +
        (AssignedTo is null ? "" : $"  → {AssignedTo}");
}
