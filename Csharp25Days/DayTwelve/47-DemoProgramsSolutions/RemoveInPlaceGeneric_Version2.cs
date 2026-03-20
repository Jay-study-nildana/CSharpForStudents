// RemoveInPlaceGeneric.cs
// Problem: RemoveInPlaceGeneric
// Remove items matching predicate from a List<T> in-place without allocating a new list.
// Complexity: O(n) time, O(1) extra space.

using System;
using System.Collections.Generic;

class RemoveInPlaceGeneric
{
    public static void RemoveInPlace<T>(List<T> list, Predicate<T> shouldRemove)
    {
        // Use two-pointer overwrite approach to avoid many RemoveAt calls
        int write = 0;
        for (int read = 0; read < list.Count; read++)
        {
            if (!shouldRemove(list[read]!))
            {
                list[write++] = list[read]!;
            }
        }
        if (write < list.Count)
            list.RemoveRange(write, list.Count - write);
    }

    static void Main()
    {
        var nums = new List<int> { 1, 2, 3, 4, 5, 6 };
        RemoveInPlace(nums, x => x % 2 == 0);
        Console.WriteLine(string.Join(", ", nums)); // 1, 3, 5
    }
}