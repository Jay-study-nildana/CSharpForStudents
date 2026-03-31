using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

class Program
{
    // Entry point: usage examples:
    // 1) dotnet run             -> interactive input, default top K = 10
    // 2) dotnet run -- 5 text.txt -> read text.txt, show top 5 words
    // 3) type text.txt | dotnet run 3 -> pipe input, show top 3 words
    static void Main(string[] args)
    {
        try
        {
            int k = 10;
            string text;

            if (args.Length == 0)
            {
                // No args: read from stdin if redirected, otherwise interactive input
                if (Console.IsInputRedirected)
                {
                    text = Console.In.ReadToEnd();
                }
                else
                {
                    Console.WriteLine("Paste your text. Enter a blank line to finish:");
                    var lines = new List<string>();
                    while (true)
                    {
                        var line = Console.ReadLine();
                        if (line == null || line == string.Empty)
                            break;
                        lines.Add(line);
                    }
                    text = string.Join(Environment.NewLine, lines);
                }
            }
            else if (args.Length == 1)
            {
                // Single arg: either k or file path
                if (int.TryParse(args[0], out var parsedK))
                {
                    k = parsedK;
                    // Read interactively or from stdin
                    if (Console.IsInputRedirected)
                        text = Console.In.ReadToEnd();
                    else
                    {
                        Console.WriteLine("Paste your text. Enter a blank line to finish:");
                        var lines = new List<string>();
                        while (true)
                        {
                            var line = Console.ReadLine();
                            if (line == null || line == string.Empty)
                                break;
                            lines.Add(line);
                        }
                        text = string.Join(Environment.NewLine, lines);
                    }
                }
                else
                {
                    // treat arg as file path
                    text = File.ReadAllText(args[0]);
                }
            }
            else
            {
                // Two-or-more args: try to find k and filepath among them
                string possiblePath = null;
                int? parsedK = null;
                foreach (var a in args)
                {
                    if (parsedK == null && int.TryParse(a, out var pk))
                        parsedK = pk;
                    else if (possiblePath == null && File.Exists(a))
                        possiblePath = a;
                }

                if (possiblePath != null)
                {
                    text = File.ReadAllText(possiblePath);
                    if (parsedK.HasValue) k = parsedK.Value;
                }
                else
                {
                    // no file found; join all args as text
                    text = string.Join(' ', args);
                    if (parsedK.HasValue) k = parsedK.Value;
                }
            }

            // Tokenize words using a Unicode-aware regex and filter empties.
            // You can tweak the regex to include apostrophes, digits, etc.
            var matches = Regex.Matches(text ?? string.Empty, @"\p{L}[\p{L}\p{Nd}']*");
            var words = matches.Select(m => m.Value).Where(s => !string.IsNullOrEmpty(s));

            var counts = CountWords(words);
            var top = TopK(counts, k);

            Console.WriteLine();
            Console.WriteLine($"Top {k} words:");
            Console.WriteLine("----------------");
            foreach (var (word, count) in top)
            {
                Console.WriteLine($"{count,8}  {word}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }

    static Dictionary<string, int> CountWords(IEnumerable<string> words)
    {
        var counts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        foreach (var w in words)
        {
            if (string.IsNullOrWhiteSpace(w))
                continue;
            // normalize (optional)
            var key = w;
            // increment safely without relying on GetValueOrDefault extension
            if (counts.TryGetValue(key, out var v))
                counts[key] = v + 1;
            else
                counts[key] = 1;
        }
        return counts;
    }

    //"Top K" means "the K items with the highest (or lowest) score/priority" — e.g., the top 10 most frequent words, the top 5 products by sales, or the top 20 search results by relevance.Its significance is that it extracts the most important/interesting items from a large collection efficiently, and many analytics, UI, and ML tasks depend on getting those top items quickly and correctly.


    static List<(string word, int count)> TopK(Dictionary<string, int> counts, int k)
    {
        return counts.OrderByDescending(kv => kv.Value)
                     .ThenBy(kv => kv.Key, StringComparer.OrdinalIgnoreCase)
                     .Take(k)
                     .Select(kv => (kv.Key, kv.Value))
                     .ToList();
    }
}