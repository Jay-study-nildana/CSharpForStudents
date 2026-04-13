// AdapterPatternDemo.cs
// Console demo of the Adapter Pattern for payment processing.

using System;

// Top-level statements
var legacyProcessor = new LegacyPaymentProcessor();
IPaymentGateway gateway = new LegacyPaymentAdapter(legacyProcessor);
var checkout = new CheckoutService(gateway);

while (true)
{
    Console.WriteLine("Enter amount to checkout (or 'exit' to quit):");
    var input = Console.ReadLine();
    if (input == "exit") break;
    if (!decimal.TryParse(input, out var amount))
    {
        Console.WriteLine("Invalid amount.");
        continue;
    }
    var success = checkout.Checkout(amount);
    Console.WriteLine(success ? "Payment successful!" : "Payment failed.");
}


public interface IPaymentGateway
{
    bool Charge(decimal amount, string currency);
}

// Adaptee: legacy third-party API with incompatible method
public class LegacyPaymentProcessor
{
    // Legacy method signature and semantics differ
    public string ProcessPayment(int cents, string currencyCode)
    {
        // returns "OK" or error code
        return "OK";
    }
}

// Adapter: converts IPaymentGateway calls into LegacyPaymentProcessor usage
public class LegacyPaymentAdapter : IPaymentGateway
{
    private readonly LegacyPaymentProcessor _processor;
    public LegacyPaymentAdapter(LegacyPaymentProcessor processor) => _processor = processor;

    public bool Charge(decimal amount, string currency)
    {
        // adapt decimal dollars to integer cents, map currency names, interpret legacy return.
        var cents = (int)(amount * 100m);
        var result = _processor.ProcessPayment(cents, currency);
        return result == "OK";
    }
}

// Usage (client code uses IPaymentGateway only)
public class CheckoutService
{
    private readonly IPaymentGateway _gateway;
    public CheckoutService(IPaymentGateway gateway) => _gateway = gateway;

    public bool Checkout(decimal amount) => _gateway.Charge(amount, "USD");
}

