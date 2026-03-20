using ComicBookShop.Core.Interfaces;

namespace ComicBookShop.Infrastructure.Logging;

/// <summary>
/// File-based structured logger with console echo for warnings/errors.
/// Demonstrates logging, file I/O, thread-safe writes, and log levels (Days 15, 18, 25).
/// </summary>
public class FileLogger : IAppLogger
{
    private readonly string _logFilePath;
    private readonly object _writeLock = new();

    public FileLogger(string logDirectory)
    {
        Directory.CreateDirectory(logDirectory);
        _logFilePath = Path.Combine(logDirectory, $"app_{DateTime.UtcNow:yyyyMMdd}.log");
    }

    public void LogInformation(string message, params object[] args) =>
        Log("INFO", message, args);

    public void LogWarning(string message, params object[] args) =>
        Log("WARN", message, args);

    public void LogError(string message, Exception? exception = null, params object[] args)
    {
        Log("ERROR", message, args);
        if (exception is not null)
            Log("ERROR", $"  Exception: {exception.GetType().Name}: {exception.Message}");
    }

    public void LogDebug(string message, params object[] args) =>
        Log("DEBUG", message, args);

    // ── Internal ────────────────────────────────────────────────────────

    private void Log(string level, string message, params object[] args)
    {
        var formatted = args.Length > 0 ? FormatMessage(message, args) : message;
        var entry = $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] [{level,-5}] {formatted}";

        lock (_writeLock)
        {
            File.AppendAllText(_logFilePath, entry + Environment.NewLine);
        }

        // Echo warnings and errors to console
        if (level is "WARN" or "ERROR")
        {
            var prev = Console.ForegroundColor;
            Console.ForegroundColor = level == "ERROR" ? ConsoleColor.Red : ConsoleColor.Yellow;
            Console.WriteLine($"  [{level}] {formatted}");
            Console.ForegroundColor = prev;
        }
    }

    /// <summary>Simple {placeholder} replacement with positional args.</summary>
    private static string FormatMessage(string template, object[] args)
    {
        var result = template;
        for (int i = 0; i < args.Length; i++)
        {
            int start = result.IndexOf('{');
            if (start < 0) break;
            int end = result.IndexOf('}', start);
            if (end < 0) break;
            result = string.Concat(result.AsSpan(0, start), args[i]?.ToString(), result.AsSpan(end + 1));
        }
        return result;
    }
}
