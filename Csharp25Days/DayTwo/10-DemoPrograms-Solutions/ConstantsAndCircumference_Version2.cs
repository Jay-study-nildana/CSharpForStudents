using System;
using System.Globalization;

class Program
{
    const double PI = 3.14159265358979323846;

    static void Main()
    {
        Console.Write("Enter radius: ");
        if (!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out double r))
        {
            Console.WriteLine("Invalid radius.");
        }
        else
        {
            double circumference = 2 * PI * r;
            double area = PI * r * r;
            Console.WriteLine($"Circumference: {circumference.ToString("F3", CultureInfo.InvariantCulture)}");
            Console.WriteLine($"Area: {area.ToString("F3", CultureInfo.InvariantCulture)}");
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }
}