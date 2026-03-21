namespace SmartStore.Core.Interfaces;

// -------------------------------------------------------
// UNIT OF WORK PATTERN
// -------------------------------------------------------
// Groups multiple repository operations into a single atomic transaction.
// Commit() persists all changes; Rollback() discards them.
// -------------------------------------------------------
public interface IUnitOfWork : IDisposable
{
    IOrderRepository Orders { get; }
    IProductRepository Products { get; }
    void Commit();
    void Rollback();
}
