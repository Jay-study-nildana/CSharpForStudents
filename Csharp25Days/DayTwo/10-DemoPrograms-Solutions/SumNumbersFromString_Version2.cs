using System;
using System.Globalization;

class Program
{
    static void Main()
    {
        Console.WriteLine("Enter numbers separated by spaces (integers or decimals):");
        string? line = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(line))
        {
            Console.WriteLine("No input provided.");
        }
        else
        {
            string[] tokens = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            double sum = 0.0;
            int ignored = 0;
            foreach (var t in tokens)
            {
                if (double.TryParse(t, NumberStyles.Any, CultureInfo.InvariantCulture, out double v))
                {
                    sum += v;
                }
                else
                {
                    ignored++;
                }
            }

            Console.WriteLine($"Sum of parsed numbers: {sum.ToString("G", CultureInfo.InvariantCulture)}");
            Console.WriteLine($"Ignored tokens: {ignored}");
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }
}