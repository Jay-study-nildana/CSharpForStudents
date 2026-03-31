// Problem: MergeSortedLists
// Merge two sorted lists into one sorted list with no duplicates.
// Complexity: O(n+m) time, O(n+m) space (output list).

using System;
using System.Collections.Generic;

class MergeSortedLists
{
    static List<int> MergeNoDuplicates(List<int> a, List<int> b)
    {
        int i = 0, j = 0;
        var outList = new List<int>();
        int? last = null;
        while (i < a.Count && j < b.Count)
        {
            int val = a[i] < b[j] ? a[i++] : b[j++];
            if (last == null || val != last.Value)
            {
                outList.Add(val);
                last = val;
            }
            // advance possible equals in other list
            while (i < a.Count && a[i] == val) i++;
            while (j < b.Count && b[j] == val) j++;
        }
        while (i < a.Count)
        {
            int val = a[i++];
            if (last == null || val != last.Value) { outList.Add(val); last = val; }
        }
        while (j < b.Count)
        {
            int val = b[j++];
            if (last == null || val != last.Value) { outList.Add(val); last = val; }
        }
        return outList;
    }

    static void Main()
    {
        var a = new List<int> { 1, 2, 4, 4, 7 };
        var b = new List<int> { 2, 3, 4, 8 };
        Console.WriteLine(string.Join(", ", a)); // 1,2,4,4,7
        Console.WriteLine(string.Join(", ", b)); // 2,3,4,8
        var merged = MergeNoDuplicates(a, b);
        Console.WriteLine(string.Join(", ", merged)); // 1,2,3,4,7,8
    }
}