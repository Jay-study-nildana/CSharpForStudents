// 01-Adapter_LegacyPayment.cs
// Intent: Adapter to make a legacy payment processor conform to IPaymentGateway.
// DI/Lifetime: Register adapter as Transient/Scoped depending on the legacy processor lifetime; prefer Transient if adapter is stateless.
// Testability: Unit-test adapter by providing a test double for the LegacyPaymentProcessor or by subclassing to simulate responses.

using System;

public interface IPaymentGateway
{
    bool Charge(decimal amount, string currency);
}

// Adaptee: legacy third-party API (cannot be changed)
public class LegacyPaymentProcessor
{
    // Processes amount in cents and returns "OK" or an error code.
    public string ProcessPayment(int cents, string currencyCode)
    {
        // Simulated behavior:
        if (cents <= 0) return "ERR_INVALID_AMOUNT";
        if (currencyCode != "USD" && currencyCode != "EUR") return "ERR_UNSUPPORTED_CURRENCY";
        return "OK";
    }
}

// Adapter: implements IPaymentGateway and delegates to the LegacyPaymentProcessor
public class LegacyPaymentAdapter : IPaymentGateway
{
    private readonly LegacyPaymentProcessor _processor;
    public LegacyPaymentAdapter(LegacyPaymentProcessor processor) => _processor = processor;

    public bool Charge(decimal amount, string currency)
    {
        // Convert dollars to cents and call legacy API
        var cents = (int)(amount * 100m);
        var result = _processor.ProcessPayment(cents, currency);
        return result == "OK";
    }
}

// Example client using the adapter
public class CheckoutService
{
    private readonly IPaymentGateway _gateway;
    public CheckoutService(IPaymentGateway gateway) => _gateway = gateway;

    public bool Checkout(decimal amount)
    {
        return _gateway.Charge(amount, "USD");
    }
}

/*
Unit-test note (conceptual):
- Create a test double for LegacyPaymentProcessor that returns specific strings.
- Inject the fake processor into LegacyPaymentAdapter and assert Charge returns expected boolean for given inputs.
*/