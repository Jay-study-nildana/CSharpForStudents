// BuildInvertedIndex.cs
// Problem: BuildInvertedIndex
// Pipeline: documents -> SelectMany(doc -> words.Select(word => (word, docId))) -> GroupBy(word) -> Select(word, distinct docIds)
// Complexity: O(totalWords * log w) depending on distinct collection operations; uses HashSet for distinct doc ids.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

record Document(int Id, string Content);

class BuildInvertedIndex
{
    static Dictionary<string, List<int>> Build(IEnumerable<Document> docs)
    {
        // split words using simple regex, normalize to lower-case
        var pairs = docs.SelectMany(doc =>
            Regex.Matches(doc.Content ?? "")
                 .Select(m => (Word: m.Value.ToLowerInvariant(), DocId: doc.Id))
        );

        // group by word, collect distinct doc ids
        return pairs
            .GroupBy(p => p.Word)
            .ToDictionary(g => g.Key, g => g.Select(p => p.DocId).Distinct().ToList());
    }

    static void Main()
    {
        var docs = new[]
        {
            new Document(1, "Apple banana apple."),
            new Document(2, "Banana orange."),
            new Document(3, "apple pie")
        };
        var idx = Build(docs);
        foreach (var kv in idx)
            Console.WriteLine($"{kv.Key}: [{string.Join(", ", kv.Value)}]");
        // apple: [1,3]
        // banana: [1,2]
        // orange: [2]
        // pie: [3]
    }
}