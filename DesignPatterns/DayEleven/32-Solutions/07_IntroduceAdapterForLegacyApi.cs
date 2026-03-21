using System;

//
// Problem: Introduce Adapter for Legacy API
// Test plan: adapter wraps legacy static API and exposes IPaymentGateway; demo uses adapter to charge.
// Demonstrates: Adapter isolates legacy API.
//

namespace Day11.RefactorLab
{
    // Legacy API (static / non-conformant)
    public static class LegacyPaymentSystem
    {
        public static bool MakePayment(string customer, int cents) => (cents % 2 == 0); // fake behavior
    }

    // Target interface
    public interface IPaymentGateway { bool Charge(string customer, decimal amount); }

    // Adapter: translate to legacy API
    public class LegacyPaymentAdapter : IPaymentGateway
    {
        public bool Charge(string customer, decimal amount)
        {
            var cents = (int)(amount * 100);
            return LegacyPaymentSystem.MakePayment(customer, cents);
        }
    }

    class Program
    {
        static void Main()
        {
            IPaymentGateway gateway = new LegacyPaymentAdapter();
            var ok = gateway.Charge("alice", 12.34m);
            Console.WriteLine($"Payment success: {ok}");
        }
    }
}