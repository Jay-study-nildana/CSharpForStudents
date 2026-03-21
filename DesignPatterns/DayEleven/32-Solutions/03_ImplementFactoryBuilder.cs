using System;

//
// Problem: Implement Factory/Builder
// Test plan: build complex Invoice via builder and factory; assert required fields are set.
// Demonstrates: separation of construction concerns.
//

namespace Day11.RefactorLab
{
    public class Order { public int Id; public string Item; public decimal Price; public int Qty; }

    public class Invoice
    {
        public int OrderId;
        public decimal Subtotal;
        public decimal Tax;
        public decimal Total;
        public string Notes = "";
    }

    public interface IInvoiceBuilder
    {
        IInvoiceBuilder WithOrder(Order order);
        IInvoiceBuilder WithTax(decimal taxRate);
        Invoice Build();
    }

    public class InvoiceBuilder : IInvoiceBuilder
    {
        private readonly Invoice _inv = new();
        public IInvoiceBuilder WithOrder(Order order)
        {
            _inv.OrderId = order.Id;
            _inv.Subtotal = order.Price * order.Qty;
            return this;
        }

        public IInvoiceBuilder WithTax(decimal taxRate)
        {
            _inv.Tax = _inv.Subtotal * taxRate;
            _inv.Total = _inv.Subtotal + _inv.Tax;
            return this;
        }

        public Invoice Build() => _inv;
    }

    public interface IInvoiceFactory { Invoice Create(Order order); }

    public class DefaultInvoiceFactory : IInvoiceFactory
    {
        private readonly Func<IInvoiceBuilder> _builderFactory;
        private readonly decimal _taxRate;
        public DefaultInvoiceFactory(Func<IInvoiceBuilder> builderFactory, decimal taxRate = 0.1m)
        {
            _builderFactory = builderFactory; _taxRate = taxRate;
        }
        public Invoice Create(Order order) => _builderFactory().WithOrder(order).WithTax(_taxRate).Build();
    }

    class Program
    {
        static void Main()
        {
            var factory = new DefaultInvoiceFactory(() => new InvoiceBuilder(), 0.08m);
            var invoice = factory.Create(new Order { Id = 42, Item = "Gadget", Price = 19.99m, Qty = 2 });
            Console.WriteLine($"Invoice for order {invoice.OrderId}: Subtotal={invoice.Subtotal}, Tax={invoice.Tax}, Total={invoice.Total}");
        }
    }
}