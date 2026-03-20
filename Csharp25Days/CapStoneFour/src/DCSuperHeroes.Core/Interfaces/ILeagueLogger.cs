namespace DCSuperHeroes.Core.Interfaces;

public interface ILeagueLogger
{
    Task LogInfoAsync(string message, CancellationToken cancellationToken = default);
    Task LogWarningAsync(string message, CancellationToken cancellationToken = default);
    Task LogErrorAsync(string message, Exception? exception = null, CancellationToken cancellationToken = default);
}