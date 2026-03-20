using System;

class Sum_Array
{
    // Sum all elements in an array
    // Control flow: foreach (read-only traversal)
    // Time: O(n), Space: O(1)
    static int Sum(int[] numbers)
    {
        int sum = 0;
        foreach (int x in numbers)
        {
            sum += x;
        }
        return sum;
    }

    static void Main()
    {
        int[] a = { 1, 2, 3, 4, 5 };
        Console.WriteLine(Sum(a)); // 15
    }
}