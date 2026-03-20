// CustomExceptionWrapping.cs
// Problem: CustomExceptionWrapping
// Define OrderProcessingException and show wrapping of low-level exceptions preserving inner exception.

using System;
using System.IO;

readonly struct OrderId { public int Value; public OrderId(int v) => Value = v; public override string ToString() => Value.ToString(); }

[Serializable]
class OrderProcessingException : Exception
{
    public OrderId OrderId { get; }
    public OrderProcessingException() { }
    public OrderProcessingException(string message) : base(message) { }
    public OrderProcessingException(string message, Exception inner) : base(message, inner) { }
    public OrderProcessingException(OrderId id, string message, Exception? inner = null) : base(message, inner) => OrderId = id;
}

class CustomExceptionWrapping
{
    static void SaveOrder(OrderId id)
    {
        // Simulate underlying error
        throw new IOException("Disk not available");
    }

    static void Process(OrderId id)
    {
        try
        {
            SaveOrder(id);
        }
        catch (Exception ex) when (ex is IOException || ex is InvalidOperationException)
        {
            // Wrap with domain-specific exception, preserving original as InnerException
            throw new OrderProcessingException(id, $"Failed to persist order {id}", ex);
        }
    }

    static void Main()
    {
        try
        {
            Process(new OrderId(42));
        }
        catch (OrderProcessingException ex)
        {
            Console.WriteLine($"OrderProcessingException for {ex.OrderId}: {ex.Message}");
            Console.WriteLine($"Inner: {ex.InnerException?.GetType().Name}: {ex.InnerException?.Message}");
        }
    }
}