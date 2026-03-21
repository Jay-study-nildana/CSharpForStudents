namespace SmartStore.Core.Interfaces;

// -------------------------------------------------------
// BRIDGE PATTERN — Implementation interface
// -------------------------------------------------------
// Represents the delivery mechanism (Console, Email, SMS, ...).
// The abstraction side (OrderNotification hierarchy) uses this interface
// so the two hierarchies can vary independently.
// -------------------------------------------------------
public interface INotificationChannel
{
    string ChannelName { get; }
    void Send(string recipient, string subject, string message);
}
