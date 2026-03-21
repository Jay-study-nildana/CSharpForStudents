using TaskManagement.Core.Interfaces;
using TaskManagement.Infrastructure.Persistence;

namespace TaskManagement.Infrastructure.Persistence;

// ─── Unit of Work pattern ─────────────────────────────────────────────────────
// Coordinates multiple repositories and flushes all changes atomically.

public class JsonFileUnitOfWork : IUnitOfWork
{
    private readonly JsonFileTaskRepository _taskRepo;

    public ITaskRepository Tasks => _taskRepo;

    public JsonFileUnitOfWork(string dataDirectory)
    {
        _taskRepo = new JsonFileTaskRepository(Path.Combine(dataDirectory, "tasks.json"));
    }

    public async Task<int> SaveChangesAsync()
    {
        await _taskRepo.FlushAsync();
        return 1;   // Indicates one "batch" saved
    }

    public void Dispose() { /* nothing unmanaged */ }
}
