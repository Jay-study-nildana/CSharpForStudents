namespace ComicBookShop.Core.Interfaces;

/// <summary>
/// Abstraction for structured application logging.
/// Demonstrates interface-based decoupling and log levels (Days 9, 18).
/// </summary>
public interface IAppLogger
{
    void LogInformation(string message, params object[] args);
    void LogWarning(string message, params object[] args);
    void LogError(string message, Exception? exception = null, params object[] args);
    void LogDebug(string message, params object[] args);
}
