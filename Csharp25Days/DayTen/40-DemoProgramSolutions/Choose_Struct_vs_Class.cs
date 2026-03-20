using System;

class Choose_Struct_vs_Class
{
    // 2D point used in tight math loops -> struct (value semantics)
    public readonly struct PointStruct
    {
        public double X { get; }
        public double Y { get; }
        public PointStruct(double x, double y) { X = x; Y = y; }
        public PointStruct Translate(double dx, double dy) => new PointStruct(X + dx, Y + dy);
        public override string ToString() => $"({X},{Y})";
    }

    // Customer entity with identity and lifecycle -> class (reference semantics)
    public class CustomerClass
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public CustomerClass(string name) { Id = Guid.NewGuid(); Name = name; }
        public override string ToString() => $"{Name} [{Id}]";
    }

    static void Main()
    {
        var p1 = new PointStruct(1, 2);
        var p2 = p1; // copy
        p2 = p2.Translate(3, 0);
        Console.WriteLine($"p1: {p1}, p2: {p2}"); // p1 unchanged

        var c1 = new CustomerClass("Alice");
        var c2 = c1; // reference copy
        c2.Name = "Alice Modified";
        Console.WriteLine($"c1: {c1}, c2: {c2}"); // both show modified name

        Console.WriteLine("Rationale: small value-like Point -> struct. Entity with identity -> class.");
    }
}