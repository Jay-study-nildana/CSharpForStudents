using TaskManagement.Core.Interfaces;

namespace TaskManagement.Infrastructure.Logging;

// ─── Singleton pattern ────────────────────────────────────────────────────────
// AppLogger is registered as a singleton in the DI container.
// It stores an in-memory log history and tees output to a file.

public class AppLogger : IAppLogger
{
    private readonly List<string> _history = new();
    private readonly string _logFilePath;
    private readonly object _lock = new();

    public AppLogger(string logDirectory)
    {
        Directory.CreateDirectory(logDirectory);
        _logFilePath = Path.Combine(logDirectory, "app.log");
    }

    public void Log(string message)
    {
        var entry = $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] INFO  {message}";
        Write(entry);
    }

    public void LogError(string message)
    {
        var entry = $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] ERROR {message}";
        Write(entry);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(entry);
        Console.ResetColor();
    }

    public IReadOnlyList<string> GetHistory()
    {
        lock (_lock) return _history.AsReadOnly();
    }

    private void Write(string entry)
    {
        lock (_lock)
        {
            _history.Add(entry);
            using var sw = File.AppendText(_logFilePath);
            sw.WriteLine(entry);
        }
    }
}
