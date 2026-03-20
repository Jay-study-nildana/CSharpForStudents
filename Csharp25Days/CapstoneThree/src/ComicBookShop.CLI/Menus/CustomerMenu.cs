using ComicBookShop.CLI.Helpers;
using ComicBookShop.Core.Entities;
using ComicBookShop.Core.Enums;
using ComicBookShop.Core.Interfaces;

namespace ComicBookShop.CLI.Menus;

/// <summary>Console sub-menu for customer management.</summary>
public class CustomerMenu
{
    private readonly ICustomerService _service;

    public CustomerMenu(ICustomerService service)
    {
        _service = service;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            ConsoleHelper.PrintHeader("Customer Management");
            Console.WriteLine("  1. List All Customers");
            Console.WriteLine("  2. Add Customer");
            Console.WriteLine("  3. Update Membership");
            Console.WriteLine("  4. Search Customer by Name");
            Console.WriteLine("  5. View Top Customers");
            Console.WriteLine("  6. Back to Main Menu");

            switch (ConsoleHelper.GetMenuChoice("Select option", 1, 6))
            {
                case 1: await ListAllAsync(); break;
                case 2: await AddCustomerAsync(); break;
                case 3: await UpdateMembershipAsync(); break;
                case 4: await SearchAsync(); break;
                case 5: await TopCustomersAsync(); break;
                case 6: return;
            }
        }
    }

    private async Task ListAllAsync()
    {
        var customers = await _service.GetAllAsync();
        DisplayCustomerList(customers);
        ConsoleHelper.WaitForKey();
    }

    private async Task AddCustomerAsync()
    {
        ConsoleHelper.PrintSubHeader("Add New Customer");
        try
        {
            var customer = new Customer
            {
                FirstName = ConsoleHelper.GetRequiredInput("First name"),
                LastName  = ConsoleHelper.GetRequiredInput("Last name"),
                Email     = ConsoleHelper.GetRequiredInput("Email"),
                Phone     = ConsoleHelper.GetRequiredInput("Phone")
            };

            if (!InputValidator.IsValidEmail(customer.Email))
            {
                ConsoleHelper.PrintError("Invalid email format.");
                return;
            }

            await _service.AddCustomerAsync(customer);
            ConsoleHelper.PrintSuccess($"Added customer {customer.FullName} (ID: {customer.Id.ToString()[..8]})");
        }
        catch (Exception ex)
        {
            ConsoleHelper.PrintError(ex.Message);
        }
        ConsoleHelper.WaitForKey();
    }

    private async Task UpdateMembershipAsync()
    {
        var customers = await _service.GetAllAsync();
        if (customers.Count == 0) { ConsoleHelper.PrintInfo("No customers."); return; }

        DisplayCustomerList(customers);
        int idx = ConsoleHelper.GetMenuChoice("Select customer #", 1, customers.Count) - 1;
        var customer = customers[idx];

        ConsoleHelper.PrintInfo($"Current membership: {customer.Membership}");
        var newTier = ConsoleHelper.GetEnumChoice<MembershipTier>("New tier");

        try
        {
            await _service.UpgradeMembershipAsync(customer.Id, newTier);
            ConsoleHelper.PrintSuccess($"Membership updated to {newTier}.");
        }
        catch (Exception ex)
        {
            ConsoleHelper.PrintError(ex.Message);
        }
        ConsoleHelper.WaitForKey();
    }

    private async Task SearchAsync()
    {
        var name = ConsoleHelper.GetRequiredInput("Name to search");
        var results = await _service.SearchByNameAsync(name);
        Console.WriteLine($"  Found {results.Count} result(s):");
        DisplayCustomerList(results);
        ConsoleHelper.WaitForKey();
    }

    private async Task TopCustomersAsync()
    {
        int count = ConsoleHelper.GetIntInput("How many top customers?", 1, 50);
        var top = await _service.GetTopCustomersAsync(count);
        DisplayCustomerList(top);
        ConsoleHelper.WaitForKey();
    }

    private static void DisplayCustomerList(IReadOnlyList<Customer> customers)
    {
        var headers = new[] { "#", "Name", "Email", "Phone", "Membership", "Total Spent" };
        var rows = customers.Select((c, i) => new[]
        {
            (i + 1).ToString(),
            c.FullName,
            c.Email,
            c.Phone,
            c.Membership.ToString(),
            $"${c.TotalSpent:F2}"
        }).ToList();

        ConsoleHelper.PrintTable(headers, rows);
    }
}
