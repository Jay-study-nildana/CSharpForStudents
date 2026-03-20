using System;
using System.Collections.Generic;
using System.Linq;

class Library_With_Private_Collections
{
    // Library manages books internally and exposes only read-only views.
    public class Book
    {
        public string Title { get; }
        public Book(string title) => Title = title ?? throw new ArgumentNullException(nameof(title));
        public override string ToString() => Title;
    }

    public class Library
    {
        private readonly List<Book> _books = new();
        public IReadOnlyCollection<Book> Books => _books.AsReadOnly();

        public void AddBook(Book book)
        {
            if (book == null) throw new ArgumentNullException(nameof(book));
            _books.Add(book);
        }

        public Book LendBook(string title)
        {
            var found = _books.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (found != null)
            {
                _books.Remove(found);
                return found;
            }
            return null;
        }
    }

    static void Main()
    {
        var lib = new Library();
        lib.AddBook(new Book("C# in Depth"));
        lib.AddBook(new Book("Design Patterns"));

        Console.WriteLine("Available: " + string.Join(", ", lib.Books));
        var lent = lib.LendBook("C# in Depth");
        Console.WriteLine("Lent: " + lent);
        Console.WriteLine("Available after lending: " + string.Join(", ", lib.Books));
        // Encapsulation prevents callers from modifying internal list directly.
    }
}