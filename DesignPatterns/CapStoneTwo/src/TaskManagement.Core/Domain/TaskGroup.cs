using TaskManagement.Core.Domain;

namespace TaskManagement.Core.Domain;

/// <summary>
/// Composite pattern — a TaskGroup is a node that can contain
/// individual TaskItems (leaves) or other TaskGroups (composites).
/// Implements uniform treatment via ITaskComponent.
/// </summary>
public interface ITaskComponent
{
    string Name { get; }
    void Display(int indent = 0);
    int TotalCount();
    int DoneCount();
}

/// <summary>Leaf node in the Composite tree.</summary>
public class TaskLeaf : ITaskComponent
{
    public string Name => _task.Title;
    private readonly TaskItem _task;

    public TaskLeaf(TaskItem task) => _task = task;

    public void Display(int indent = 0)
    {
        Console.WriteLine($"{new string(' ', indent * 2)}- {_task}");
    }

    public int TotalCount() => 1;
    public int DoneCount() => _task.Status == TaskStatus.Done ? 1 : 0;
}

/// <summary>
/// Composite node — represents a Project or a Sprint that groups tasks.
/// </summary>
public class TaskGroup : ITaskComponent
{
    public string Name { get; }
    private readonly List<ITaskComponent> _children = new();

    public TaskGroup(string name) => Name = name;

    public void Add(ITaskComponent component) => _children.Add(component);
    public void Remove(ITaskComponent component) => _children.Remove(component);
    public IReadOnlyList<ITaskComponent> Children => _children.AsReadOnly();

    public void Display(int indent = 0)
    {
        Console.WriteLine($"{new string(' ', indent * 2)}+ [{Name}]  ({DoneCount()}/{TotalCount()} done)");
        foreach (var child in _children)
            child.Display(indent + 1);
    }

    public int TotalCount() => _children.Sum(c => c.TotalCount());
    public int DoneCount()  => _children.Sum(c => c.DoneCount());
}
