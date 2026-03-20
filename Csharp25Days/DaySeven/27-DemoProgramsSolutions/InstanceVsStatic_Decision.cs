using System;

/// <summary>
/// Problem: InstanceVsStatic_Decision
/// Demonstrates when to use static helpers vs instance members.
/// Static for stateless math helpers; instance when state matters (Counter).
/// </summary>
class InstanceVsStatic_Decision
{
    // Static helper: pure function, stateless
    public static class MathHelpers
    {
        public static int Clamp(int value, int min, int max) =>
            Math.Max(min, Math.Min(max, value));
    }

    // Instance class: holds state per instance
    public class Counter
    {
        private int _count;
        public int Count => _count;

        public void Increment() => _count++;
        public void Add(int delta) => _count += delta;
    }

    static void Main()
    {
        Console.WriteLine("Static helper (Clamp): " + MathHelpers.Clamp(15, 0, 10));
        var c1 = new Counter();
        var c2 = new Counter();
        c1.Increment();
        c1.Add(2);
        c2.Add(5);
        Console.WriteLine($"Counter1: {c1.Count}, Counter2: {c2.Count}");
        Console.WriteLine("Note: MathHelpers is static (stateless). Counter is instance (per-object state).");
    }
}