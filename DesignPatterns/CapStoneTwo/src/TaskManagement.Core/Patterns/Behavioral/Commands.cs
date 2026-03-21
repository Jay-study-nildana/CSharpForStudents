using TaskManagement.Core.Domain;
using TaskManagement.Core.Interfaces;
using DomainTaskStatus = TaskManagement.Core.Domain.TaskStatus;

namespace TaskManagement.Core.Patterns.Behavioral;

// ─── Command pattern ──────────────────────────────────────────────────────────
// ICommand is defined in IInterfaces.cs.

/// <summary>
/// Assigns a task to a user. Supports undo (un-assigns and reverts status).
/// </summary>
public class AssignTaskCommand : ICommand
{
    private readonly TaskItem _task;
    private readonly string _newUser;
    private readonly string? _previousUser;
    private readonly DomainTaskStatus _previousStatus;
    private readonly IUnitOfWork _uow;

    public string Description => $"Assign '{_task.Title}' to {_newUser}";

    public AssignTaskCommand(TaskItem task, string newUser, IUnitOfWork uow)
    {
        _task = task;
        _newUser = newUser;
        _previousUser = task.AssignedTo;
        _previousStatus = task.Status;
        _uow = uow;
    }

    public async Task ExecuteAsync()
    {
        _task.Assign(_newUser);
        await _uow.Tasks.UpdateAsync(_task);
        await _uow.SaveChangesAsync();
    }

    public async Task UndoAsync()
    {
        // Restore previous state
        if (_previousUser is not null)
            _task.Assign(_previousUser);
        else
        {
            // Reflect in domain by reverting status manually
            _task.UpdateStatus(_previousStatus);
        }
        await _uow.Tasks.UpdateAsync(_task);
        await _uow.SaveChangesAsync();
    }
}

/// <summary>
/// Marks a task as complete.
/// </summary>
public class CompleteTaskCommand : ICommand
{
    private readonly TaskItem _task;
    private readonly DomainTaskStatus _previousStatus;
    private readonly IUnitOfWork _uow;

    public string Description => $"Complete '{_task.Title}'";

    public CompleteTaskCommand(TaskItem task, IUnitOfWork uow)
    {
        _task = task;
        _previousStatus = task.Status;
        _uow = uow;
    }

    public async Task ExecuteAsync()
    {
        _task.Complete();
        await _uow.Tasks.UpdateAsync(_task);
        await _uow.SaveChangesAsync();
    }

    public async Task UndoAsync()
    {
        _task.UpdateStatus(_previousStatus);
        await _uow.Tasks.UpdateAsync(_task);
        await _uow.SaveChangesAsync();
    }
}

/// <summary>
/// Command history for undo/redo support.
/// </summary>
public class CommandHistory
{
    private readonly Stack<ICommand> _undoStack = new();
    private readonly Stack<ICommand> _redoStack = new();

    public bool CanUndo => _undoStack.Count > 0;
    public bool CanRedo => _redoStack.Count > 0;

    public async Task ExecuteAsync(ICommand command)
    {
        await command.ExecuteAsync();
        _undoStack.Push(command);
        _redoStack.Clear();   // New action invalidates redo history
    }

    public async Task UndoAsync()
    {
        if (!CanUndo) return;
        var command = _undoStack.Pop();
        await command.UndoAsync();
        _redoStack.Push(command);
        Console.WriteLine($"  [Undo] {command.Description}");
    }

    public async Task RedoAsync()
    {
        if (!CanRedo) return;
        var command = _redoStack.Pop();
        await command.ExecuteAsync();
        _undoStack.Push(command);
        Console.WriteLine($"  [Redo] {command.Description}");
    }

    public IEnumerable<string> GetHistory() =>
        _undoStack.Select(c => c.Description).Reverse();
}
