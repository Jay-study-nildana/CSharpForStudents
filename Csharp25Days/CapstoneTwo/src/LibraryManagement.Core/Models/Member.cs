namespace LibraryManagement.Core.Models;

/// <summary>
/// Represents a library member who can borrow books.
/// </summary>
public class Member
{
    // --- Identity (immutable after creation) ---
    public Guid Id { get; init; }
    public DateTime MembershipDate { get; init; }

    // --- Mutable profile fields ---
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    /// <summary>Maximum number of books this member may have on loan simultaneously.</summary>
    public int MaxLoansAllowed { get; set; } = 3;

    public Member(string name, string email)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        MembershipDate = DateTime.UtcNow;
    }

    /// <summary>Parameterless constructor required for JSON deserialization.</summary>
    public Member() { }

    public override string ToString() =>
        $"[{Id}] {Name} | {Email} | Member since {MembershipDate:yyyy-MM-dd}";
}
