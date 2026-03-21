```csharp
// 02-Adapter_LoggingAdapter.cs
// Intent: Adapter to map a legacy logging API to a modern ILogger interface.
// DI/Lifetime: Adapter stateless, register as Transient or Singleton depending on legacy implementation thread-safety.
// Testability: In tests provide a fake legacy logger to verify messages mapped and formatted correctly.

using System;

public interface ILogger
{
    void Info(string message);
    void Error(string message);
}

// Adaptee: legacy logging library (unified Write method)
public class LegacyLogger
{
    // Level could be "INFO", "ERROR", etc.
    public void Write(string level, string message)
    {
        Console.WriteLine($"[{level}] {message}");
    }
}

// Adapter: maps ILogger calls to LegacyLogger.Write
public class LegacyLoggingAdapter : ILogger
{
    private readonly LegacyLogger _legacy;
    public LegacyLoggingAdapter(LegacyLogger legacy) => _legacy = legacy;

    public void Info(string message) => _legacy.Write("INFO", message);
    public void Error(string message) => _legacy.Write("ERROR", message);
}

// Example service that uses ILogger
public class ProcessingService
{
    private readonly ILogger _logger;
    public ProcessingService(ILogger logger) => _logger = logger;

    public void Process()
    {
        _logger.Info("Processing started");
        try
        {
            // do work
        }
        catch (Exception ex)
        {
            _logger.Error($"Processing failed: {ex.Message}");
        }
    }
}