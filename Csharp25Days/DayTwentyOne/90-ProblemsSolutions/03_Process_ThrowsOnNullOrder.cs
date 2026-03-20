using System;
using Moq;
using Xunit;

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

public class OrderProcessorTests_03
{
    [Fact]
    public void Process_NullOrder_ThrowsAndDoesNotCallDependencies()
    {
        // Arrange
        var mockPayment = new Mock<IPaymentGateway>();
        var mockRepo = new Mock<IOrderRepository>();
        var mockEmail = new Mock<IEmailSender>();
        var sut = new OrderProcessor(mockPayment.Object, mockRepo.Object, mockEmail.Object);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => sut.Process(null, "token"));

        // Verify no dependency calls
        mockPayment.Verify(p => p.Charge(It.IsAny<string>(), It.IsAny<decimal>()), Times.Never);
        mockRepo.Verify(r => r.Save(It.IsAny<Order>()), Times.Never);
        mockEmail.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void Process_EmptyCardToken_ThrowsArgumentException()
    {
        // Arrange
        var mockPayment = new Mock<IPaymentGateway>();
        var mockRepo = new Mock<IOrderRepository>();
        var mockEmail = new Mock<IEmailSender>();
        var sut = new OrderProcessor(mockPayment.Object, mockRepo.Object, mockEmail.Object);
        var order = new Order { Id = 1, Total = 10m, CustomerEmail = "a@b.com" };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => sut.Process(order, ""));
        mockPayment.Verify(p => p.Charge(It.IsAny<string>(), It.IsAny<decimal>()), Times.Never);
    }
}