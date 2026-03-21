using TaskManagement.Core.Interfaces;

namespace TaskManagement.Core.Patterns.Behavioral;

// ─── Mediator pattern ─────────────────────────────────────────────────────────
// WorkflowMediator centralises how the UI, services, and event bus interact.
// Components send messages to the mediator instead of calling each other directly.

public interface IWorkflowMediator
{
    Task NotifyAsync(string eventName, object? data = null);
}

public class WorkflowMediator : IWorkflowMediator
{
    private readonly ITaskService _taskService;
    private readonly TaskEventBus _eventBus;
    private readonly IAppLogger _logger;

    public WorkflowMediator(ITaskService taskService, TaskEventBus eventBus, IAppLogger logger)
    {
        _taskService = taskService;
        _eventBus    = eventBus;
        _logger      = logger;
    }

    public async Task NotifyAsync(string eventName, object? data = null)
    {
        switch (eventName)
        {
            case "ShowSummary":
                var tasks = await _taskService.GetAllTasksAsync();
                var total = tasks.Count();
                var done  = tasks.Count(t => t.Status == Domain.TaskStatus.Done);
                _logger.Log($"[Mediator] Summary requested: {done}/{total} done.");
                Console.WriteLine($"\n  Total tasks: {total}  |  Completed: {done}  |  Remaining: {total - done}");
                break;

            case "TaskCreated":
                if (data is Domain.TaskItem created)
                    _eventBus.PublishCreated(created);
                break;

            case "TaskAssigned":
                if (data is (Domain.TaskItem assigned, string user))
                    _eventBus.PublishAssigned(assigned, user);
                break;

            case "TaskCompleted":
                if (data is Domain.TaskItem completed)
                    _eventBus.PublishCompleted(completed);
                break;

            default:
                _logger.Log($"[Mediator] Unknown event: {eventName}");
                break;
        }
    }
}
