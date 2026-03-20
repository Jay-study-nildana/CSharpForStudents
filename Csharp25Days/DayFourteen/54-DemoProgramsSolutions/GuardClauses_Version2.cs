// GuardClauses.cs
// Problem: GuardClauses
// Implement Guard helper and demonstrate failing fast in constructor-like method.
// Approach: throw specific exceptions for programmer/API contract violations.

using System;

record User(string Name, string Email, int Age);

static class Guard
{
    public static T NotNull<T>(T? value, string paramName) where T : class
    {
        if (value is null) throw new ArgumentNullException(paramName);
        return value;
    }

    public static void NotNullOrWhiteSpace(string? value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Value is required", paramName);
    }

    public static void InRange(int value, int min, int max, string paramName)
    {
        if (value < min || value > max) throw new ArgumentOutOfRangeException(paramName, $"Value must be in range [{min},{max}]");
    }
}

class GuardClauses
{
    static User CreateUser(string? name, string? email, int age)
    {
        Guard.NotNullOrWhiteSpace(name, nameof(name));
        Guard.NotNullOrWhiteSpace(email, nameof(email));
        Guard.InRange(age, 0, 120, nameof(age));
        return new User(name!, email!, age);
    }

    static void Main()
    {
        try
        {
            var u = CreateUser("Alice", "alice@example.com", 30);
            Console.WriteLine(u);
            // This will fail fast with ArgumentException
            var bad = CreateUser("", "bob@example.com", 25);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed: {ex.GetType().Name} - {ex.Message}");
        }
    }
}