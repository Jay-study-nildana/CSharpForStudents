// BackgroundWorkerWithScopedServices.cs
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace DI.Exercises
{
    // A simple scoped repository used inside background worker
    public interface IBackgroundRepository
    {
        Task<int> CleanupAsync(CancellationToken ct = default);
    }

    public class BackgroundRepository : IBackgroundRepository
    {
        public Task<int> CleanupAsync(CancellationToken ct = default) => Task.FromResult(1);
    }

    // BackgroundService that uses IServiceScopeFactory to create a scope per iteration
    public class PeriodicCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _interval = TimeSpan.FromSeconds(10);

        public PeriodicCleanupService(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var repo = scope.ServiceProvider.GetRequiredService<IBackgroundRepository>();
                    var deleted = await repo.CleanupAsync(stoppingToken);
                    // log or handle result...
                }
                catch (Exception)
                {
                    // handle/log errors
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }
    }

    // Note: The BackgroundService must not request scoped services directly in ctor.
    // Using IServiceScopeFactory ensures each loop runs with a fresh scope and correct disposal.
}