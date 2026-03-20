using System;

class Record_Struct_Point
{
    // C# 10+ readonly record struct (value type with record conveniences)
    public readonly record struct Point(int X, int Y);

    static void Main()
    {
        Point p1 = new(2, 3);
        var p2 = p1 with { X = 5 }; // returns a new Point (value copy with change)
        Console.WriteLine($"p1: ({p1.X},{p1.Y}), p2: ({p2.X},{p2.Y})");

        var arr = new Point[] { new(0,0), new(1,1), new(2,2) };
        foreach (var p in arr) Console.WriteLine($"Point: {p}");

        Console.WriteLine("record struct gives value semantics + concise syntax and deconstruction support.");
    }
}