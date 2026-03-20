using System;
using System.Collections.Generic;

class Shape_Area_Override
{
    // Abstract base Shape: requires derived classes to implement Area()
    public abstract class Shape
    {
        public string Name { get; }
        protected Shape(string name) => Name = name;
        public abstract double Area();
    }

    public class Circle : Shape
    {
        public double Radius { get; }
        public Circle(double radius) : base("Circle") => Radius = radius;
        public override double Area() => Math.PI * Radius * Radius;
    }

    public class Rectangle : Shape
    {
        public double Width { get; }
        public double Height { get; }
        public Rectangle(double w, double h) : base("Rectangle") { Width = w; Height = h; }
        public override double Area() => Width * Height;
    }

    static void Main()
    {
        var shapes = new List<Shape>
        {
            new Circle(2.0),
            new Rectangle(3.0, 4.0),
            new Circle(1.0)
        };

        double total = 0;
        foreach (var s in shapes)
        {
            Console.WriteLine($"{s.GetType().Name} area = {s.Area():F2}");
            total += s.Area();
        }
        Console.WriteLine($"Total area = {total:F2}");

        // Inheritance used because shapes share concept of Area; abstract base enforces contract.
    }
}