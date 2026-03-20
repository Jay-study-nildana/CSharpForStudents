namespace LibraryManagement.Core.Exceptions;

public sealed class BookNotFoundException : LibraryException
{
    public Guid BookId { get; }

    public BookNotFoundException(Guid bookId)
        : base($"Book with ID '{bookId}' was not found.")
    {
        BookId = bookId;
    }
}
