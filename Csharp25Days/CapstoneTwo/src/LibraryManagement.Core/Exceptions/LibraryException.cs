namespace LibraryManagement.Core.Exceptions;

/// <summary>
/// Base class for all domain-specific exceptions in the library system.
/// Demonstrates custom exception hierarchies (Day 14).
/// </summary>
public abstract class LibraryException : Exception
{
    protected LibraryException(string message) : base(message) { }
    protected LibraryException(string message, Exception innerException)
        : base(message, innerException) { }
}
