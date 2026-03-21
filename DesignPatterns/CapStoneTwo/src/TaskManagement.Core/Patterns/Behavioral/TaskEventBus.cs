using TaskManagement.Core.Domain;
using TaskManagement.Core.Interfaces;

namespace TaskManagement.Core.Patterns.Behavioral;

// ─── Observer pattern ─────────────────────────────────────────────────────────
// ITaskEventListener is defined in IInterfaces.cs.
// The EventBus is a simple in-process publish-subscribe hub.

public class TaskEventBus
{
    private readonly List<ITaskEventListener> _listeners = new();

    public void Subscribe(ITaskEventListener listener) => _listeners.Add(listener);
    public void Unsubscribe(ITaskEventListener listener) => _listeners.Remove(listener);

    public void PublishCreated(TaskItem task)
    {
        foreach (var l in _listeners) l.OnTaskCreated(task);
    }

    public void PublishAssigned(TaskItem task, string user)
    {
        foreach (var l in _listeners) l.OnTaskAssigned(task, user);
    }

    public void PublishCompleted(TaskItem task)
    {
        foreach (var l in _listeners) l.OnTaskCompleted(task);
    }
}

/// <summary>Observer: writes task events to the app log.</summary>
public class LoggingTaskEventListener : ITaskEventListener
{
    private readonly IAppLogger _logger;
    public LoggingTaskEventListener(IAppLogger logger) => _logger = logger;

    public void OnTaskCreated(TaskItem task)   => _logger.Log($"[Event] Created:   {task.Title}");
    public void OnTaskAssigned(TaskItem task, string user) => _logger.Log($"[Event] Assigned:  '{task.Title}' → {user}");
    public void OnTaskCompleted(TaskItem task) => _logger.Log($"[Event] Completed: {task.Title}");
}

/// <summary>Observer: shows a console notification on task events.</summary>
public class ConsoleAlertEventListener : ITaskEventListener
{
    public void OnTaskCreated(TaskItem task)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"  [Alert] New task created: {task.Title}");
        Console.ResetColor();
    }

    public void OnTaskAssigned(TaskItem task, string user)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"  [Alert] Task assigned to {user}: {task.Title}");
        Console.ResetColor();
    }

    public void OnTaskCompleted(TaskItem task)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"  [Alert] Task completed: {task.Title}");
        Console.ResetColor();
    }
}
