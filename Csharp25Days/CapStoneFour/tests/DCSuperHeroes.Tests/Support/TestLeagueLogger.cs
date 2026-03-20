using DCSuperHeroes.Core.Interfaces;

namespace DCSuperHeroes.Tests.Support;

public sealed class TestLeagueLogger : ILeagueLogger
{
    public List<string> Messages { get; } = [];

    public Task LogInfoAsync(string message, CancellationToken cancellationToken = default)
    {
        Messages.Add($"INFO:{message}");
        return Task.CompletedTask;
    }

    public Task LogWarningAsync(string message, CancellationToken cancellationToken = default)
    {
        Messages.Add($"WARN:{message}");
        return Task.CompletedTask;
    }

    public Task LogErrorAsync(string message, Exception? exception = null, CancellationToken cancellationToken = default)
    {
        Messages.Add($"ERROR:{message}");
        return Task.CompletedTask;
    }
}