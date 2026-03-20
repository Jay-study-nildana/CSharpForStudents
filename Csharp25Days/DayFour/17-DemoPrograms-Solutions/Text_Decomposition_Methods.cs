using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class Text_Decomposition_Methods
{
    // Decompose text processing into small methods
    // Normalize: lowercase, trim, remove punctuation
    // Tokenize: split on whitespace
    // CountWords: produce dictionary
    // WordFrequency: composes helpers
    // Time: O(n) where n = number of characters/tokens, Space: O(u) unique tokens

    static string Normalize(string s)
    {
        if (s == null) return string.Empty;
        s = s.ToLowerInvariant().Trim();
        // simple punctuation removal
        s = Regex.Replace(s, @"[^\w\s]", "");
        return s;
    }

    static string[] Tokenize(string s)
    {
        var norm = Normalize(s);
        if (string.IsNullOrWhiteSpace(norm)) return Array.Empty<string>();
        return norm.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
    }

    static Dictionary<string, int> CountWords(string[] tokens)
    {
        var d = new Dictionary<string, int>();
        foreach (var t in tokens)
        {
            if (d.ContainsKey(t)) d[t]++; else d[t] = 1;
        }
        return d;
    }

    static Dictionary<string, int> WordFrequency(string text)
    {
        var tokens = Tokenize(text);
        return CountWords(tokens);
    }

    static void Main()
    {
        string text = "Hello, world! Hello C# world.";
        var freq = WordFrequency(text);
        foreach (var kv in freq.OrderByDescending(kv => kv.Value))
            Console.WriteLine($"{kv.Key}: {kv.Value}");
    }
}