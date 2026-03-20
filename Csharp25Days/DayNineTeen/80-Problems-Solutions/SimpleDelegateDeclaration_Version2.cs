// SimpleDelegateDeclaration.cs
// Solution: declare a custom delegate, assign a method, and invoke it.

using System;

namespace Day19.Solutions
{
    // Custom delegate type
    public delegate int Transformer(int x);

    public static class SimpleDelegateDeclaration
    {
        public static int Square(int x) => x * x;

        // Demo method: shows assignment and invocation
        public static void Run()
        {
            Transformer t = Square;   // method group conversion
            int result = t(7);        // invoke delegate
            Console.WriteLine($"Square(7) = {result}"); // expected: 49
        }
    }
}