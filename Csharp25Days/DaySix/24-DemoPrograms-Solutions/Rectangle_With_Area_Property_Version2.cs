using System;

class Rectangle_With_Area_Property
{
    // Rectangle with init-only properties and computed Area property.
    public class Rectangle
    {
        public double Width { get; init; }
        public double Height { get; init; }
        public double Area => Width * Height;

        public Rectangle(double width, double height)
        {
            if (width < 0 || height < 0) throw new ArgumentOutOfRangeException("Dimensions must be non-negative.");
            Width = width;
            Height = height;
        }

        public override string ToString() => $"W={Width}, H={Height}, Area={Area}";
    }

    static void Main()
    {
        var r = new Rectangle(3.5, 2.0);
        Console.WriteLine(r);
        // Area is computed; there is no stored 'area' field to keep in sync.
    }
}