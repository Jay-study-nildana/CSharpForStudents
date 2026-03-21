namespace SmartStore.Patterns.Structural;

// ================================================================
// ADAPTER PATTERN
// ================================================================
// The legacy payment processor has an incompatible API (InitiateCharge).
// LegacyPaymentAdapter wraps it to satisfy IPaymentGateway.
//
// Intent   : Convert the interface of a class into another interface
//            clients expect.
// Problem  : We must use LegacyPaymentProcessor but our system expects
//            IPaymentGateway — the interfaces are incompatible.
// Solution : The adapter implements IPaymentGateway and translates calls
//            to the legacy format internally.
// ================================================================

/// <summary>
/// Simulates a third-party legacy payment system with an incompatible interface.
/// This class cannot be changed (imagine it's a compiled DLL).
/// </summary>
public class LegacyPaymentProcessor
{
    /// <summary>Charges clientCode the given amount in cents.</summary>
    public string InitiateCharge(string clientCode, int amountCents)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"  [LegacyProcessor] Charging client '{clientCode}' for {amountCents} cents...");
        Console.ResetColor();
        return amountCents > 0 ? "APPROVED" : "DECLINED";
    }
}

/// <summary>
/// ADAPTER: Wraps LegacyPaymentProcessor and exposes IPaymentGateway.
/// </summary>
public class LegacyPaymentAdapter : IPaymentGateway
{
    private readonly LegacyPaymentProcessor _legacy = new();

    public bool ProcessPayment(string customerId, decimal amount)
    {
        // Translate: decimal dollars → integer cents, customerId stays the same
        var result = _legacy.InitiateCharge(customerId, (int)(amount * 100));
        return result == "APPROVED";
    }
}
