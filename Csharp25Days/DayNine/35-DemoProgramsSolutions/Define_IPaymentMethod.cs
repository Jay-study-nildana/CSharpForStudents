using System;
using System.Collections.Generic;

class Define_IPaymentMethod
{
    // Problem: Define IPaymentMethod and two implementations
    public interface IPaymentMethod
    {
        // Returns true on success
        bool Process(decimal amount);
    }

    public class CreditCardPayment : IPaymentMethod
    {
        public bool Process(decimal amount)
        {
            Console.WriteLine($"CreditCard: processing {amount:C}");
            return true;
        }
    }

    public class PayPalPayment : IPaymentMethod
    {
        public bool Process(decimal amount)
        {
            Console.WriteLine($"PayPal: processing {amount:C}");
            return true;
        }
    }

    static void Main()
    {
        List<IPaymentMethod> methods = new() { new CreditCardPayment(), new PayPalPayment() };
        foreach (var m in methods)
        {
            bool ok = m.Process(19.99m);
            Console.WriteLine($"Processed by {m.GetType().Name}: {ok}");
        }

        // Note: IPaymentMethod allows swapping implementations (testing/fakes).
    }
}