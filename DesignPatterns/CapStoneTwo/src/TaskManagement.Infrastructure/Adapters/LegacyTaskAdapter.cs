using TaskManagement.Core.Domain;

namespace TaskManagement.Infrastructure.Adapters;

// ─── Adapter pattern ──────────────────────────────────────────────────────────
// The legacy system exports tasks as raw CSV lines.
// LegacyTaskAdapter converts those lines into domain TaskItem objects,
// bridging the incompatible interface without changing either side.

/// <summary>The legacy interface we cannot change.</summary>
public interface ILegacyCsvTaskSource
{
    IEnumerable<string> GetRawLines();
}

/// <summary>Sample legacy implementation (hard-coded CSV for demo).</summary>
public class HardCodedLegacyCsvSource : ILegacyCsvTaskSource
{
    public IEnumerable<string> GetRawLines() =>
    [
        "LEGACY-001|Fix login page crash|Bug|High|Alice",
        "LEGACY-002|Add dark mode support|Feature|Medium|Bob",
        "LEGACY-003|Update dependencies|Chore|Low|"
    ];
}

/// <summary>
/// Adapter: converts ILegacyCsvTaskSource lines into domain TaskItems.
/// </summary>
public class LegacyTaskAdapter
{
    private readonly ILegacyCsvTaskSource _source;

    public LegacyTaskAdapter(ILegacyCsvTaskSource source) => _source = source;

    public IEnumerable<TaskItem> ImportTasks()
    {
        foreach (var line in _source.GetRawLines())
        {
            var parts = line.Split('|');
            if (parts.Length < 5) continue;

            var priorityStr = parts[3].Trim();
            var priority = Enum.TryParse<TaskPriority>(priorityStr, true, out var p)
                ? p : TaskPriority.Medium;

            var task = new Core.Patterns.Creational.TaskBuilder()
                .WithTitle(parts[1].Trim())
                .WithDescription($"Imported from legacy: {parts[0].Trim()}")
                .WithPriority(priority)
                .WithTaskType(parts[2].Trim())
                .AssignedTo(parts[4].Trim())
                .WithDueDate(DateTime.UtcNow.AddDays(7))
                .Build();

            yield return task;
        }
    }
}
