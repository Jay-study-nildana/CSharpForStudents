using System;

class Immutable_Record_Money
{
    // Money as a record (reference record) - good for value semantics and equality
    public record Money(string Currency, decimal Amount);

    static void Main()
    {
        var m1 = new Money("USD", 19.99m);
        var m2 = m1 with { Amount = 25.00m }; // creates a new instance
        Console.WriteLine($"m1: {m1}, m2: {m2}");
        Console.WriteLine($"Equality: m1 == m2? {m1 == m2}");

        var m3 = new Money("USD", 19.99m);
        Console.WriteLine($"m1 == m3? {m1 == m3} (value equality)");

        Console.WriteLine("Immutability prevents accidental changes to monetary values and makes them safe to share.");
    }
}