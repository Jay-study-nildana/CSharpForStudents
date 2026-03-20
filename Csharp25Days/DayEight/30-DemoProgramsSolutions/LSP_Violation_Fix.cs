using System;

class LSP_Violation_Fix
{
    // Demonstrate LSP violation with Rectangle/Square and then show a safer design.
    public class Rectangle
    {
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }
        public int Area() => Width * Height;
    }

    // Problematic Square inherits Rectangle and overrides setters: breaks LSP
    public class BadSquare : Rectangle
    {
        private int _side;
        public override int Width { get => _side; set { _side = value; base.Width = value; base.Height = value; } }
        public override int Height { get => _side; set { _side = value; base.Width = value; base.Height = value; } }
    }

    // Safer: use IShape interface or separate classes without inheritance for mutable rectangle/square
    public interface IShape { int Area(); }

    public class Rect : IShape
    {
        public int Width { get; }
        public int Height { get; }
        public Rect(int w, int h) { Width = w; Height = h; }
        public int Area() => Width * Height;
    }

    public class Square : IShape
    {
        public int Side { get; }
        public Square(int side) { Side = side; }
        public int Area() => Side * Side;
    }

    static void Main()
    {
        // Show the LSP violation example
        Rectangle r = new Rectangle { Width = 2, Height = 3 };
        Console.WriteLine($"Rectangle area (2x3): {r.Area()}"); // 6

        Rectangle s = new BadSquare();
        s.Width = 5;
        s.Height = 4;
        Console.WriteLine($"BadSquare reported area (after set Width=5 THEN Height=4): {s.Area()}");
        // Unexpected semantics: setting Height after Width changes side; behavior surprising → violates LSP.

        // Safer approach using IShape
        IShape a = new Rect(2,3);
        IShape b = new Square(5);
        Console.WriteLine($"Rect area: {a.Area()}, Square area: {b.Area()}");

        Console.WriteLine("Fix: avoid inheriting Square from Rectangle when mutability semantics differ; prefer composition or common interface.");
    }
}