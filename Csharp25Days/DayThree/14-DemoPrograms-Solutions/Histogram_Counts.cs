using System;

class Histogram_Counts
{
    // Count occurrences of values in range [0..K]
    // Control flow: foreach to traverse input; index into counts
    // Time: O(n + K), Space: O(K)
    static int[] Histogram(int[] numbers, int K)
    {
        if (K < 0) throw new ArgumentException("K must be non-negative");
        int[] counts = new int[K + 1];
        foreach (int x in numbers)
        {
            if (x >= 0 && x <= K) counts[x]++;
            else throw new ArgumentOutOfRangeException(nameof(numbers), "Value out of expected range");
        }
        return counts;
    }

    static void Main()
    {
        int[] a = { 0, 2, 1, 2, 0, 3 };
        int K = 3;
        var counts = Histogram(a, K);
        Console.WriteLine(string.Join(", ", counts)); // counts for 0..3
    }
}