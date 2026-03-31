# Tries (Prefix Trees) — Concept, Complexity, and C# Implementation

Overview
--------
A trie (pronounced "try"), also called a prefix tree, is a tree-like data structure for storing a set of strings (or sequences). Each node represents a prefix; edges represent characters. Tries provide very fast prefix-based queries — insert, exact lookup, and prefix search are all proportional to the length of the key (not the number of keys). Tries are useful for autocomplete/typeahead, dictionary lookups, longest-prefix matching (e.g., routing), and any application where prefix queries are common.

Key ideas
---------
- Root node represents the empty prefix.
- Each edge out of a node is labeled with a character (or token).
- A path from the root to a node spells a prefix; nodes often store a flag (`IsWord`) to indicate a stored string.
- Common prefixes are shared, so many words with the same prefix store that prefix only once.

Core operations
---------------
- `Insert(word)`: walk characters and create nodes as needed. Mark final node as a word.
- `Search(word)`: walk characters; success if you reach a node whose `IsWord` is true.
- `StartsWith(prefix)`: walk characters; success if all chars exist (regardless of `IsWord`).
- `Enumerate completions`: find node for prefix and traverse its subtree to collect words.

Time and space complexity
-------------------------
- `Insert` / `Search` / `StartsWith`: O(L) time where L is the length of the key (word).
- Enumerate completions: O(P + M) where P is prefix length to reach the node and M is cost to traverse and emit matches.
- Space: O(total characters across all keys) in naive implementations, plus overhead per node for child pointers/collections. Tries can be memory-heavy if many nodes have few children (sparse alphabet).

Variants and optimizations
--------------------------
- Compressed trie / Radix tree / Patricia trie: collapse chains of single-child nodes into a single edge labeled with multiple characters — reduces nodes and speeds traversal.
- Ternary Search Tree (TST): memory-efficient alternative that behaves like a BST on characters.
- Array-based children: for a small fixed alphabet (e.g., `a`–`z`), store children in an array for faster access and lower per-child overhead.
- Suffix trie / suffix tree: stores all suffixes for substring queries (memory heavy).
- Store counts/weights at nodes to support frequency-based suggestions.
- Use `Span<char>` and pooling where available in hot code paths to reduce allocations.

When to use a trie
------------------
- Autocomplete/typeahead (return all words that start with a prefix).
- Dictionary with many shared prefixes.
- Longest-prefix match (routing, IP prefix lookup).
- Cases where query time must be O(length) and lexicographic traversal is needed.

When not to use a trie
----------------------
- Small key sets or random keys where hash tables are simpler and smaller.
- When alphabet is very large and nodes are sparse (memory overhead).
- When only exact lookup is needed and hash/dictionary is sufficient.

Representation choices and trade-offs
-------------------------------------
- Use `Dictionary<char, Node>` for sparse alphabets and flexibility.
- Use fixed arrays for dense and small alphabets (fast, low overhead).
- Use compressed edges (radix) to save memory when many nodes have single children.
- Consider storing frequencies at word nodes for ranking suggestions.

---

## C# implementation — generic trie (Dictionary-based)

Paste the following `Trie.cs` into a C# project. It demonstrates insertion, exact search, prefix search, enumeration of completions, and deletion.

```csharp
// Trie.cs
using System;
using System.Collections.Generic;

public class TrieNode
{
    // children map; use Dictionary<char, TrieNode> for sparse alphabets
    public readonly Dictionary<char, TrieNode> Children = new();
    public bool IsWord;
    // optional: store frequency/count or payload
    public int Frequency; // e.g., how many times the word was inserted (0 if not a word)
}

public class Trie
{
    private readonly TrieNode _root = new();

    // Insert a word; increments Frequency at the final node.
    public void Insert(string word)
    {
        if (string.IsNullOrEmpty(word)) return;
        var node = _root;
        foreach (var ch in word)
        {
            if (!node.Children.TryGetValue(ch, out var next))
            {
                next = new TrieNode();
                node.Children[ch] = next;
            }
            node = next;
        }
        node.IsWord = true;
        node.Frequency++; // optional: track frequency
    }

    // Exact match
    public bool Search(string word)
    {
        if (string.IsNullOrEmpty(word)) return false;
        var node = _root;
        foreach (var ch in word)
        {
            if (!node.Children.TryGetValue(ch, out node)) return false;
        }
        return node.IsWord;
    }

    // Does any word start with the given prefix?
    public bool StartsWith(string prefix)
    {
        if (string.IsNullOrEmpty(prefix)) return true;
        var node = _root;
        foreach (var ch in prefix)
        {
            if (!node.Children.TryGetValue(ch, out node)) return false;
        }
        return true;
    }

    // Enumerate all words that start with the given prefix.
    public IEnumerable<string> GetWordsWithPrefix(string prefix)
    {
        var node = _root;
        foreach (var ch in prefix)
        {
            if (!node.Children.TryGetValue(ch, out node)) yield break;
        }
        foreach (var word in Collect(node, prefix)) yield return word;
    }

    // Depth-first collect (simple recursion)
    private IEnumerable<string> Collect(TrieNode node, string current)
    {
        if (node.IsWord) yield return current;
        foreach (var kvp in node.Children)
        {
            foreach (var w in Collect(kvp.Value, current + kvp.Key))
                yield return w;
        }
    }

    // Optional: delete a word; returns true if deleted
    public bool Delete(string word)
    {
        return DeleteInternal(_root, word, 0);
    }

    private bool DeleteInternal(TrieNode node, string word, int depth)
    {
        if (node == null) return false;
        if (depth == word.Length)
        {
            if (!node.IsWord) return false;
            node.IsWord = false;
            node.Frequency = 0;
            // If node has no children, signal to parent it can remove this child.
            return node.Children.Count == 0;
        }

        var ch = word[depth];
        if (!node.Children.TryGetValue(ch, out var child)) return false;
        var shouldDeleteChild = DeleteInternal(child, word, depth + 1);
        if (shouldDeleteChild)
        {
            node.Children.Remove(ch);
            return !node.IsWord && node.Children.Count == 0;
        }
        return false;
    }
}
```

---

## Example usage — console program demonstrating insert, search, and autocomplete

Create a `Program.cs` and paste this example to see the trie in action.

```csharp
// Program.cs
using System;
using System.Linq;

class Program
{
    static void Main()
    {
        var trie = new Trie();

        // sample words (could come from a file or corpus)
        var words = new[]
        {
            "apple", "app", "application", "apt", "banana", "band", "bandana",
            "can", "candy", "cap", "cape", "capital"
        };

        // insert words; optionally insert duplicates to simulate frequency
        foreach (var w in words)
        {
            trie.Insert(w);
        }
        trie.Insert("apple"); // increment frequency for "apple"
        trie.Insert("app");   // increment frequency for "app"

        Console.WriteLine($"Search(\"app\") => {trie.Search("app")}");
        Console.WriteLine($"Search(\"apricot\") => {trie.Search("apricot")}");
        Console.WriteLine();

        // autocomplete / suggestions
        var prefix = "ap";
        Console.WriteLine($"Words with prefix \"{prefix}\":");
        foreach (var w in trie.GetWordsWithPrefix(prefix).OrderBy(s => s))
        {
            Console.WriteLine("  " + w);
        }

        // demonstrate deletion
        Console.WriteLine();
        Console.WriteLine("Deleting 'app'...");
        trie.Delete("app");
        Console.WriteLine($"Search(\"app\") => {trie.Search("app")}");
        Console.WriteLine($"Words with prefix \"{prefix}\":");
        foreach (var w in trie.GetWordsWithPrefix(prefix).OrderBy(s => s))
        {
            Console.WriteLine("  " + w);
        }
    }
}
```

---

## Optimizations and memory considerations
- If your input is limited to lowercase ASCII (`a`–`z`), replace `Dictionary<char, TrieNode>` with `TrieNode[] children = new TrieNode[26]` for much lower overhead and faster indexing.
- Use compressed tries (radix) to collapse single-child chains: reduces node count and memory, and improves cache locality.
- For large corpora, avoid recursion when collecting words (use an explicit stack) to prevent stack overflow.
- If you need frequency-ranked suggestions, store frequency at word nodes (as shown) and use a priority queue when collecting top-K suggestions per prefix.
- For streaming heavy-hitter scenarios, consider combining a Space-Saving algorithm for heavy hitters with a trie keyed by top prefixes.

---

## Exercises for students
1. Implement an array-based trie for lowercase letters and benchmark memory and lookup speed vs the Dictionary-based trie.
2. Extend the trie to return the top-K most frequent completions for a prefix (store frequency at nodes; use a min-heap while traversing).
3. Implement a compressed trie (radix trie) that stores string segments on edges. Compare node count with the uncompressed trie for a sample word list (e.g., many words with common prefixes).
4. Add deletion unit tests to verify edge cases (deleting non-existent words, deleting words that are prefixes of others).
5. Measure performance: build a trie from a large word list (e.g., `/usr/share/dict/words`) and time `Insert`, `Search`, and `GetWordsWithPrefix`.

---

## Further reading
- Algorithms textbooks (trie/prefix tree chapters)
- Radix trees / Patricia tries for production-grade prefix trees
- Ternary Search Trees as memory-efficient alternatives
- For large-scale or persistent data: prefix B-trees or tries optimized for disk

---


