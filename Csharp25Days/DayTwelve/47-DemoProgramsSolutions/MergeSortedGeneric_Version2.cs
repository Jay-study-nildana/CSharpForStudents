// MergeSortedGeneric.cs
// Problem: MergeSortedGeneric
// Merge two sorted IReadOnlyList<T> into sorted List<T> without duplicates.
// Complexity: O(n + m) time, O(n + m) space. Uses IComparer<T> to compare generically.

using System;
using System.Collections.Generic;

class MergeSortedGeneric
{
    public static List<T> MergeSorted<T>(IReadOnlyList<T> a, IReadOnlyList<T> b, IComparer<T>? comparer = null)
    {
        comparer ??= Comparer<T>.Default;
        var result = new List<T>();
        int i = 0, j = 0;
        T? last = default;
        bool hasLast = false;

        while (i < a.Count && j < b.Count)
        {
            T pick;
            if (comparer.Compare(a[i], b[j]) <= 0) pick = a[i++];
            else pick = b[j++];

            if (!hasLast || !EqualityComparer<T>.Default.Equals(last!, pick))
            {
                result.Add(pick);
                last = pick;
                hasLast = true;
            }
            // Skip duplicates equal to pick in both lists
            while (i < a.Count && comparer.Compare(a[i], pick) == 0) i++;
            while (j < b.Count && comparer.Compare(b[j], pick) == 0) j++;
        }
        while (i < a.Count)
        {
            var v = a[i++];
            if (!hasLast || !EqualityComparer<T>.Default.Equals(last!, v)) { result.Add(v); last = v; hasLast = true; }
        }
        while (j < b.Count)
        {
            var v = b[j++];
            if (!hasLast || !EqualityComparer<T>.Default.Equals(last!, v)) { result.Add(v); last = v; hasLast = true; }
        }
        return result;
    }

    static void Main()
    {
        var a = new List<int> { 1, 2, 4, 4, 7 };
        var b = new List<int> { 2, 3, 4, 8 };
        var merged = MergeSorted(a, b);
        Console.WriteLine(string.Join(", ", merged)); // 1, 2, 3, 4, 7, 8
    }
}