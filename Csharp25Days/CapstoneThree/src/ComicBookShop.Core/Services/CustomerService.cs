using ComicBookShop.Core.Entities;
using ComicBookShop.Core.Enums;
using ComicBookShop.Core.Exceptions;
using ComicBookShop.Core.Interfaces;

namespace ComicBookShop.Core.Services;

/// <summary>
/// Business logic for customer management.
/// Demonstrates DI, LINQ, and validation (Days 13, 14, 17).
/// </summary>
public class CustomerService : ICustomerService
{
    private readonly IRepository<Customer> _repository;
    private readonly IAppLogger _logger;

    public CustomerService(IRepository<Customer> repository, IAppLogger logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Customer> AddCustomerAsync(Customer customer)
    {
        if (string.IsNullOrWhiteSpace(customer.FirstName))
            throw new ValidationException("First name is required.");
        if (string.IsNullOrWhiteSpace(customer.LastName))
            throw new ValidationException("Last name is required.");
        if (string.IsNullOrWhiteSpace(customer.Email) || !customer.Email.Contains('@'))
            throw new ValidationException("A valid email address is required.");

        await _repository.AddAsync(customer);
        _logger.LogInformation("Added customer: {Name} ({Email})", customer.FullName, customer.Email);
        return customer;
    }

    public async Task<Customer> UpdateCustomerAsync(Customer customer)
    {
        var existing = await _repository.GetByIdAsync(customer.Id)
            ?? throw new EntityNotFoundException(nameof(Customer), customer.Id);

        existing.FirstName = customer.FirstName;
        existing.LastName = customer.LastName;
        existing.Email = customer.Email;
        existing.Phone = customer.Phone;
        existing.MarkUpdated();

        await _repository.UpdateAsync(existing);
        _logger.LogInformation("Updated customer: {Name}", existing.FullName);
        return existing;
    }

    public async Task<Customer?> GetByIdAsync(Guid id) =>
        await _repository.GetByIdAsync(id);

    public async Task<IReadOnlyList<Customer>> GetAllAsync() =>
        await _repository.GetAllAsync();

    public async Task<IReadOnlyList<Customer>> SearchByNameAsync(string name)
    {
        var results = await _repository.FindAsync(c =>
            c.FullName.Contains(name, StringComparison.OrdinalIgnoreCase));
        return results.OrderBy(c => c.LastName).ToList().AsReadOnly();
    }

    public async Task UpgradeMembershipAsync(Guid customerId, MembershipTier newTier)
    {
        var customer = await _repository.GetByIdAsync(customerId)
            ?? throw new EntityNotFoundException(nameof(Customer), customerId);

        var oldTier = customer.Membership;
        customer.Membership = newTier;
        customer.MarkUpdated();

        await _repository.UpdateAsync(customer);
        _logger.LogInformation("Membership changed for {Name}: {Old} -> {New}",
            customer.FullName, oldTier, newTier);
    }

    public async Task<IReadOnlyList<Customer>> GetTopCustomersAsync(int count)
    {
        var all = await _repository.GetAllAsync();

        return all
            .OrderByDescending(c => c.TotalSpent)
            .Take(count)
            .ToList()
            .AsReadOnly();
    }
}
