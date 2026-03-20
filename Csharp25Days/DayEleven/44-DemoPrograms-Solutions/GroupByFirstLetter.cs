// Problem: GroupByFirstLetter
// Group names by first letter into Dictionary<char,List<string>> and keep lists sorted.
// Complexity: O(n log m) overall if sorting each group where m is group size.

using System;
using System.Collections.Generic;
using System.Linq;

class GroupByFirstLetter
{
    static Dictionary<char,List<string>> GroupAndSort(IEnumerable<string> names)
    {
        var dict = new Dictionary<char, List<string>>();
        foreach (var name in names)
        {
            if (string.IsNullOrEmpty(name)) continue;
            char key = char.ToUpperInvariant(name[0]);
            if (!dict.TryGetValue(key, out var list)) dict[key] = list = new List<string>();
            list.Add(name);
        }
        // sort each group
        foreach (var kv in dict) kv.Value.Sort(StringComparer.OrdinalIgnoreCase);
        return dict;
    }

    static void Main()
    {
        var names = new[]{"alice","Bob","aaron","Bella","charlie"};
        var groups = GroupAndSort(names);
        foreach (var ch in new[]{'A','B','C'})
        {
            if (groups.TryGetValue(ch, out var list))
                Console.WriteLine($"{ch}: {string.Join(", ", list)}");
        }
    }
}