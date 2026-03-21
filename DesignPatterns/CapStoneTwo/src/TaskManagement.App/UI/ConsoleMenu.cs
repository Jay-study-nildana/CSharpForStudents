using TaskManagement.Core.Domain;
using TaskManagement.Core.Interfaces;
using TaskManagement.Core.Patterns.Behavioral;
using TaskManagement.Core.Patterns.Structural;

namespace TaskManagement.App.UI;

/// <summary>
/// Console menu shell — wires up user input to the WorkflowFacade, 
/// CommandHistory (undo/redo), Mediator, and Bridge-based report.
/// </summary>
public class ConsoleMenu
{
    private readonly ITaskService _taskService;
    private readonly WorkflowFacade _facade;
    private readonly CommandHistory _history;
    private readonly IWorkflowMediator _mediator;
    private readonly TaskReportGenerator _reportGenerator;
    private readonly IAppLogger _logger;
    private readonly IUnitOfWork _uow;

    public ConsoleMenu(
        ITaskService taskService,
        WorkflowFacade facade,
        CommandHistory history,
        IWorkflowMediator mediator,
        TaskReportGenerator reportGenerator,
        IAppLogger logger,
        IUnitOfWork uow)
    {
        _taskService     = taskService;
        _facade          = facade;
        _history         = history;
        _mediator        = mediator;
        _reportGenerator = reportGenerator;
        _logger          = logger;
        _uow             = uow;
    }

    public async Task RunAsync()
    {
        PrintBanner();
        while (true)
        {
            PrintMenu();
            var choice = Console.ReadLine()?.Trim();
            Console.WriteLine();

            switch (choice)
            {
                case "1": await CreateTaskAsync();        break;
                case "2": await ListTasksAsync();         break;
                case "3": await AssignTaskAsync();        break;
                case "4": await CompleteTaskAsync();      break;
                case "5": await UndoLastActionAsync();    break;
                case "6": await RedoLastActionAsync();    break;
                case "7": await ShowReportAsync();        break;
                case "8": await ShowSummaryAsync();       break;
                case "9": await ShowLogAsync();           break;
                case "0": Console.WriteLine("Goodbye!"); return;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("  Invalid option. Try again.");
                    Console.ResetColor();
                    break;
            }
        }
    }

    // ─── Menu actions ─────────────────────────────────────────────────────────

    private async Task CreateTaskAsync()
    {
        Console.WriteLine("─── Create New Task ───────────────────────");
        var title       = Prompt("Title");
        var description = Prompt("Description");
        var taskType    = PromptChoice("Type [bug/feature/chore]", "bug", "feature", "chore");
        var priority    = PromptEnum<TaskPriority>("Priority [Low/Medium/High/Critical]");
        var assignee    = Prompt("Assign to (leave blank to skip)");

        try
        {
            if (!string.IsNullOrWhiteSpace(assignee))
            {
                var task = await _facade.CreateAndAssignAsync(
                    title, description, priority, taskType, assignee);
                Console.WriteLine($"\n  Created and assigned: {task}");
            }
            else
            {
                var task = await _taskService.CreateTaskAsync(title, description, priority, taskType);
                await _mediator.NotifyAsync("TaskCreated", task);
                Console.WriteLine($"\n  Created: {task}");
            }
        }
        catch (InvalidOperationException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n  Error: {ex.Message}");
            Console.ResetColor();
        }
    }

    private async Task ListTasksAsync()
    {
        Console.WriteLine("─── All Tasks (sorted) ────────────────────");
        var tasks = (await _taskService.GetSortedTasksAsync()).ToList();
        if (tasks.Count == 0) { Console.WriteLine("  (no tasks yet)"); return; }
        foreach (var t in tasks) Console.WriteLine($"  {t}");
    }

    private async Task AssignTaskAsync()
    {
        Console.WriteLine("─── Assign Task ────────────────────────────");
        var id   = await PromptGuid("Task ID (first 8 chars OK)");
        var user = Prompt("Assign to");
        if (id is null) return;

        var task = await _uow.Tasks.GetByIdAsync(id.Value);
        if (task is null) { Console.WriteLine("  Task not found."); return; }

        var cmd = new AssignTaskCommand(task, user, _uow);
        await _history.ExecuteAsync(cmd);
        await _mediator.NotifyAsync("TaskAssigned", (task, user));
        Console.WriteLine($"  Assigned: {task}");
    }

    private async Task CompleteTaskAsync()
    {
        Console.WriteLine("─── Complete Task ──────────────────────────");
        var id = await PromptGuid("Task ID (first 8 chars OK)");
        if (id is null) return;

        var task = await _uow.Tasks.GetByIdAsync(id.Value);
        if (task is null) { Console.WriteLine("  Task not found."); return; }

        var cmd = new CompleteTaskCommand(task, _uow);
        await _history.ExecuteAsync(cmd);
        await _mediator.NotifyAsync("TaskCompleted", task);
        Console.WriteLine($"  Completed: {task}");
    }

    private async Task UndoLastActionAsync()
    {
        if (!_history.CanUndo) { Console.WriteLine("  Nothing to undo."); return; }
        await _history.UndoAsync();
    }

    private async Task RedoLastActionAsync()
    {
        if (!_history.CanRedo) { Console.WriteLine("  Nothing to redo."); return; }
        await _history.RedoAsync();
    }

    private async Task ShowReportAsync()
    {
        var tasks = await _taskService.GetSortedTasksAsync();
        _reportGenerator.GenerateStatusReport(tasks);
    }

    private async Task ShowSummaryAsync()
        => await _mediator.NotifyAsync("ShowSummary");

    private Task ShowLogAsync()
    {
        Console.WriteLine("─── Application Log ────────────────────────");
        var log = _logger.GetHistory();
        foreach (var line in log.TakeLast(20))
            Console.WriteLine($"  {line}");
        return Task.CompletedTask;
    }

    // ─── Input helpers ────────────────────────────────────────────────────────

    private static string Prompt(string label)
    {
        Console.Write($"  {label}: ");
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }

    private static string PromptChoice(string label, params string[] choices)
    {
        while (true)
        {
            Console.Write($"  {label}: ");
            var input = Console.ReadLine()?.Trim().ToLowerInvariant();
            if (input is not null && choices.Contains(input)) return input;
            Console.WriteLine($"    Please enter one of: {string.Join(", ", choices)}");
        }
    }

    private static T PromptEnum<T>(string label) where T : struct, Enum
    {
        while (true)
        {
            Console.Write($"  {label}: ");
            var input = Console.ReadLine()?.Trim();
            if (Enum.TryParse<T>(input, true, out var result)) return result;
            Console.WriteLine($"    Valid values: {string.Join(", ", Enum.GetNames<T>())}");
        }
    }

    private async Task<Guid?> PromptGuid(string label)
    {
        Console.Write($"  {label}: ");
        var input = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(input)) return null;

        // Try exact GUID first
        if (Guid.TryParse(input, out var exact)) return exact;

        // Otherwise search by prefix
        var all = await _taskService.GetAllTasksAsync();
        var match = all.FirstOrDefault(t => t.Id.ToString().StartsWith(input, StringComparison.OrdinalIgnoreCase));
        if (match is not null) return match.Id;

        Console.WriteLine("  Could not find a matching task ID.");
        return null;
    }

    private static void PrintBanner()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"
  ╔════════════════════════════════════════════════════╗
  ║    Smart Task Management System                    ║
  ║    Design Patterns Capstone — C# & .NET Course     ║
  ╚════════════════════════════════════════════════════╝");
        Console.ResetColor();
    }

    private static void PrintMenu()
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine(@"
  ┌─────────────────────────────┐
  │  1. Create Task             │
  │  2. List Tasks              │
  │  3. Assign Task             │
  │  4. Complete Task           │
  │  5. Undo Last Action        │
  │  6. Redo Last Action        │
  │  7. Show Report             │
  │  8. Show Summary            │
  │  9. Show Log                │
  │  0. Exit                    │
  └─────────────────────────────┘");
        Console.ResetColor();
        Console.Write("  Choice: ");
    }
}
