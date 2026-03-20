using System;
using System.Collections.Generic;

class PaymentProcessor_Strategy
{
    // Base payment method: polymorphic processing
    public abstract class PaymentMethod
    {
        public abstract void Process(decimal amount);
    }

    public class CreditCardPayment : PaymentMethod
    {
        public override void Process(decimal amount) => Console.WriteLine($"Processing credit card for {amount:C}");
    }

    public class PayPalPayment : PaymentMethod
    {
        public override void Process(decimal amount) => Console.WriteLine($"Processing PayPal for {amount:C}");
    }

    public class BankTransferPayment : PaymentMethod
    {
        public override void Process(decimal amount) => Console.WriteLine($"Processing bank transfer for {amount:C}");
    }

    static void Main()
    {
        // Replace conditional logic with polymorphism
        var methods = new List<PaymentMethod>
        {
            new CreditCardPayment(),
            new PayPalPayment(),
            new BankTransferPayment()
        };

        decimal total = 123.45m;
        foreach (var m in methods) m.Process(total);

        // This design is open for extension (add new payment types) without modifying caller code.
    }
}