# Day 11 — Collections: arrays, lists, dictionaries, sets (C# / .NET)

Objectives
- Core collection types in C# (.NET) and their trade‑offs.
- How to choose the right collection for common tasks.
- Iteration patterns and complexity implications.
- Guided exercise: design a word-frequency structure for fast lookups.
- Homework: plan collections for capstone entities and sketch operations.

Why collections matter
Choose collections by the operations you need (indexing, insert/delete, lookup, iteration, ordering, uniqueness, concurrency). Think about semantics (ordered vs unordered, unique vs duplicate, mutable vs immutable) and complexity (O(1), O(log n), O(n)).

Core types and characteristics (conceptual)
- Array (`T[]`)
  - Fixed-size contiguous storage. O(1) index access, O(n) insert/delete in middle; low overhead.
  - Use for numeric data, buffers, or when size is known.

- List<T> (`List<T>`)
  - Dynamic array: amortized O(1) append, O(1) index access, O(n) mid-insert/remove.
  - Default choice for ordered, resizable collections.

- LinkedList<T>
  - Doubly linked list: O(1) insert/remove if you have the node; O(n) search/index.
  - Use for frequent insert/remove at both ends or known node operations.

- Dictionary<TKey,TValue> / HashSet<T>
  - Hash-based: average O(1) lookup/insert/remove. Dictionary: key→value; HashSet: unique items.
  - Use for fast membership and mapping (IDs → objects).

- SortedDictionary<TKey,TValue> / SortedSet<T>
  - Tree-based: O(log n) operations, keys remain ordered. Use when you need ordering or range queries.

- Concurrent collections (e.g., `ConcurrentDictionary`) when multithreaded access matters.

Iteration patterns
- Index-based: `for (int i = 0; i < arr.Length; i++)` — when indices matter.
- `foreach` over `IEnumerable<T>` — idiomatic and safe.
- Key/value iteration for dictionaries: `foreach (var kv in dict)` to avoid extra lookups.
- LINQ (`Select`, `Where`, `OrderBy`) for concise transformations (may allocate/iterate).

Complexity trade-offs (high-level)
- Random access vs insertion cost: arrays / List<T> favor random access; linked lists favor cheap node insert/remove.
- Uniqueness vs duplicates: HashSet<T> ensures uniqueness; List<T> allows duplicates.
- Ordering vs performance: sorted collections add log n cost but provide order/range operations.
- Memory overhead: hash tables need extra space; linked lists have per-node overhead.

In-class quick comparison
- Array: +fast index, low overhead; −fixed size.
- List<T>: +resizable, familiar API; −mid-list ops cost O(n).
- LinkedList<T>: +cheap node ops; −no index access, high overhead.
- Dictionary: +fast lookup; −unordered, key hashing cost.
- SortedDictionary: +ordered; −slower than Dictionary.
- HashSet: +dedup +fast membership; −no order.

Guided exercise — word frequency + fast lookups
Goals:
- frequency(word) → count
- exists(word) → bool
- top_k(k) → most frequent k words
- optional: prefix lookup (autocomplete)

Design choices
- Primary store: `Dictionary<string,int>` for O(1) average count updates/lookups.
- Top-k: for occasional queries use LINQ `OrderByDescending().Take(k)`; for streaming/large data use a size-k min-heap (`PriorityQueue<TElement,TPriority>`) for O(n log k).
- Prefix search: add a Trie (prefix tree) or maintain an inverted index mapping prefix → `HashSet<string>`.

C# examples

Simple counting + top-k with LINQ:
```csharp
using System.Collections.Generic;
using System.Linq;

Dictionary<string,int> CountWords(IEnumerable<string> words)
{
    var counts = new Dictionary<string,int>(StringComparer.OrdinalIgnoreCase);
    foreach (var w in words)
        counts[w] = counts.GetValueOrDefault(w) + 1;
    return counts;
}

List<(string word,int count)> TopK(Dictionary<string,int> counts, int k)
{
    return counts.OrderByDescending(kv => kv.Value)
                 .Take(k)
                 .Select(kv => (kv.Key, kv.Value))
                 .ToList();
}
```

Streaming top-k using PriorityQueue (.NET 6+):
```csharp
// Maintain a min-heap of size k
var pq = new PriorityQueue<string,int>();
foreach (var kv in counts)
{
    pq.Enqueue(kv.Key, kv.Value);
    if (pq.Count > k) pq.Dequeue();
}
// pq now holds top-k words by count (smallest count at root)
```

Trie sketch for prefix lookups (simplified):
```csharp
class TrieNode
{
    public Dictionary<char,TrieNode> Children = new();
    public bool IsWord;
}
void Insert(TrieNode root, string word)
{
    var node = root;
    foreach (var ch in word)
        node = node.Children.TryGetValue(ch, out var next) ? next : node.Children[ch] = new TrieNode();
    node.IsWord = true;
}
```

Homework (capstone planning)
For your capstone, list 3 entities and choose collection(s) + justify:

Example: Task manager
- tasks: `Dictionary<Guid,Task>` — O(1) lookup/update by id.
- userTasks: `Dictionary<Guid,List<Guid>>` or `Dictionary<Guid,HashSet<Guid>>` (use HashSet to avoid duplicate task links).
- tags: `HashSet<string>` per task — deduplicated tags.
- recentActivity: `LinkedList<Activity>` or `Queue<Activity>` for append/pop oldest.
- searchIndex: `Dictionary<string,HashSet<Guid>>` (term → set of task ids) for quick lookup.

Sample operations (C# pseudo):
```csharp
var task = tasks[taskId];                // O(1)
task.Tags.Add("urgent");                 // HashSet.Add O(1)
recentActivity.AddFirst(newActivity);    // LinkedList O(1)
var matches = searchIndex.GetValueOrDefault(term, new HashSet<Guid>());
```

Deliverable for next class
- Be ready to justify one chosen collection per entity (operation cost, semantics).
- Bring one operation and state its expected complexity.

Tip: Start with Dictionary/ List for clarity; optimize with specialized structures (trie, heap, concurrent collections) only when needed by profiling.