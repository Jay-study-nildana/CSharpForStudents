using System;

class DTOs_Should_Be_Immutable
{
    // ApiResponse as an immutable record
    public record ApiResponse(int StatusCode, string Message, object Payload);

    static void Main()
    {
        var r = new ApiResponse(200, "OK", new { Id = 1, Name = "Item" });
        Console.WriteLine(r);

        Console.WriteLine();
        Console.WriteLine("Which DTOs should be immutable?");
        Console.WriteLine("- External API messages (requests/responses): yes (predictable, safe to cache)");
        Console.WriteLine("- Value objects (Money, Point, Size): yes (value semantics)");
        Console.WriteLine("- Event payloads and messages sent across threads/queues: yes");
        Console.WriteLine("- Large domain entities with lifecycle: maybe no (entities often need mutability)");
    }
}