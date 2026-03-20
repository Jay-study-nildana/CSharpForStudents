using System;
using System.Threading;

/// <summary>
/// Problem: Testable_IdGenerator
/// Shows static generator vs injectable, testable instance generator.
/// </summary>
class Testable_IdGenerator
{
    // Static generator (global state)
    public static class StaticIdGenerator
    {
        private static long _next = 0;
        public static long Next() => Interlocked.Increment(ref _next);
    }

    // Testable interface and implementation
    public interface IIdGenerator
    {
        long Next();
    }

    public class SequentialIdGenerator : IIdGenerator
    {
        private long _next = 0;
        public long Next() => Interlocked.Increment(ref _next);
    }

    static void Main()
    {
        Console.WriteLine("Static IDs: " + StaticIdGenerator.Next() + ", " + StaticIdGenerator.Next());

        var gen = new SequentialIdGenerator();
        Console.WriteLine("Instance IDs: " + gen.Next() + ", " + gen.Next());

        Console.WriteLine("Testing-friendly: create SequentialIdGenerator in tests and assert deterministic sequence.");
    }
}