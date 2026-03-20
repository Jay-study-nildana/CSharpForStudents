using LibraryManagement.Core.Interfaces;

namespace LibraryManagement.Infrastructure.Logging;

/// <summary>
/// Dual-output logger: coloured console output + daily rolling log file.
///
/// Curriculum topics demonstrated:
///   Day 18 – structured log levels (Info / Warning / Error), log files
///   Day 20 – fire-and-forget async file write (non-blocking I/O)
///   Day 25 – SemaphoreSlim guards concurrent file-append operations
/// </summary>
public sealed class ConsoleFileLogger : IAppLogger
{
    private readonly string        _logFilePath;
    private readonly SemaphoreSlim _fileLock = new(1, 1);

    public ConsoleFileLogger(string logDirectory)
    {
        Directory.CreateDirectory(logDirectory);
        _logFilePath = Path.Combine(logDirectory, $"library-{DateTime.UtcNow:yyyy-MM-dd}.log");
    }

    public void LogInfo(string message)    => Log("INFO",  message, ConsoleColor.Gray);
    public void LogWarning(string message) => Log("WARN",  message, ConsoleColor.Yellow);
    public void LogError(string message, Exception? ex = null)
    {
        var full = ex is null ? message : $"{message} | {ex.GetType().Name}: {ex.Message}";
        Log("ERROR", full, ConsoleColor.Red);
    }

    private void Log(string level, string message, ConsoleColor colour)
    {
        var entry = $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] [{level,-5}] {message}";

        Console.ForegroundColor = colour;
        Console.WriteLine(entry);
        Console.ResetColor();

        // Fire-and-forget: writing to disk should not block the UI thread.
        // Unobserved faults are silently swallowed here; in production code
        // you would propagate them to a fallback sink (Day 20 discussion point).
        _ = AppendToFileAsync(entry);
    }

    private async Task AppendToFileAsync(string entry)
    {
        await _fileLock.WaitAsync();
        try
        {
            await File.AppendAllTextAsync(_logFilePath, entry + Environment.NewLine);
        }
        finally
        {
            _fileLock.Release();
        }
    }
}
