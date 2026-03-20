// Problem: CountDistinctWords
// Given an array of words, return the number of distinct words (case-insensitive).
// Complexity: O(n * avg_len) time, O(u) space where u is number of unique words.

using System;
using System.Collections.Generic;

class CountDistinctWords
{
    static int CountDistinct(string[] words)
    {
        var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var w in words)
            if (!string.IsNullOrEmpty(w))
                set.Add(w);
        return set.Count;
    }

    // Example usage
    static void Main()
    {
        var words = new[] {"Apple", "banana", "apple", "Cherry", "BANANA", ""};
        Console.WriteLine(CountDistinct(words)); // Output: 3
    }
}