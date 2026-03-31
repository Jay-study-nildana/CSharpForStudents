// Problem: FindAnagrams
// Group words that are anagrams into List<List<string>>.
// Complexity: O(n * L log L) where L is average word length (sorting chars), or O(n * L) with counting key.

using System;
using System.Collections.Generic;
using System.Linq;

class FindAnagrams
{
    static List<List<string>> GroupAnagrams(IEnumerable<string> words)
    {
        var dict = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        foreach (var w in words)
        {
            var key = String.Concat(
                                    w.ToLowerInvariant()
                                    .OrderBy(c => c)
                                    ); // sorted letters as key
            if (!dict.TryGetValue(key, out var list)) 
                dict[key] = list = new List<string>();
            list.Add(w);
        }
        return dict.Values.ToList();
    }

    static void Main()
    {
        var words = new[] { "eat", "tea", "tan", "ate", "nat", "bat" };
        var groups = GroupAnagrams(words);
        foreach (var g in groups) Console.WriteLine($"[{string.Join(", ", g)}]");
    }
}