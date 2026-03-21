namespace SmartStore.Patterns.Behavioral.Commands;

// ================================================================
// COMMAND PATTERN
// ================================================================
// Encapsulates requests as objects, enabling undo/redo, queuing,
// and command history.
//
// Intent   : Encapsulate a request as an object, allowing you to
//            parameterise clients, queue requests, and support undo.
// Problem  : Order operations (place, cancel) need to be reversible
//            and auditable without coupling the caller to the executor.
// Solution : Each operation is an ICommand (Execute / Undo).
//            CommandHistory maintains a stack for undo support.
// ================================================================

public class PlaceOrderCommand : ICommand
{
    private readonly IUnitOfWork _uow;
    private readonly Order _order;

    public PlaceOrderCommand(IUnitOfWork uow, Order order)
    {
        _uow   = uow;
        _order = order;
    }

    public string Description => $"Place Order #{_order.Id} for {_order.Customer.Name}";

    public void Execute()
    {
        _order.Status = OrderStatus.Pending;
        _uow.Orders.Add(_order);
        _uow.Commit();
        Console.WriteLine($"    [Command] Executed: {Description}");
    }

    public void Undo()
    {
        _order.Status = OrderStatus.Cancelled;
        _uow.Orders.Remove(_order.Id);
        _uow.Commit();
        Console.WriteLine($"    [Command] Undone  : {Description} — order removed and status set to Cancelled");
    }
}

public class CancelOrderCommand : ICommand
{
    private readonly IUnitOfWork _uow;
    private readonly Order _order;
    private OrderStatus _previousStatus;

    public CancelOrderCommand(IUnitOfWork uow, Order order)
    {
        _uow   = uow;
        _order = order;
    }

    public string Description => $"Cancel Order #{_order.Id}";

    public void Execute()
    {
        _previousStatus = _order.Status;
        _order.Status   = OrderStatus.Cancelled;
        _uow.Orders.Update(_order);
        _uow.Commit();
        Console.WriteLine($"    [Command] Executed: {Description}  (was: {_previousStatus})");
    }

    public void Undo()
    {
        _order.Status = _previousStatus;
        _uow.Orders.Update(_order);
        _uow.Commit();
        Console.WriteLine($"    [Command] Undone  : {Description} — status restored to {_previousStatus}");
    }
}

// ================================================================
// COMMAND HISTORY (Invoker)
// ================================================================
public class CommandHistory
{
    private readonly Stack<ICommand> _history = new();

    public void Execute(ICommand command)
    {
        command.Execute();
        _history.Push(command);
    }

    public void Undo()
    {
        if (_history.Count == 0)
        {
            Console.WriteLine("    [History] Nothing to undo.");
            return;
        }
        var cmd = _history.Pop();
        cmd.Undo();
    }

    public void Print()
    {
        Console.WriteLine("    [History] Stack (most recent first):");
        if (_history.Count == 0)
        {
            Console.WriteLine("              (empty)");
            return;
        }
        foreach (var cmd in _history)
            Console.WriteLine($"              • {cmd.Description}");
    }
}
