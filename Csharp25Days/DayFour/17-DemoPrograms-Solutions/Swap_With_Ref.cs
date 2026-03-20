using System;

class Swap_With_Ref
{
    // Swap two ints using ref
    // Demonstrates passing by reference vs by value
    // Time: O(1), Space: O(1)

    static void Swap(ref int a, ref int b)
    {
        int tmp = a;
        a = b;
        b = tmp;
    }

    static void NoSwap(int a, int b)
    {
        int tmp = a;
        a = b;
        b = tmp;
    }

    static void Main()
    {
        int x = 1, y = 2;
        Console.WriteLine($"Before: x={x}, y={y}");
        NoSwap(x, y);
        Console.WriteLine($"After NoSwap (by value): x={x}, y={y}");
        Swap(ref x, ref y);
        Console.WriteLine($"After Swap (by ref): x={x}, y={y}");
    }
}