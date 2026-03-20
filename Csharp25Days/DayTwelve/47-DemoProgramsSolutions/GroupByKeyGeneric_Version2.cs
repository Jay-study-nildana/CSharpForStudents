// GroupByKeyGeneric.cs
// Problem: GroupByKeyGeneric
// Generic grouping by a key selector: returns Dictionary<TKey, List<T>>.
// Complexity: O(n * keyCost) time, O(n) space.

using System;
using System.Collections.Generic;

class GroupByKeyGeneric
{
    public static Dictionary<TKey, List<T>> GroupByKey<T, TKey>(
        IEnumerable<T> items,
        Func<T, TKey> keySelector,
        IEqualityComparer<TKey>? keyComparer = null)
    {
        var dict = new Dictionary<TKey, List<T>>(keyComparer);
        foreach (var item in items)
        {
            var key = keySelector(item!);
            if (!dict.TryGetValue(key, out var list))
            {
                list = new List<T>();
                dict[key] = list;
            }
            list.Add(item!);
        }
        return dict;
    }

    // Example: group anagrams using key = sorted characters string
    static void Main()
    {
        var words = new[] { "eat", "tea", "tan", "ate", "nat", "bat" };
        var groups = GroupByKey(words, s => {
            var ch = s.ToCharArray();
            Array.Sort(ch);
            return new string(ch);
        }, StringComparer.OrdinalIgnoreCase);

        foreach (var kv in groups)
            Console.WriteLine($"Key {kv.Key}: [{string.Join(", ", kv.Value)}]");
    }
}