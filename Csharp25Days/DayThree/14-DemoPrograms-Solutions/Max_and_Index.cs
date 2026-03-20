using System;

class Max_and_Index
{
    // Find maximum value and its first index
    // Control flow: for (need index)
    // Time: O(n), Space: O(1)
    static (int maxValue, int index) FindMaxAndIndex(int[] numbers)
    {
        if (numbers == null || numbers.Length == 0)
            throw new ArgumentException("Array must be non-empty");

        int maxVal = numbers[0];
        int idx = 0;
        for (int i = 1; i < numbers.Length; i++)
        {
            if (numbers[i] > maxVal)
            {
                maxVal = numbers[i];
                idx = i;
            }
        }
        return (maxVal, idx);
    }

    static void Main()
    {
        int[] a = { 3, 7, 2, 7, 5 };
        var (max, i) = FindMaxAndIndex(a);
        Console.WriteLine($"{max} at {i}"); // "7 at 1"
    }
}