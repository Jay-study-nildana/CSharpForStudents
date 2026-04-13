using Jay.LearningHelperForStudents.Utilities;
using System.Text.Json;

var lh = new Lh();

// Simulate user input
var registrations = new[]
{
     new { Name = "Alice", Email = "alice@example.com", Age = 30 },
     new { Name = "", Email = "bob@example.com", Age = 17 },
     new { Name = "Charlie", Email = "invalid-email", Age = 25 },
     new { Name = "Dana", Email = "dana@example.com", Age = 150 }
 };

foreach (var reg in registrations)
{
    var result = ValidateRegistration(reg.Name, reg.Email, reg.Age);
    Console.WriteLine($"Validating: Name={reg.Name}, Email={reg.Email}, Age={reg.Age}");
    if (result.IsValid)
    {
        Console.WriteLine("  Registration is valid!");
    }
    else
    {
        Console.WriteLine("  Errors:");
        foreach (var error in result.Errors)
            Console.WriteLine("   - " + error);
    }
    Console.WriteLine();
}

// Validation logic using ValidationResult
ValidationResult ValidateRegistration(string name, string email, int age)
{
    var result = new ValidationResult();

    if (string.IsNullOrWhiteSpace(name))
    result.Errors.Add("Name is required.");

    if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
    result.Errors.Add("A valid email is required.");

    if (age < 18 || age > 120)
    result.Errors.Add("Age must be between 18 and 120.");

    return result;
}

// ValidationResult class
public class ValidationResult
{
    public bool IsValid => !Errors.Any();
    public List<string> Errors { get; } = new();
}