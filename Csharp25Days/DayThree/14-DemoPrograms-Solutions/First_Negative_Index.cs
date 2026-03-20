using System;

class First_Negative_Index
{
    // Return index of first negative number or -1
    // Control flow: for with early break (or return)
    // Time: O(n) worst-case, O(1) best-case, Space: O(1)
    static int FirstNegative(int[] numbers)
    {
        for (int i = 0; i < numbers.Length; i++)
        {
            if (numbers[i] < 0) return i; // early exit
        }
        return -1;
    }

    static void Main()
    {
        int[] a = { 3, 5, -2, 0 };
        Console.WriteLine(FirstNegative(a)); // 2
    }
}