// LambdaLINQFilter.cs
// Solution: lambdas with LINQ to filter/transform collections.

using System;
using System.Linq;

namespace Day19.Solutions
{
    public static class LambdaLINQFilter
    {
        public static void Run()
        {
            var numbers = new[] { 1, 2, 3, 4, 5, 6 };
            var processed = numbers
                .Where(n => n % 2 == 0)   // filter with lambda
                .Select(n => n * 10)      // transform with lambda
                .ToArray();

            Console.WriteLine($"Processed: {string.Join(',', processed)}"); // 20,40,60
        }
    }
}