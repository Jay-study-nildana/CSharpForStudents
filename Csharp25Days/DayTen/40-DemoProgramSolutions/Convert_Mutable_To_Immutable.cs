using System;

class Convert_Mutable_To_Immutable
{
    // Mutable DTO (legacy)
    public class MutableOrderDto
    {
        public int Id { get; set; }
        public decimal Total { get; set; }
    }

    // Immutable replacement using record
    public record OrderDto(int Id, decimal Total);

    static void Main()
    {
        var mutable = new MutableOrderDto { Id = 1, Total = 9.99m };
        mutable.Total = 19.99m; // mutation possible

        var immutable = new OrderDto(1, 9.99m);
        var updated = immutable with { Total = 19.99m }; // creates new instance

        Console.WriteLine($"Mutable after change: Id={mutable.Id}, Total={mutable.Total}");
        Console.WriteLine($"Immutable original: {immutable}, updated copy: {updated}");
        Console.WriteLine("Migration tip: prefer record DTOs for clarity and thread-safety; use 'with' to create modified copies.");
    }
}