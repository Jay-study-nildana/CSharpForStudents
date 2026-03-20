using Xunit;
using Moq;
using ComicBookShop.Core.Entities;
using ComicBookShop.Core.Enums;
using ComicBookShop.Core.Events;
using ComicBookShop.Core.Exceptions;
using ComicBookShop.Core.Interfaces;
using ComicBookShop.Core.Services;

namespace ComicBookShop.Tests.Unit;

/// <summary>Unit tests for OrderService (Days 21, 22).</summary>
public class OrderServiceTests
{
    private readonly Mock<IRepository<Order>> _orderRepo;
    private readonly Mock<IRepository<ComicBook>> _comicRepo;
    private readonly Mock<IRepository<Customer>> _customerRepo;
    private readonly Mock<IAppLogger> _logger;
    private readonly InventoryManager _inventoryManager;
    private readonly OrderService _service;

    public OrderServiceTests()
    {
        _orderRepo = new Mock<IRepository<Order>>();
        _comicRepo = new Mock<IRepository<ComicBook>>();
        _customerRepo = new Mock<IRepository<Customer>>();
        _logger = new Mock<IAppLogger>();

        _inventoryManager = new InventoryManager(_comicRepo.Object, _logger.Object, 5);

        _service = new OrderService(
            _orderRepo.Object,
            _comicRepo.Object,
            _customerRepo.Object,
            _inventoryManager,
            _logger.Object);
    }

    [Fact]
    public async Task PlaceOrderAsync_ValidOrder_ReturnsReceipt()
    {
        var customer = SampleCustomer();
        var comic = SampleComic();

        _customerRepo.Setup(r => r.GetByIdAsync(customer.Id)).ReturnsAsync(customer);
        _comicRepo.Setup(r => r.GetByIdAsync(comic.Id)).ReturnsAsync(comic);
        _comicRepo.Setup(r => r.UpdateAsync(It.IsAny<ComicBook>())).Returns(Task.CompletedTask);
        _customerRepo.Setup(r => r.UpdateAsync(It.IsAny<Customer>())).Returns(Task.CompletedTask);
        _orderRepo.Setup(r => r.AddAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

        var items = new List<(Guid, int)> { (comic.Id, 2) };
        var receipt = await _service.PlaceOrderAsync(customer.Id, items);

        Assert.Equal(customer.FullName, receipt.CustomerName);
        Assert.Single(receipt.Lines);
        Assert.Equal(2, receipt.Lines[0].Quantity);
        Assert.Equal(comic.Price * 2, receipt.Subtotal);
    }

    [Fact]
    public async Task PlaceOrderAsync_AppliesGoldDiscount()
    {
        var customer = SampleCustomer();
        customer.Membership = MembershipTier.Gold; // 10% discount
        var comic = SampleComic();
        comic.Price = 100m;

        _customerRepo.Setup(r => r.GetByIdAsync(customer.Id)).ReturnsAsync(customer);
        _comicRepo.Setup(r => r.GetByIdAsync(comic.Id)).ReturnsAsync(comic);
        _comicRepo.Setup(r => r.UpdateAsync(It.IsAny<ComicBook>())).Returns(Task.CompletedTask);
        _customerRepo.Setup(r => r.UpdateAsync(It.IsAny<Customer>())).Returns(Task.CompletedTask);
        _orderRepo.Setup(r => r.AddAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

        var items = new List<(Guid, int)> { (comic.Id, 1) };
        var receipt = await _service.PlaceOrderAsync(customer.Id, items);

        Assert.Equal(100m, receipt.Subtotal);
        Assert.Equal(10m, receipt.Discount);
        Assert.Equal(90m, receipt.Total);
    }

    [Fact]
    public async Task PlaceOrderAsync_EmptyItems_ThrowsValidation()
    {
        var items = new List<(Guid, int)>();
        await Assert.ThrowsAsync<ValidationException>(
            () => _service.PlaceOrderAsync(Guid.NewGuid(), items));
    }

    [Fact]
    public async Task PlaceOrderAsync_CustomerNotFound_ThrowsEntityNotFound()
    {
        _customerRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Customer?)null);

        var items = new List<(Guid, int)> { (Guid.NewGuid(), 1) };
        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _service.PlaceOrderAsync(Guid.NewGuid(), items));
    }

    [Fact]
    public async Task PlaceOrderAsync_InsufficientStock_Throws()
    {
        var customer = SampleCustomer();
        var comic = SampleComic();
        comic.StockQuantity = 1;

        _customerRepo.Setup(r => r.GetByIdAsync(customer.Id)).ReturnsAsync(customer);
        _comicRepo.Setup(r => r.GetByIdAsync(comic.Id)).ReturnsAsync(comic);

        var items = new List<(Guid, int)> { (comic.Id, 5) };
        await Assert.ThrowsAsync<InsufficientStockException>(
            () => _service.PlaceOrderAsync(customer.Id, items));
    }

    [Fact]
    public async Task PlaceOrderAsync_FiresOrderPlacedEvent()
    {
        var customer = SampleCustomer();
        var comic = SampleComic();

        _customerRepo.Setup(r => r.GetByIdAsync(customer.Id)).ReturnsAsync(customer);
        _comicRepo.Setup(r => r.GetByIdAsync(comic.Id)).ReturnsAsync(comic);
        _comicRepo.Setup(r => r.UpdateAsync(It.IsAny<ComicBook>())).Returns(Task.CompletedTask);
        _customerRepo.Setup(r => r.UpdateAsync(It.IsAny<Customer>())).Returns(Task.CompletedTask);
        _orderRepo.Setup(r => r.AddAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

        OrderPlacedEventArgs? firedArgs = null;
        _service.OrderPlaced += (_, e) => firedArgs = e;

        var items = new List<(Guid, int)> { (comic.Id, 1) };
        await _service.PlaceOrderAsync(customer.Id, items);

        Assert.NotNull(firedArgs);
        Assert.Equal(customer.FullName, firedArgs!.CustomerName);
    }

    [Fact]
    public async Task GetTopSellingComicsAsync_ReturnsOrdered()
    {
        var orders = new List<Order>
        {
            new()
            {
                Status = OrderStatus.Confirmed,
                Items = new()
                {
                    new() { ComicTitle = "Batman", Quantity = 3, Genre = Genre.Action },
                    new() { ComicTitle = "Spider-Man", Quantity = 1, Genre = Genre.Superhero }
                }
            },
            new()
            {
                Status = OrderStatus.Confirmed,
                Items = new()
                {
                    new() { ComicTitle = "Spider-Man", Quantity = 5, Genre = Genre.Superhero }
                }
            }
        };
        _orderRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(orders.AsReadOnly());

        var top = await _service.GetTopSellingComicsAsync(10);

        Assert.Equal("Spider-Man", top[0].Title);
        Assert.Equal(6, top[0].TotalSold);
    }

    [Fact]
    public async Task GetRevenueByGenreAsync_AggregatesCorrectly()
    {
        var orders = new List<Order>
        {
            new()
            {
                Status = OrderStatus.Confirmed,
                Items = new()
                {
                    new() { ComicTitle = "A", Genre = Genre.Action, Quantity = 2, UnitPrice = 10m },
                    new() { ComicTitle = "B", Genre = Genre.Horror, Quantity = 1, UnitPrice = 15m },
                }
            }
        };
        _orderRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(orders.AsReadOnly());

        var rev = await _service.GetRevenueByGenreAsync();

        Assert.Equal(20m, rev[Genre.Action]);
        Assert.Equal(15m, rev[Genre.Horror]);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    private static Customer SampleCustomer() => new()
    {
        FirstName = "Bruce", LastName = "Wayne",
        Email = "bruce@example.com", Membership = MembershipTier.Bronze
    };

    private static ComicBook SampleComic() => new()
    {
        Title = "Batman", Author = "Frank Miller", Genre = Genre.Action,
        Price = 14.99m, StockQuantity = 20
    };
}
