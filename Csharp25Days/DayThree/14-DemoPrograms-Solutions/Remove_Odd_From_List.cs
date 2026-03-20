using System;
using System.Collections.Generic;

class Remove_Odd_From_List
{
    // Remove odd numbers from a List<int>
    // Control flow: backward for-loop OR List.RemoveAll (preferred)
    // Using RemoveAll here: O(n) time, O(1) extra space (amortized)
    static void RemoveOdds(List<int> list)
    {
        // Preferred: built-in RemoveAll which scans once
        list.RemoveAll(x => (x & 1) != 0);
    }

    static void Main()
    {
        var list = new List<int> { 1, 2, 3, 4, 5, 6 };
        RemoveOdds(list);
        Console.WriteLine(string.Join(", ", list)); // 2, 4, 6
    }
}