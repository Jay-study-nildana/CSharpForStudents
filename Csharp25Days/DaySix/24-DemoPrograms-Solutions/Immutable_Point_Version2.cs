using System;

class Immutable_Point
{
    // Immutable 2D point: get-only properties and method that returns a new translated point.
    public class Point
    {
        public double X { get; }
        public double Y { get; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Point Translate(double dx, double dy) => new Point(X + dx, Y + dy);

        public override string ToString() => $"({X}, {Y})";
    }

    static void Main()
    {
        var p = new Point(1.0, 2.0);
        var q = p.Translate(3.0, -1.0);
        Console.WriteLine($"p = {p}, q = {q}");
        // Immutable design: original point p remains unchanged after Translate.
    }
}