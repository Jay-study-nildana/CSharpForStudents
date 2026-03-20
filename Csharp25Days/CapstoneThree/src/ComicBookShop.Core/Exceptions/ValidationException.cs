namespace ComicBookShop.Core.Exceptions;

/// <summary>
/// Thrown when input data fails business-rule validation.
/// Demonstrates defensive programming (Day 14).
/// </summary>
public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}
