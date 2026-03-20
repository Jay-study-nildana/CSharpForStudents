namespace LibraryManagement.Core.Exceptions;

public sealed class BookNotAvailableException : LibraryException
{
    public Guid BookId { get; }

    public BookNotAvailableException(Guid bookId, string bookTitle)
        : base($"Book \"{bookTitle}\" (ID: {bookId}) is not available for borrowing.")
    {
        BookId = bookId;
    }
}
