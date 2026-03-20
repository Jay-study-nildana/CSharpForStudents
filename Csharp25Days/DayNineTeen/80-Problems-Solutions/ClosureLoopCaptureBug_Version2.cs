// ClosureLoopCaptureBug.cs
// Solution: demonstrate loop variable capture bug and the correct fix.

using System;
using System.Collections.Generic;

namespace Day19.Solutions
{
    public static class ClosureLoopCaptureBug
    {
        public static void Run()
        {
            Console.WriteLine("Bad capture example:");
            var bad = new List<Action>();
            for (int i = 0; i < 3; i++)
            {
                // BAD: captures loop variable `i` directly
                bad.Add(() => Console.WriteLine($"Bad: {i}"));
            }
            foreach (var a in bad) a(); // often prints 3,3,3 (older semantics or surprising)

            Console.WriteLine("Fixed capture example:");
            var good = new List<Action>();
            for (int i = 0; i < 3; i++)
            {
                int copy = i; // copy per iteration
                good.Add(() => Console.WriteLine($"Good: {copy}"));
            }
            foreach (var a in good) a(); // prints 0,1,2
        }
    }
}