using DCSuperHeroes.Core.Interfaces;

namespace DCSuperHeroes.Infrastructure.Logging;

public sealed class FileLeagueLogger : ILeagueLogger
{
    private readonly string _logDirectory;
    private readonly SemaphoreSlim _gate = new(1, 1);

    public FileLeagueLogger(string logDirectory)
    {
        _logDirectory = logDirectory;
        Directory.CreateDirectory(_logDirectory);
    }

    public Task LogInfoAsync(string message, CancellationToken cancellationToken = default) =>
        WriteAsync("INFO", ConsoleColor.Cyan, message, cancellationToken);

    public Task LogWarningAsync(string message, CancellationToken cancellationToken = default) =>
        WriteAsync("WARN", ConsoleColor.Yellow, message, cancellationToken);

    public Task LogErrorAsync(string message, Exception? exception = null, CancellationToken cancellationToken = default)
    {
        var combined = exception is null
            ? message
            : $"{message}{Environment.NewLine}{exception}";

        return WriteAsync("ERROR", ConsoleColor.Red, combined, cancellationToken);
    }

    private async Task WriteAsync(string level, ConsoleColor color, string message, CancellationToken cancellationToken)
    {
        var line = $"[{DateTime.UtcNow:O}] [{level}] {message}";

        await _gate.WaitAsync(cancellationToken);
        try
        {
            Console.ForegroundColor = color;
            Console.WriteLine(line);
            Console.ResetColor();

            var filePath = Path.Combine(_logDirectory, $"watchtower-{DateTime.UtcNow:yyyyMMdd}.log");
            await File.AppendAllTextAsync(filePath, line + Environment.NewLine, cancellationToken);
        }
        finally
        {
            _gate.Release();
        }
    }
}