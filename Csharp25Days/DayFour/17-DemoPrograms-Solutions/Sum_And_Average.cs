using System;

class Sum_And_Average
{
    // Sum(int[]) - returns sum
    // SumAndAverage(int[]) - returns (sum, average) as tuple
    // Control flow: for/foreach; return tuple for multiple outputs
    // Time: O(n), Space: O(1)

    static int Sum(int[] values)
    {
        if (values == null) throw new ArgumentNullException(nameof(values));
        int s = 0;
        foreach (var v in values) s += v;
        return s;
    }

    static (int sum, double average) SumAndAverage(int[] values)
    {
        if (values == null) throw new ArgumentNullException(nameof(values));
        if (values.Length == 0) return (0, 0.0);
        int s = Sum(values);
        double avg = (double)s / values.Length;
        return (s, avg);
    }

    static void Main()
    {
        int[] a = { 2, 4, 6, 8 };
        var sum = Sum(a);
        var (s, avg) = SumAndAverage(a);
        Console.WriteLine($"Sum: {sum}, Tuple: Sum={s}, Avg={avg:F2}");
    }
}