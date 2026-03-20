using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("Sum and Average Calculator");
        Console.WriteLine("Enter numbers separated by spaces (or press Enter to input one per line).");
        Console.Write("Input: ");
        string? line = Console.ReadLine();

        List<double> numbers = new List<double>();

        if (!string.IsNullOrWhiteSpace(line))
        {
            var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var p in parts)
            {
                if (double.TryParse(p, NumberStyles.Any, CultureInfo.InvariantCulture, out double v))
                    numbers.Add(v);
            }
        }
        else
        {
            Console.WriteLine("Enter numbers one per line. Empty line to finish.");
            while (true)
            {
                string? l = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(l)) break;
                if (double.TryParse(l.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out double v))
                    numbers.Add(v);
                else
                    Console.WriteLine("Invalid number, try again.");
            }
        }

        if (numbers.Count == 0)
        {
            Console.WriteLine("No numbers provided.");
        }
        else
        {
            double sum = numbers.Sum();
            double avg = numbers.Average();
            double min = numbers.Min();
            double max = numbers.Max();
            Console.WriteLine($"Count: {numbers.Count}");
            Console.WriteLine($"Sum: {sum}");
            Console.WriteLine($"Average: {avg}");
            Console.WriteLine($"Min: {min}");
            Console.WriteLine($"Max: {max}");
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }
}