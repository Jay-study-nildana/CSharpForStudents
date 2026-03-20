using System;
using Moq;
using Xunit;

// Abstraction for time
public interface IClock { DateTime UtcNow { get; } }

// Simple order with discount logic
public class Order { public decimal Total { get; set; } public decimal Discount { get; set; } }
public class DiscountService
{
    private readonly IClock _clock;
    public DiscountService(IClock clock) { _clock = clock; }

    // Applies 10% discount if order placed before noon UTC
    public void ApplyDiscountIfEarly(Order order)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));
        var now = _clock.UtcNow;
        if (now.TimeOfDay < TimeSpan.FromHours(12))
        {
            order.Discount = Math.Round(order.Total * 0.10m, 2);
        }
        else
        {
            order.Discount = 0m;
        }
    }
}

// Tests
public class DiscountServiceTests
{
    [Fact]
    public void ApplyDiscountIfEarly_BeforeNoon_AppliesDiscount()
    {
        // Arrange
        var fakeClock = new Mock<IClock>();
        fakeClock.Setup(c => c.UtcNow).Returns(new DateTime(2026, 3, 20, 9, 0, 0, DateTimeKind.Utc));
        var svc = new DiscountService(fakeClock.Object);
        var order = new Order { Total = 100m };

        // Act
        svc.ApplyDiscountIfEarly(order);

        // Assert
        Assert.Equal(10.00m, order.Discount);
    }

    [Fact]
    public void ApplyDiscountIfEarly_AfterNoon_NoDiscount()
    {
        // Arrange
        var fakeClock = new Mock<IClock>();
        fakeClock.Setup(c => c.UtcNow).Returns(new DateTime(2026, 3, 20, 13, 0, 0, DateTimeKind.Utc));
        var svc = new DiscountService(fakeClock.Object);
        var order = new Order { Total = 100m };

        // Act
        svc.ApplyDiscountIfEarly(order);

        // Assert
        Assert.Equal(0m, order.Discount);
    }
}