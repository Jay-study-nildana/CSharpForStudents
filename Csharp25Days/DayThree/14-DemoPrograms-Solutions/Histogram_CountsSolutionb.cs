using System;
using System.Linq;

class Histogram_Counts
{
    // Count occurrences of values in range [0..K]
    // Time: O(n + K), Space: O(K)
    static int[] Histogram(int[] numbers, int K)
    {
        if (K < 0) throw new ArgumentException("K must be non-negative");
        int[] counts = new int[K + 1];
        foreach (int x in numbers)
        {
            if (x >= 0 && x <= K) counts[x]++;
            else Console.WriteLine($"Value {x} is out of expected range [0..{K}] and will be ignored.");
        }
        return counts;
    }

    static void PrintHorizontalHistogram(int[] counts)
    {
        Console.WriteLine("Value | Count");
        Console.WriteLine("------+-------");
        for (int i = 0; i < counts.Length; i++)
        {
            Console.WriteLine($"{i,5} | {new string('*', counts[i])} ({counts[i]})");
        }
    }

    static string Center(string s, int width)
    {
        if (s.Length >= width) return s;
        int left = (width - s.Length) / 2 + s.Length;
        return s.PadLeft(left).PadRight(width);
    }

    static void PrintVerticalHistogram(int[] counts)
    {
        int max = counts.Max();
        int cols = counts.Length;
        if (max == 0)
        {
            Console.WriteLine("All counts are zero; nothing to draw.");
            return;
        }

        int yLabelWidth = max.ToString().Length;   // width for y-axis numbers
        int colWidth = Math.Max(3, 3);             // width per column (adjustable)
        char barChar = '█';                        // block for clearer bars

        // Rows: from max down to 1
        for (int level = max; level >= 1; level--)
        {
            Console.Write(level.ToString().PadLeft(yLabelWidth) + " | ");
            for (int i = 0; i < cols; i++)
            {
                string cell = counts[i] >= level ? barChar.ToString() : " ";
                Console.Write(Center(cell, colWidth));
            }
            Console.WriteLine();
        }

        // x-axis separator
        Console.Write(new string(' ', yLabelWidth) + " | ");
        Console.WriteLine(new string('-', colWidth * cols));

        // x-axis labels (values), centered under each column
        Console.Write(new string(' ', yLabelWidth) + "   "); // spacing to align labels under columns
        for (int i = 0; i < cols; i++)
        {
            Console.Write(Center(i.ToString(), colWidth));
        }
        Console.WriteLine();
    }

    static void Main()
    {
        int K = 10;
        int[] a = new int[20];
        Random rand = new Random();
        for (int i = 0; i < a.Length; i++)
        {
            a[i] = rand.Next(0, K + 1); // random in [0..K]
        }

        var counts = Histogram(a, K);

        Console.WriteLine("Original array: " + string.Join(", ", a));
        Console.WriteLine();

        Console.WriteLine("Horizontal histogram (value | stars (count)):");
        PrintHorizontalHistogram(counts);

        Console.WriteLine();
        Console.WriteLine("Vertical histogram (y-axis = counts, x-axis = values):");
        PrintVerticalHistogram(counts);
    }
}