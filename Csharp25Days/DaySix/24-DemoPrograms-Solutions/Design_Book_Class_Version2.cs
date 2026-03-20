using System;

class Design_Book_Class
{
    // Book class: demonstrates private fields, read-only properties, constructor validation.
    public class Book
    {
        private int _pages;
        public string Title { get; }
        public string Author { get; }
        public int Pages
        {
            get => _pages;
            private set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(Pages), "Pages cannot be negative.");
                _pages = value;
            }
        }

        public Book(string title, string author, int pages)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Author = author ?? "Unknown";
            Pages = pages;
        }

        public override string ToString() => $"{Title} by {Author} ({Pages} pages)";
    }

    static void Main()
    {
        var book = new Book("1984", "George Orwell", 328);
        Console.WriteLine(book);
        // Encapsulation note: Pages uses a private backing field and a private setter,
        // so callers cannot set an invalid page count after construction.
    }
}