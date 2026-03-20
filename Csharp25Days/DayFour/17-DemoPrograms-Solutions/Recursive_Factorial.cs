using System;

class Recursive_Factorial
{
    // Factorial implemented recursively and iteratively
    // Recursive: local variables exist per call (stack frame)
    // Time: O(n), Space: O(n) recursion, iterative space O(1)

    static long FactorialRecursive(int n)
    {
        if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));
        if (n == 0) return 1;
        return n * FactorialRecursive(n - 1);
    }

    static long FactorialIterative(int n)
    {
        if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));
        long r = 1;
        for (int i = 2; i <= n; i++) r *= i;
        return r;
    }

    static void Main()
    {
        Console.WriteLine(FactorialRecursive(5)); // 120
        Console.WriteLine(FactorialIterative(5)); // 120
    }
}