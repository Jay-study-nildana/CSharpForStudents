// Problem: PrefixSearchTrie
// Implement a Trie supporting Insert and GetWordsWithPrefix.
// Complexity: Insert O(L), prefix lookup O(L + m) where L = prefix length, m = matched nodes output.

using System;
using System.Collections.Generic;
using System.Text;

class TrieNode
{
    public Dictionary<char, TrieNode> Children = new();
    public bool IsWord;
}

class Trie
{
    private readonly TrieNode _root = new();

    public void Insert(string word)
    {
        var node = _root;
        foreach (var ch in word)
            node = node.Children.TryGetValue(ch, out var n) ? n : node.Children[ch] = new TrieNode();
        node.IsWord = true;
    }

    public List<string> GetWordsWithPrefix(string prefix)
    {
        var node = _root;
        foreach (var ch in prefix)
        {
            if (!node.Children.TryGetValue(ch, out node))
                return new List<string>();
        }
        var results = new List<string>();
        var sb = new StringBuilder(prefix);
        DFS(node, sb, results);
        return results;
    }

    private void DFS(TrieNode node, StringBuilder sb, List<string> outList)
    {
        if (node.IsWord) outList.Add(sb.ToString());
        foreach (var kv in node.Children)
        {
            sb.Append(kv.Key);
            DFS(kv.Value, sb, outList);
            sb.Length -= 1;
        }
    }

    // Example usage
    static void Main()
    {
        var trie = new Trie();
        trie.Insert("apple");
        trie.Insert("app");
        trie.Insert("apt");
        trie.Insert("bat");
        var matches = trie.GetWordsWithPrefix("ap");
        Console.WriteLine(string.Join(", ", matches)); // app, apple, apt (order depends on children enumeration)
    }
}