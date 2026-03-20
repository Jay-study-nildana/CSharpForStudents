using System;
using System.Collections.Generic;

class Student_With_Borrowing
{
    // Demonstrates encapsulating an internal list and exposing a read-only view.
    public class Book
    {
        public string Title { get; }
        public Book(string title) => Title = title ?? throw new ArgumentNullException(nameof(title));
        public override string ToString() => Title;
    }

    public class Student
    {
        public int Id { get; }
        public string Name { get; }
        private readonly List<Book> _borrowed = new();
        public IReadOnlyList<Book> BorrowedBooks => _borrowed.AsReadOnly();

        public Student(int id, string name)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public void Borrow(Book book)
        {
            if (book == null) throw new ArgumentNullException(nameof(book));
            _borrowed.Add(book);
        }

        public bool Return(Book book) => _borrowed.Remove(book);
    }

    static void Main()
    {
        var s = new Student(1, "Alice");
        var b = new Book("Clean Code");
        s.Borrow(b);
        Console.WriteLine($"{s.Name} borrowed: {string.Join(", ", s.BorrowedBooks)}");
        // Encapsulation: callers cannot modify _borrowed directly because BorrowedBooks is IReadOnlyList.
    }
}