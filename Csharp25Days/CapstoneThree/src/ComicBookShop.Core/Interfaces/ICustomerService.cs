using ComicBookShop.Core.Entities;
using ComicBookShop.Core.Enums;

namespace ComicBookShop.Core.Interfaces;

/// <summary>Service contract for customer management.</summary>
public interface ICustomerService
{
    Task<Customer> AddCustomerAsync(Customer customer);
    Task<Customer> UpdateCustomerAsync(Customer customer);
    Task<Customer?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Customer>> GetAllAsync();
    Task<IReadOnlyList<Customer>> SearchByNameAsync(string name);
    Task UpgradeMembershipAsync(Guid customerId, MembershipTier newTier);
    Task<IReadOnlyList<Customer>> GetTopCustomersAsync(int count);
}
