namespace ComicBookShop.Core.Exceptions;

/// <summary>
/// Thrown when an order requests more copies than are in stock.
/// Demonstrates custom exceptions with extra context (Day 14).
/// </summary>
public class InsufficientStockException : Exception
{
    public string ComicTitle { get; }
    public int Available { get; }
    public int Requested { get; }

    public InsufficientStockException(string comicTitle, int available, int requested)
        : base($"Insufficient stock for '{comicTitle}'. Available: {available}, Requested: {requested}.")
    {
        ComicTitle = comicTitle;
        Available = available;
        Requested = requested;
    }
}
