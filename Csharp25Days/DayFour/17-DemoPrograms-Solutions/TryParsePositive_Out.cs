using System;

class TryParsePositive_Out
{
    // Custom TryParsePositive using out parameter
    // Time: O(m) where m = length of input string, Space: O(1)

    static bool TryParsePositive(string s, out int value)
    {
        value = 0;
        if (string.IsNullOrWhiteSpace(s)) return false;
        if (!int.TryParse(s.Trim(), out int tmp)) return false;
        if (tmp <= 0) return false;
        value = tmp;
        return true;
    }

    static void Main()
    {
        string[] tests = { "42", "-3", "abc", "  7 " };
        foreach (var t in tests)
        {
            if (TryParsePositive(t, out int v))
                Console.WriteLine($"Parsed positive {v} from '{t}'");
            else
                Console.WriteLine($"Failed to parse positive int from '{t}'");
        }
    }
}