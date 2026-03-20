using System;
using Moq;
using Xunit;

// Minimal production models & interfaces (same shapes as problem)
public class Order { public int Id { get; set; } public decimal Total { get; set; } public string CustomerEmail { get; set; } }
public interface IPaymentGateway { bool Charge(string cardToken, decimal amount); }
public interface IOrderRepository { void Save(Order order); }
public interface IEmailSender { void Send(string to, string subject, string body); }

public class OrderProcessor
{
    private readonly IPaymentGateway _payment;
    private readonly IOrderRepository _repo;
    private readonly IEmailSender _email;

    public OrderProcessor(IPaymentGateway payment, IOrderRepository repo, IEmailSender email)
    {
        _payment = payment ?? throw new ArgumentNullException(nameof(payment));
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        _email = email ?? throw new ArgumentNullException(nameof(email));
    }

    public bool Process(Order order, string cardToken)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));
        if (string.IsNullOrWhiteSpace(cardToken)) throw new ArgumentException("cardToken required");

        var charged = _payment.Charge(cardToken, order.Total);
        if (!charged)
        {
            _email.Send(order.CustomerEmail, "Payment failed", "Please update payment info.");
            return false;
        }

        _repo.Save(order);
        _email.Send(order.CustomerEmail, "Order received", "Thanks for your order.");
        return true;
    }
}

// Test
public class OrderProcessorTests_02
{
    [Fact]
    public void Process_PaymentFails_SendsFailureEmailAndDoesNotSave()
    {
        // Arrange
        var order = new Order { Id = 2, Total = 55m, CustomerEmail = "d@ex.com" };
        var mockPayment = new Mock<IPaymentGateway>();
        var mockRepo = new Mock<IOrderRepository>();
        var mockEmail = new Mock<IEmailSender>();

        mockPayment.Setup(p => p.Charge(It.IsAny<string>(), order.Total)).Returns(false);

        var sut = new OrderProcessor(mockPayment.Object, mockRepo.Object, mockEmail.Object);

        // Act
        var result = sut.Process(order, "token-bad");

        // Assert
        Assert.False(result);
        mockRepo.Verify(r => r.Save(It.IsAny<Order>()), Times.Never);
        mockEmail.Verify(e => e.Send(order.CustomerEmail, "Payment failed", It.IsAny<string>()), Times.Once);
    }
}