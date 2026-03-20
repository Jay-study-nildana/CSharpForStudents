// 07-ReplaceIfElseWithPolymorphism_PaymentProcessor.cs
// Use IPaymentHandler implementations instead of if/else chains.
using System;
using System.Collections.Generic;

public enum PaymentType { CreditCard, PayPal, BankTransfer }
public class Payment { public PaymentType Type; public decimal Amount; /* other props */ }

public interface IPaymentHandler
{
    bool CanHandle(Payment payment);
    void Handle(Payment payment);
}

public class CreditCardHandler : IPaymentHandler
{
    public bool CanHandle(Payment payment) => payment.Type == PaymentType.CreditCard;
    public void Handle(Payment payment) => Console.WriteLine($"Processing credit card: {payment.Amount:C}");
}

public class PayPalHandler : IPaymentHandler
{
    public bool CanHandle(Payment payment) => payment.Type == PaymentType.PayPal;
    public void Handle(Payment payment) => Console.WriteLine($"Processing PayPal: {payment.Amount:C}");
}

public class BankTransferHandler : IPaymentHandler
{
    public bool CanHandle(Payment payment) => payment.Type == PaymentType.BankTransfer;
    public void Handle(Payment payment) => Console.WriteLine($"Processing bank transfer: {payment.Amount:C}");
}

public class PaymentProcessor
{
    private readonly IEnumerable<IPaymentHandler> _handlers;
    public PaymentProcessor(IEnumerable<IPaymentHandler> handlers) => _handlers = handlers;

    public void ProcessPayment(Payment payment)
    {
        foreach (var h in _handlers)
        {
            if (h.CanHandle(payment)) { h.Handle(payment); return; }
        }
        throw new InvalidOperationException("No handler for payment type");
    }
}