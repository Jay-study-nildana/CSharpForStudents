using Jay.LearningHelperForStudents.Utilities;
using System.Text.Json;

var lh = new Lh();

 
var orders = new List<(int Id, string Status)>
 {
     (101, "ok"),
     (102, "missing"),
     (103, "db_error"),
     (104, "ok"),
     (105, "serialize_test"),
     (169,"some_unknown_status")
 };

foreach (var order in orders)
{
    try
    {
        ProcessOrder(order.Id, order.Status);
        Console.WriteLine($"Order {order.Id} processed successfully.");
    }
    catch (OrderProcessingException ex)
    {
        Console.WriteLine($"Order {ex.OrderId} failed: {ex.Message}");
        if (ex.InnerException != null)
            Console.WriteLine($"  Inner: {ex.InnerException.Message}");
    }

    lh.AddSimpleConsoleDivider();
}

// Simulate order processing with different failure scenarios
void ProcessOrder(int orderId, string status)
{
    switch (status)
    {
        case "ok":
            // Success
            break;
        case "missing":
            throw new OrderProcessingException(orderId, $"Order {orderId} not found.");
        case "db_error":
            try
            {
                // Simulate DB error
                throw new InvalidOperationException("Database connection lost.");
            }
            catch (Exception inner)
            {
                throw new OrderProcessingException(orderId, $"Order {orderId} failed due to DB error.", inner);
            }
        case "serialize_test":
            var ex = new OrderProcessingException(orderId, $"Order {orderId} failed and will be serialized.");
            // Serialize exception data (not the exception object itself)
            var dto = new OrderProcessingExceptionDto
            {
                OrderId = ex.OrderId,
                Message = ex.Message
            };
            string json = JsonSerializer.Serialize(dto);
            Console.WriteLine($"Serialized exception data: {json}");
            // Simulate throwing after serialization
            throw ex;
        default:
            throw new OrderProcessingException($"Unknown error for order {orderId}.");
    }
}

// DTO for serializing exception data
public class OrderProcessingExceptionDto
{
    public int OrderId { get; set; }
    public string? Message { get; set; }
}

// Custom exception class
public class OrderProcessingException : Exception
{
    public int OrderId { get; }

    public OrderProcessingException() { }

    public OrderProcessingException(string message) : base(message) { }

    public OrderProcessingException(string message, Exception inner) : base(message, inner) { }

    public OrderProcessingException(int orderId, string message, Exception? inner = null)
        : base(message, inner)
    {
        OrderId = orderId;
    }
}