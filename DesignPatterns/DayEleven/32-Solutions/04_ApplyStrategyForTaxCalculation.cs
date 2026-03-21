using System;

//
// Problem: Apply Strategy for Tax Calculation
// Test plan: swap strategies at runtime; show different computed tax and totals.
// Demonstrates: ITaxStrategy injection and runtime swap.
//

namespace Day11.RefactorLab
{
    public class Invoice { public decimal Subtotal; public decimal Tax; public decimal Total; }

    public interface ITaxStrategy
    {
        decimal CalculateTax(decimal subtotal);
    }

    public class NoTaxStrategy : ITaxStrategy { public decimal CalculateTax(decimal subtotal) => 0m; }
    public class FixedRateTaxStrategy : ITaxStrategy
    {
        private readonly decimal _rate;
        public FixedRateTaxStrategy(decimal rate) => _rate = rate;
        public decimal CalculateTax(decimal subtotal) => subtotal * _rate;
    }

    public class BillingService
    {
        private ITaxStrategy _taxStrategy;
        public BillingService(ITaxStrategy taxStrategy) => _taxStrategy = taxStrategy;
        public void SetTaxStrategy(ITaxStrategy strat) => _taxStrategy = strat;
        public Invoice CreateInvoice(decimal subtotal)
        {
            var inv = new Invoice { Subtotal = subtotal };
            inv.Tax = _taxStrategy.CalculateTax(subtotal);
            inv.Total = inv.Subtotal + inv.Tax;
            return inv;
        }
    }

    class Program
    {
        static void Main()
        {
            var svc = new BillingService(new NoTaxStrategy());
            var inv1 = svc.CreateInvoice(100m);
            Console.WriteLine($"No tax: Total={inv1.Total}");

            svc.SetTaxStrategy(new FixedRateTaxStrategy(0.2m));
            var inv2 = svc.CreateInvoice(100m);
            Console.WriteLine($"20% tax: Total={inv2.Total}");
        }
    }
}