# Day 11 — Collections: Practice Problems (C# / .NET)

Instructions
- Solve each problem using appropriate C#/.NET collection types.
- Aim for clear, idiomatic solutions and note expected time/space complexity.
- Where concurrency, ordering, or uniqueness matters, explain your choice of collection.

Problems

1) CountDistinctWords
Given an array of words (strings), return the number of distinct words (case-insensitive). Discuss complexity and which collection you used.

2) TopKFrequentWords
Given an array of words and integer k, return the k most frequent words in descending frequency. If tie, order lexicographically. Implement an efficient solution and explain trade-offs.

3) PrefixSearchTrie
Design and implement a simple Trie (prefix tree) to support `Insert(string)` and `GetWordsWithPrefix(string prefix)` returning all words that start with prefix. Discuss memory and lookup complexity.

4) MergeSortedLists
Given two sorted lists of integers, merge them into a single sorted list (no duplicates). Show an O(n) solution using appropriate iteration pattern.

5) RemoveEvenNumbersInPlace
Given a `List<int>`, remove all even numbers in-place (no new list). Provide a safe iteration pattern and explain why it avoids enumeration errors.

6) GroupByFirstLetter
Given a list of names, group them by first letter into a `Dictionary<char,List<string>>` and keep each list sorted. Show creation and retrieval of group for a given letter.

7) TaskLookupById
Model a simple `Task` entity (Id: Guid, Title: string). Given many tasks, design a collection for O(1) lookup by id, and show adding, updating, and deleting tasks.

8) RecentActivityBoundedQueue
Implement a bounded recent-activity store that keeps the last N events (append newest, drop oldest). Provide `Add(Event)` and `GetRecent()` operations and state their complexity.

9) FindAnagrams
Given a list of words, group them into sets of anagrams (words with same letters). Return `List<List<string>>` groups. Choose collections for grouping and explain costs.

10) ConcurrentWordCount
Given a large enumerable of text lines processed in parallel, compute a thread-safe word frequency dictionary. Use appropriate concurrent collections and discuss locking vs lock-free options.

Deliverables
- Solutions should be in C# and compile on .NET 6+ (unless you request otherwise).
- For each problem, state the time and space complexity in a comment or short note.