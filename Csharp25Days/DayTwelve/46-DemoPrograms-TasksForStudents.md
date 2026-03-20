# Day 12 — Generics & Type Safety: Practice Problems

Instructions
- Solve each problem using C# generics and appropriate .NET generic collections.
- Provide a short explanation of your chosen collection(s), expected time and space complexity, and whether boxing is avoided (for value types).
- Aim for clear, idiomatic code that compiles on .NET 6+.

Problems

1) CountDistinctGeneric  
Write a generic method CountDistinct<T>(IEnumerable<T> items, IEqualityComparer<T>? comparer = null) that returns the number of distinct items. Explain why a HashSet<T> is appropriate and whether boxing occurs for value types.

2) TopKByFrequencyGeneric  
Implement TopKByFrequency<T>(IEnumerable<T> items, int k, IEqualityComparer<T>? comparer = null) returning the top-k most frequent items. Use an efficient approach (dictionary + min-heap / PriorityQueue). Explain time/space trade-offs.

3) MergeSortedGeneric  
Write MergeSorted<T>(IReadOnlyList<T> a, IReadOnlyList<T> b, IComparer<T>? comparer = null) to merge two sorted sequences into a new sorted List<T> without duplicates. Describe complexity and why IComparer<T> is used.

4) RemoveInPlaceGeneric  
Implement RemoveInPlace<T>(List<T> list, Predicate<T> shouldRemove) that removes matching items in-place without allocating a new list. Show a safe iteration pattern and analyze complexity.

5) GroupByKeyGeneric  
Create GroupByKey<T, TKey>(IEnumerable<T> items, Func<T, TKey> keySelector, IEqualityComparer<TKey>? keyComparer = null) returning Dictionary<TKey, List<T>>. Demonstrate using this to group anagrams (use key = sorted chars string).

6) GenericRepositoryCrud  
Design a simple in-memory generic repository: class InMemoryRepository<T, TId> where TId : notnull. Implement Add, Get, Update, Remove using Dictionary<TId, T>. Show how to provide the id selector via Func<T, TId>.

7) BoundedRecentActivityGeneric  
Implement a bounded recent activity store RecentActivity<T> that keeps the last N items (newest first). Provide Add(T) and IEnumerable<T> GetRecent() and explain complexity. Use LinkedList<T> or Queue<T> appropriately.

8) GroupAnagramsGeneric (via GroupByKeyGeneric)  
Using GroupByKey from problem 5, implement GroupAnagrams(IEnumerable<string> words) that returns List<List<string>> groups of anagrams. Explain complexity and show no unnecessary boxing.

9) ConcurrentFrequencyCountGeneric  
Implement ConcurrentFrequencyCount<T>(IEnumerable<T> items, IEqualityComparer<T>? comparer = null) to count frequencies in parallel using ConcurrentDictionary<T,int>. Discuss concurrency choices and whether locking is needed.

10) PairAndSwapGeneric  
Create a generic Pair<T> record/class and a static Swap<T>(ref T a, ref T b) method. Demonstrate usage with both value types and reference types and explain how generics avoid casts/boxing.

Deliverables
- Implementations (one per problem) with short usage examples.
- Complexity notes and brief explanation for collection choices.
- Solutions should be C# and compile on .NET 6+.