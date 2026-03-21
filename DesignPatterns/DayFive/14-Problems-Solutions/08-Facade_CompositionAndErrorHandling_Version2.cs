// 08-Facade_CompositionAndErrorHandling.cs
// Intent: PaymentFacade centralizes payment, logging, retry logic, and error translation.
// DI/Lifetime: Facade Transient/Scoped; configure retry policy externally or via DI configuration.
// Testability: Inject fake gateway and logger to simulate failures and assert retry behavior.

using System;

public class PaymentResult
{
    public bool Success { get; set; }
    public string Error { get; set; }
}

public class PaymentFacade
{
    private readonly IPaymentGateway _gateway;
    private readonly ILogger _logger;
    private readonly int _maxAttempts;

    public PaymentFacade(IPaymentGateway gateway, ILogger logger, int maxAttempts = 3)
    {
        _gateway = gateway; _logger = logger; _maxAttempts = maxAttempts;
    }

    public PaymentResult ChargeWithRetry(decimal amount, string currency)
    {
        for (int attempt = 1; attempt <= _maxAttempts; attempt++)
        {
            try
            {
                _logger.Info($"Attempt {attempt} to charge {amount} {currency}");
                var ok = _gateway.Charge(amount, currency);
                if (ok)
                {
                    _logger.Info("Payment succeeded");
                    return new PaymentResult { Success = true };
                }
                else
                {
                    _logger.Error("Payment gateway declined the charge");
                    // Decide whether to retry on decline; here we do not retry on logical decline
                    return new PaymentResult { Success = false, Error = "Declined" };
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Payment attempt {attempt} failed: {ex.Message}");
                if (attempt == _maxAttempts)
                {
                    // translate to domain-friendly error
                    return new PaymentResult { Success = false, Error = "GatewayError" };
                }

                // simple backoff (in real code use async Task.Delay and jitter)
            }
        }

        return new PaymentResult { Success = false, Error = "Unknown" };
    }
}

/*
Why place retry here?
- The facade centralizes orchestration concerns (retries, logging, error translation) so clients get a predictable semantic result.
- Avoid scattering retry logic across callers; keep policy consistent.
*/