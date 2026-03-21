using System;

//
// Problem: Create Facade for Subsystem
// Test plan: use facade to ProcessOrder (payment + save + notify) via single call; demo prints each step.
// Demonstrates: Facade simplifies complex subsystem interactions.
//

namespace Day11.RefactorLab
{
    public class Order { public int Id; public decimal Amount; public string Customer; }

    public interface IPayment { bool Pay(Order o); }
    public interface IRepository { void Save(Order o); }
    public interface INotifier { void Notify(Order o); }

    public class Payment : IPayment { public bool Pay(Order o) { Console.WriteLine("Payment processed"); return true; } }
    public class Repo : IRepository { public void Save(Order o) => Console.WriteLine("Order saved"); }
    public class Notifier : INotifier { public void Notify(Order o) => Console.WriteLine("Customer notified"); }

    // Facade
    public class OrderFacade
    {
        private readonly IPayment _payment;
        private readonly IRepository _repo;
        private readonly INotifier _notifier;
        public OrderFacade(IPayment p, IRepository r, INotifier n) { _payment = p; _repo = r; _notifier = n; }
        public void ProcessOrder(Order o)
        {
            if (_payment.Pay(o)) { _repo.Save(o); _notifier.Notify(o); }
        }
    }

    class Program
    {
        static void Main()
        {
            var facade = new OrderFacade(new Payment(), new Repo(), new Notifier());
            facade.ProcessOrder(new Order { Id = 1, Amount = 9.99m, Customer = "Bob" });
        }
    }
}