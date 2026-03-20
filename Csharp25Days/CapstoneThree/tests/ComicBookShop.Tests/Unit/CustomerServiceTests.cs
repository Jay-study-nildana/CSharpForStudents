using Xunit;
using Moq;
using ComicBookShop.Core.Entities;
using ComicBookShop.Core.Enums;
using ComicBookShop.Core.Exceptions;
using ComicBookShop.Core.Interfaces;
using ComicBookShop.Core.Services;

namespace ComicBookShop.Tests.Unit;

/// <summary>Unit tests for CustomerService (Day 21).</summary>
public class CustomerServiceTests
{
    private readonly Mock<IRepository<Customer>> _mockRepo;
    private readonly Mock<IAppLogger> _mockLogger;
    private readonly CustomerService _service;

    public CustomerServiceTests()
    {
        _mockRepo = new Mock<IRepository<Customer>>();
        _mockLogger = new Mock<IAppLogger>();
        _service = new CustomerService(_mockRepo.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task AddCustomerAsync_ValidCustomer_AddsAndReturns()
    {
        var customer = CreateSample();
        _mockRepo.Setup(r => r.AddAsync(It.IsAny<Customer>())).Returns(Task.CompletedTask);

        var result = await _service.AddCustomerAsync(customer);

        Assert.Equal("Tony Stark", result.FullName);
        _mockRepo.Verify(r => r.AddAsync(customer), Times.Once);
    }

    [Fact]
    public async Task AddCustomerAsync_MissingFirstName_ThrowsValidation()
    {
        var customer = CreateSample();
        customer.FirstName = "";

        await Assert.ThrowsAsync<ValidationException>(() => _service.AddCustomerAsync(customer));
    }

    [Fact]
    public async Task AddCustomerAsync_InvalidEmail_ThrowsValidation()
    {
        var customer = CreateSample();
        customer.Email = "notanemail";

        await Assert.ThrowsAsync<ValidationException>(() => _service.AddCustomerAsync(customer));
    }

    [Fact]
    public async Task UpgradeMembershipAsync_ValidCustomer_ChangesTier()
    {
        var customer = CreateSample();
        _mockRepo.Setup(r => r.GetByIdAsync(customer.Id)).ReturnsAsync(customer);
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Customer>())).Returns(Task.CompletedTask);

        await _service.UpgradeMembershipAsync(customer.Id, MembershipTier.Gold);

        Assert.Equal(MembershipTier.Gold, customer.Membership);
    }

    [Fact]
    public async Task UpgradeMembershipAsync_NotFound_ThrowsEntityNotFound()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Customer?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _service.UpgradeMembershipAsync(Guid.NewGuid(), MembershipTier.Gold));
    }

    [Fact]
    public async Task SearchByNameAsync_FindsMatch()
    {
        var customers = SampleList();
        _mockRepo.Setup(r => r.FindAsync(It.IsAny<Func<Customer, bool>>()))
                 .ReturnsAsync((Func<Customer, bool> pred) =>
                     customers.Where(pred).ToList().AsReadOnly());

        var results = await _service.SearchByNameAsync("stark");
        Assert.Single(results);
    }

    [Fact]
    public async Task GetTopCustomersAsync_ReturnsOrderedBySpend()
    {
        var customers = SampleList();
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(customers.AsReadOnly());

        var top = await _service.GetTopCustomersAsync(2);

        Assert.Equal(2, top.Count);
        Assert.True(top[0].TotalSpent >= top[1].TotalSpent);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    private static Customer CreateSample() => new()
    {
        FirstName = "Tony", LastName = "Stark",
        Email = "tony@example.com", Phone = "555-0001",
        Membership = MembershipTier.Silver
    };

    private static List<Customer> SampleList() => new()
    {
        new() { FirstName = "Tony", LastName = "Stark", Email = "tony@example.com", TotalSpent = 500m },
        new() { FirstName = "Bruce", LastName = "Wayne", Email = "bruce@example.com", TotalSpent = 1200m },
        new() { FirstName = "Diana", LastName = "Prince", Email = "diana@example.com", TotalSpent = 300m },
    };
}
