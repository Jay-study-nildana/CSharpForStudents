// 05-ReplacePrimitiveWithType_EmailValidation.cs
// Introduced Email value object to validate and encapsulate email semantics.
using System;
using System.Text.RegularExpressions;

public readonly struct Email
{
    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
    public string Value { get; }
    public Email(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !EmailRegex.IsMatch(email))
            throw new ArgumentException("Invalid email", nameof(email));
        Value = email;
    }
    public override string ToString() => Value;
}

public class UserService
{
    public void RegisterUser(Email email)
    {
        // email is guaranteed valid here
        Console.WriteLine($"Registering {email}");
    }
}

// Usage:
// var email = new Email("alice@example.com");
// new UserService().RegisterUser(email);