using System;

class Params_VarArgs_Sum
{
    // SumAll(params int[])
    // Demonstrates calling with array vs multiple args
    // Time: O(n), Space: O(1)

    static int SumAll(params int[] numbers)
    {
        int s = 0;
        if (numbers == null) return 0;
        foreach (var n in numbers) s += n;
        return s;
    }

    static void Main()
    {
        Console.WriteLine(SumAll(1, 2, 3));            // 6
        Console.WriteLine(SumAll(new int[] { 4, 5 })); // 9
        Console.WriteLine(SumAll());                   // 0
    }
}