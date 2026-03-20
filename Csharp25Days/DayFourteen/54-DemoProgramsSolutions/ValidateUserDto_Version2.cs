// ValidateUserDto.cs
// Problem: ValidateUserDto
// Implement ValidationResult and a validator for UserDto (non-throwing).
// Approach: return structured ValidationResult for expected user input errors.

using System;
using System.Collections.Generic;
using System.Linq;

record UserDto(string? Name, string? Email, int? Age);

class ValidationResult
{
    public List<string> Errors { get; } = new();
    public bool IsValid => Errors.Count == 0;
    public void Add(string msg) => Errors.Add(msg);
    public override string ToString() => string.Join("; ", Errors);
}

class ValidateUserDto
{
    public static ValidationResult ValidateUser(UserDto dto)
    {
        var r = new ValidationResult();
        if (string.IsNullOrWhiteSpace(dto.Name)) r.Add("Name is required.");
        if (string.IsNullOrWhiteSpace(dto.Email) || !dto.Email.Contains('@')) r.Add("Valid email required.");
        if (!dto.Age.HasValue) r.Add("Age is required.");
        else if (dto.Age < 0 || dto.Age > 120) r.Add("Age must be in range 0..120.");
        return r;
    }

    static void Main()
    {
        var dto = new UserDto("Bob", "invalid-email", 200);
        var result = ValidateUser(dto);
        if (!result.IsValid)
        {
            Console.WriteLine("Validation failed: " + result);
            // Controller would return 400 with errors
        }
        else
        {
            Console.WriteLine("Valid: proceed to create user");
        }
    }
}