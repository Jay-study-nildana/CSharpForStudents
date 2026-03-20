// PairAndSwapGeneric.cs
// Problem: PairAndSwapGeneric
// Generic Pair<T> and Swap<T>(ref T a, ref T b). Demonstrate with value and reference types.
// Complexity: O(1) operations. No boxing when used with value types.

using System;

class Pair<T>
{
    public T First { get; set; }
    public T Second { get; set; }
    public Pair(T first, T second) { First = first; Second = second; }
    public override string ToString() => $"({First}, {Second})";
}

class PairAndSwapGeneric
{
    public static void Swap<T>(ref T a, ref T b)
    {
        T tmp = a;
        a = b;
        b = tmp;
    }

    static void Main()
    {
        // Value types
        int x = 1, y = 2;
        Console.WriteLine($"Before: x={x}, y={y}");
        Swap(ref x, ref y);
        Console.WriteLine($"After: x={x}, y={y}");

        // Reference types
        var p = new Pair<string>("Alice", "Bob");
        Console.WriteLine("Pair before: " + p);
        Swap(ref p.First, ref p.Second);
        Console.WriteLine("Pair after: " + p);
    }
}