// EventPatternRefactor.cs
// Solution: implement standard event pattern with EventHandler<TEventArgs> and safe invocation.

using System;

namespace Day19.Solutions
{
    public class OrderPlacedEventArgs : EventArgs
    {
        public Guid OrderId { get; init; }
        public Guid UserId { get; init; }
        public DateTime PlacedAt { get; init; }
    }

    public class OrderService
    {
        public event EventHandler<OrderPlacedEventArgs>? OrderPlaced;

        protected virtual void OnOrderPlaced(OrderPlacedEventArgs e)
        {
            // snapshot for thread-safety
            var handler = OrderPlaced;
            handler?.Invoke(this, e);
        }

        public void PlaceOrder(Guid orderId, Guid userId)
        {
            // business logic here...
            OnOrderPlaced(new OrderPlacedEventArgs { OrderId = orderId, UserId = userId, PlacedAt = DateTime.UtcNow });
        }
    }

    public static class EventPatternRefactor
    {
        public static void Run()
        {
            var svc = new OrderService();
            EventHandler<OrderPlacedEventArgs> emailHandler = (s, e) =>
                Console.WriteLine($"Emailing user {e.UserId} about order {e.OrderId}");
            svc.OrderPlaced += emailHandler;

            svc.PlaceOrder(Guid.NewGuid(), Guid.NewGuid());

            svc.OrderPlaced -= emailHandler; // unsubscribe to avoid leaks
        }
    }
}