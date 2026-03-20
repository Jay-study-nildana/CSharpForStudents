using ComicBookShop.Core.Enums;

namespace ComicBookShop.Core.Entities;

/// <summary>
/// Represents a shop customer with loyalty membership.
/// Demonstrates encapsulation, computed properties, and switch expressions (Days 6, 3).
/// </summary>
public class Customer : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public MembershipTier Membership { get; set; } = MembershipTier.Bronze;
    public decimal TotalSpent { get; set; }

    /// <summary>Computed full name from first and last name.</summary>
    public string FullName => $"{FirstName} {LastName}";

    /// <summary>Returns the discount percentage based on membership tier (switch expression).</summary>
    public decimal GetDiscountPercentage() => Membership switch
    {
        MembershipTier.Silver   => 0.05m,
        MembershipTier.Gold     => 0.10m,
        MembershipTier.Platinum => 0.15m,
        _                       => 0m
    };

    public override string ToString() =>
        $"{FullName} ({Email}) - {Membership} Member - Total Spent: ${TotalSpent:F2}";
}
