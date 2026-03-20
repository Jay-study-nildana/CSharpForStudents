using System;

class Readonly_Struct_Performance
{
    // Small readonly struct for safe value semantics and thread-safety
    public readonly struct Vector3
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }
        public Vector3(double x, double y, double z) { X = x; Y = y; Z = z; }
        public double Length() => Math.Sqrt(X*X + Y*Y + Z*Z);
        public Vector3 Scale(double s) => new Vector3(X*s, Y*s, Z*s);
    }

    static void Main()
    {
        var v = new Vector3(1,2,3);
        var scaled = v.Scale(2);
        Console.WriteLine($"v.Length={v.Length():F2}, scaled.Length={scaled.Length():F2}");
        Console.WriteLine("readonly struct prevents accidental mutation and is efficient for small value types.");
    }
}