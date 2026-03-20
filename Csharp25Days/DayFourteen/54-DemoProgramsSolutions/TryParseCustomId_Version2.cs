// TryParseCustomId.cs
// Problem: TryParseCustomId
// Implement OrderId struct with TryParse pattern. No exceptions used to signal invalid input.

using System;

readonly struct OrderId
{
    public Guid Value { get; }
    public OrderId(Guid value) => Value = value;
    public static bool TryParse(string? input, out OrderId id)
    {
        id = default;
        if (string.IsNullOrWhiteSpace(input)) return false;
        if (Guid.TryParse(input, out var g))
        {
            id = new OrderId(g);
            return true;
        }
        return false;
    }
    public override string ToString() => Value.ToString();
}

class TryParseCustomId
{
    static void Main()
    {
        var good = Guid.NewGuid().ToString();
        if (OrderId.TryParse(good, out var id)) Console.WriteLine("Parsed: " + id);
        else Console.WriteLine("Invalid id");

        if (OrderId.TryParse("not-a-guid", out var id2)) Console.WriteLine("Parsed: " + id2);
        else Console.WriteLine("Invalid id (expected)");
    }
}