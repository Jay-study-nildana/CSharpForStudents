namespace SmartStore.Core.Interfaces;

// -------------------------------------------------------
// ADAPTER PATTERN — Target interface
// -------------------------------------------------------
// The application depends on this abstraction.
// The LegacyPaymentAdapter adapts the incompatible legacy system to fit here.
// -------------------------------------------------------
public interface IPaymentGateway
{
    bool ProcessPayment(string customerId, decimal amount);
}
