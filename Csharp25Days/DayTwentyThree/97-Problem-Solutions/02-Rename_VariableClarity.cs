// 02-Rename_VariableClarity.cs
// Improved names: loyaltyDiscount instead of 'd'.
using System;

public class Customer { public int Id; public int LoyaltyPoints; }

public class DiscountService
{
    public int CalculateDiscount(Customer customer)
    {
        // placeholder for actual logic
        return customer.LoyaltyPoints / 10;
    }

    public void ShowDiscount(Customer customer)
    {
        int loyaltyDiscount = CalculateDiscount(customer);
        Console.WriteLine($"Discount for customer {customer.Id}: {loyaltyDiscount}");
    }
}