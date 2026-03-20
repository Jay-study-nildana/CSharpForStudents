namespace LibraryManagement.Core.Interfaces;

/// <summary>
/// Simple logger contract so the domain layer stays independent of any
/// concrete logging framework (dependency-inversion principle, Day 17).
/// Log levels: Info, Warning, Error (Day 18).
/// </summary>
public interface IAppLogger
{
    void LogInfo(string message);
    void LogWarning(string message);
    void LogError(string message, Exception? ex = null);
}
