// AsyncEventHandlers.cs
// Solution: implement async event pattern and await subscribers.

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Day19.Solutions
{
    public delegate Task AsyncEventHandler<TEventArgs>(object? sender, TEventArgs e);

    public class OrderPlacedEventArgs { public Guid OrderId { get; set; } }

    public class AsyncOrderService
    {
        public event AsyncEventHandler<OrderPlacedEventArgs>? OrderPlacedAsync;

        protected virtual async Task OnOrderPlacedAsync(OrderPlacedEventArgs e)
        {
            var handler = OrderPlacedAsync;
            if (handler == null) return;

            // Invoke in parallel and wait for all to complete
            var handlers = handler.GetInvocationList()
                .Cast<AsyncEventHandler<OrderPlacedEventArgs>>()
                .Select(h => h(this, e));
            await Task.WhenAll(handlers);
        }

        public async Task PlaceOrderAsync(Guid orderId)
        {
            // business logic...
            await OnOrderPlacedAsync(new OrderPlacedEventArgs { OrderId = orderId });
        }
    }

    public static class AsyncEventHandlers
    {
        public static async Task Run()
        {
            var svc = new AsyncOrderService();

            svc.OrderPlacedAsync += async (s, e) =>
            {
                await Task.Delay(50); // simulate I/O
                Console.WriteLine($"Async email for {e.OrderId}");
            };

            svc.OrderPlacedAsync += async (s, e) =>
            {
                await Task.Delay(10);
                Console.WriteLine($"Async audit for {e.OrderId}");
            };

            await svc.PlaceOrderAsync(Guid.NewGuid());
        }
    }
}