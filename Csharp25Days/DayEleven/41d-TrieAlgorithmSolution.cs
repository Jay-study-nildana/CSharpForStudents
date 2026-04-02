using Jay.LearningHelperForStudents;
using System;
using System.Collections.Generic;

var trie = new Trie();

var randomgenerator = new RandomRepeatedWordsGenerator();

//get a collection from the GenerateRandomRepeatedWords Method.

var words = randomgenerator.GenerateRandomRepeatedWords(null, 20, 2, 10, null, true, true);

foreach(var x in words)
{
    Console.WriteLine(String.Join(' ',x));
}

// insert words; optionally insert duplicates to simulate frequency
foreach (var w in words)
{
    trie.Insert(w);
}

//lets display frequency of all words in the trie

var wordsunique = words.Distinct().ToList() ;

foreach(var x in wordsunique)
{
    Console.WriteLine($"Frequency of \"{x}\" => {trie.WordFrequency(x)}");
}

Console.WriteLine("Enter a word to search:");
string input;
input = Console.ReadLine() ?? "";

Console.WriteLine($"Search(\"{input}\") => {trie.Search(input)}");
Console.WriteLine();
Console.WriteLine($"Frequency of \"{input}\" => {trie.WordFrequency(input)}");

// autocomplete / suggestions
Console.WriteLine("Enter a word for auto complete and suggestions:");
var prefix = Console.ReadLine() ?? "";
Console.WriteLine($"Words with prefix \"{prefix}\":");
foreach (var w in trie.GetWordsWithPrefix(prefix).OrderBy(s => s))
{
    Console.WriteLine("  " + w);
}

// demonstrate deletion
Console.WriteLine("Enter a word for deletions:");
var deleteterm =Console.ReadLine() ?? "";
Console.WriteLine($"Deleting '{deleteterm}'...");
trie.Delete(deleteterm);
Console.WriteLine($"Search(\"{deleteterm}\") => {trie.Search(deleteterm)}");
Console.WriteLine($"Words with prefix \"{prefix}\":");
foreach (var w in trie.GetWordsWithPrefix(prefix).OrderBy(s => s))
{
    Console.WriteLine("  " + w);
}

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

    public int WordFrequency(string word)
    {
        if (string.IsNullOrEmpty(word)) return 0;
        var node = _root;
        foreach(var ch in word)
        {
            if (!node.Children.TryGetValue(ch, out node)) return 0;
        }
        return node.IsWord ? node.Frequency : 0;
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