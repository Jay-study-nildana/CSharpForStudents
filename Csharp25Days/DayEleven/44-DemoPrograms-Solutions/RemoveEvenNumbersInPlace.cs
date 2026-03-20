// Problem: RemoveEvenNumbersInPlace
// Remove even numbers from a List<int> in-place safely.
// Complexity: O(n) time, O(1) extra space.

using System;
using System.Collections.Generic;

class RemoveEvenNumbersInPlace
{
    // Iterate backwards to remove safely
    static void RemoveEvens(List<int> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i] % 2 == 0) list.RemoveAt(i);
        }
    }

    static void Main()
    {
        var list = new List<int>{1,2,3,4,5,6};
        RemoveEvens(list);
        Console.WriteLine(string.Join(", ", list)); // 1,3,5
    }
}