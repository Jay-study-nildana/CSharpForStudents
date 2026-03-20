using System;

class Count_Evens_Odds
{
    // Count even and odd numbers
    // Control flow: foreach for clarity
    // Time: O(n), Space: O(1)
    static (int evens, int odds) Count(int[] numbers)
    {
        int e = 0, o = 0;
        foreach (int x in numbers)
        {
            if ((x & 1) == 0) e++;
            else o++;
        }
        return (e, o);
    }

    static void Main()
    {
        int[] a = { 1, 2, 3, 4, 5, 6 };
        var (ev, od) = Count(a);
        Console.WriteLine($"Evens: {ev}, Odds: {od}"); // Evens: 3, Odds: 3
    }
}