using System;
using System.Linq;

class Tuple_Returns_Summary
{
    static (int Sum, double Average, int Count) Summarize(int[] values)
    {
        if (values == null || values.Length == 0) return (0, 0.0, 0);
        int sum = values.Sum();
        double avg = (double)sum / values.Length;
        return (sum, avg, values.Length);
    }

    static void Main()
    {
        var data = new[] { 1, 2, 3, 4, 5 };
        var result = Summarize(data);
        Console.WriteLine($"Sum={result.Sum}, Avg={result.Average:F2}, Count={result.Count}");

        // Deconstruction
        var (s, a, c) = Summarize(new int[] { 10, 20 });
        Console.WriteLine($"Deconstructed: Sum={s}, Avg={a}, Count={c}");

        Console.WriteLine("Use named tuples for quick multi-value returns; prefer records for public APIs.");
    }
}