using System;
using Moq;
using Xunit;

// Payment interface
public interface IPaymentGateway { bool Charge(string cardToken, decimal amount); }

// Retrier that will attempt N times before giving up
public class PaymentRetrier
{
    private readonly IPaymentGateway _gateway;
    private readonly int _maxAttempts;
    public PaymentRetrier(IPaymentGateway gateway, int maxAttempts = 3)
    {
        _gateway = gateway;
        _maxAttempts = Math.Max(1, maxAttempts);
    }

    public bool ChargeWithRetry(string token, decimal amount)
    {
        for (int attempt = 1; attempt <= _maxAttempts; attempt++)
        {
            try
            {
                if (_gateway.Charge(token, amount)) return true;
            }
            catch (TransientException)
            {
                // swallow and retry
            }
        }
        return false;
    }
}

// Example transient exception
public class TransientException : Exception { public TransientException(string msg) : base(msg) { } }

// Tests
public class PaymentRetrierTests
{
    [Fact]
    public void ChargeWithRetry_SucceedsAfterTransientFailures()
    {
        // Arrange
        var mockGateway = new Mock<IPaymentGateway>();
        // First two calls throw, third returns true
        mockGateway.SetupSequence(g => g.Charge("t", 10m))
            .Throws(new TransientException("timeout"))
            .Throws(new TransientException("timeout"))
            .Returns(true);

        var retrier = new PaymentRetrier(mockGateway.Object, maxAttempts: 5);

        // Act
        var result = retrier.ChargeWithRetry("t", 10m);

        // Assert
        Assert.True(result);
        mockGateway.Verify(g => g.Charge("t", 10m), Times.Exactly(3));
    }

    [Fact]
    public void ChargeWithRetry_FailsAfterMaxAttempts()
    {
        // Arrange
        var mockGateway = new Mock<IPaymentGateway>();
        mockGateway.Setup(g => g.Charge(It.IsAny<string>(), It.IsAny<decimal>())).Returns(false);
        var retrier = new PaymentRetrier(mockGateway.Object, maxAttempts: 3);

        // Act
        var result = retrier.ChargeWithRetry("t", 10m);

        // Assert
        Assert.False(result);
        mockGateway.Verify(g => g.Charge("t", 10m), Times.Exactly(3));
    }
}