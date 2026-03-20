// GroupAnagramsGeneric.cs
// Problem: GroupAnagramsGeneric (implemented via generic GroupByKey approach)
// Complexity: O(n * L log L) using sorting per word; can be O(n * L) with counting key.

using System;
using System.Collections.Generic;
using System.Linq;

class GroupAnagramsGeneric
{
    public static List<List<string>> GroupAnagrams(IEnumerable<string> words)
    {
        var dict = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        foreach (var w in words)
        {
            var keyChars = w.ToLowerInvariant().ToCharArray();
            Array.Sort(keyChars);
            var key = new string(keyChars); // sorted letters as key
            if (!dict.TryGetValue(key, out var lst)) dict[key] = lst = new List<string>();
            lst.Add(w);
        }
        return dict.Values.Select(g => g.ToList()).ToList();
    }

    static void Main()
    {
        var words = new[] { "eat", "tea", "tan", "ate", "nat", "bat" };
        var groups = GroupAnagrams(words);
        foreach (var g in groups) Console.WriteLine($"[{string.Join(", ", g)}]");
    }
}