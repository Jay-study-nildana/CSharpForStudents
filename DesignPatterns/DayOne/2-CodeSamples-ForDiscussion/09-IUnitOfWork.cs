// 09-IUnitOfWork.cs
// Unit of Work contract used to coordinate multiple repositories and transactional boundaries.

public interface IUnitOfWork
{
    IRepository<T> Repository<T>() where T : class;
    void Commit();
    void Rollback();
}