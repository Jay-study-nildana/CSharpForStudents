// FuncActionPredicateUsage.cs
// Solution: show Func, Action, and Predicate usage.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Day19.Solutions
{
    public static class FuncActionPredicateUsage
    {
        public static void Run()
        {
            // Func<T,TResult>
            Func<int, string> toHex = v => v.ToString("X");
            Console.WriteLine($"255 => 0x{toHex(255)}"); // "FF"

            // Action<T>
            Action<string> log = s => Console.WriteLine($"LOG: {s}");
            log("Hello Action"); // side-effect

            // Predicate<T>
            Predicate<int> isEven = n => n % 2 == 0;
            var numbers = Enumerable.Range(1, 6).ToList();
            var evens = numbers.FindAll(isEven);
            Console.WriteLine($"Evens: {string.Join(',', evens)}"); // 2,4,6
        }
    }
}