using TaskManagement.Core.Domain;
using TaskManagement.Core.Interfaces;

namespace TaskManagement.Core.Patterns.Behavioral;

// ─── Strategy pattern ────────────────────────────────────────────────────────
// IPriorityStrategy is defined in IInterfaces.cs.
// Each concrete strategy implements a different sorting algorithm.

public class PriorityDescendingStrategy : IPriorityStrategy
{
    public string Name => "Priority (High → Low)";
    public IEnumerable<TaskItem> Sort(IEnumerable<TaskItem> tasks) =>
        tasks.OrderByDescending(t => t.Priority);
}

public class DueDateAscendingStrategy : IPriorityStrategy
{
    public string Name => "Due Date (Soonest First)";
    public IEnumerable<TaskItem> Sort(IEnumerable<TaskItem> tasks) =>
        tasks.OrderBy(t => t.DueDate ?? DateTime.MaxValue);
}

public class StatusGroupingStrategy : IPriorityStrategy
{
    public string Name => "Status (Todo → InProgress → Done)";
    public IEnumerable<TaskItem> Sort(IEnumerable<TaskItem> tasks) =>
        tasks.OrderBy(t => t.Status).ThenByDescending(t => t.Priority);
}
