using System;
using System.Threading;

class Product_With_Static_IdGenerator
{
    // Product with thread-safe static id generator using Interlocked.
    public class Product
    {
        private static long _nextId = 0;
        public long Id { get; }
        public string Name { get; }

        public Product(string name)
        {
            Id = Interlocked.Increment(ref _nextId);
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public override string ToString() => $"Product {Id}: {Name}";
    }

    static void Main()
    {
        var p1 = new Product("Pen");
        var p2 = new Product("Notebook");
        Console.WriteLine(p1);
        Console.WriteLine(p2);
        // Static id generator ensures each product gets a unique Id across instances.
    }
}