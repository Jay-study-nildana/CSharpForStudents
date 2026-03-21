namespace SmartStore.Infrastructure;

// -------------------------------------------------------
// UNIT OF WORK PATTERN — In-Memory implementation
// -------------------------------------------------------
// Wraps two repositories under one transactional boundary.
// In this in-memory version Commit() and Rollback() are
// simulated — in a real app they'd flush/rollback a DbContext.
// -------------------------------------------------------
public class InMemoryUnitOfWork : IUnitOfWork
{
    private bool _committed;

    public IOrderRepository   Orders   { get; }
    public IProductRepository Products { get; }

    public InMemoryUnitOfWork(IOrderRepository orders, IProductRepository products)
    {
        Orders   = orders;
        Products = products;
    }

    public void Commit()
    {
        _committed = true;
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("  [UnitOfWork] Transaction committed.");
        Console.ResetColor();
    }

    public void Rollback()
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("  [UnitOfWork] Transaction rolled back.");
        Console.ResetColor();
    }

    public void Dispose()
    {
        if (!_committed)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("  [UnitOfWork] Warning: disposed without commit.");
            Console.ResetColor();
        }
        GC.SuppressFinalize(this);
    }
}
