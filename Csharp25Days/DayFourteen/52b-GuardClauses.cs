using Jay.LearningHelperForStudents.Utilities;
using System.Text.Json;

var lh = new Lh();
 
 try
{
    // NotNull: Valid
    var name = "Alice";
    var result = Guard.NotNull(name, nameof(name));
    Console.WriteLine($"NotNull passed: {result}");

    // NotNull: Invalid
    string? nullName = null;
    Guard.NotNull(nullName, nameof(nullName));
}
catch (ArgumentNullException ex)
{
    Console.WriteLine($"Caught ArgumentNullException: {ex.ParamName}");
}

try
{
    // NotNullOrWhiteSpace: Valid
    var city = "New York";
    Guard.NotNullOrWhiteSpace(city, nameof(city));
    Console.WriteLine("NotNullOrWhiteSpace passed: " + city);

    // NotNullOrWhiteSpace: Invalid (empty)
    var empty = "";
    Guard.NotNullOrWhiteSpace(empty, nameof(empty));
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Caught ArgumentException: {ex.ParamName} - {ex.Message}");
}

try
{
    // InRange: Valid
    int age = 25;
    Guard.InRange(age, 18, 65, nameof(age));
    Console.WriteLine("InRange passed: " + age);

    // InRange: Invalid (too low)
    int tooYoung = 10;
    Guard.InRange(tooYoung, 18, 65, nameof(tooYoung));
}
catch (ArgumentOutOfRangeException ex)
{
    Console.WriteLine($"Caught ArgumentOutOfRangeException: {ex.ParamName}");
}

// Guard class as provided
public static class Guard
{
    public static T NotNull<T>(T? value, string paramName)
        where T : class
    {
        if (value is null) throw new ArgumentNullException(paramName);
        return value;
    }

    public static void NotNullOrWhiteSpace(string? s, string paramName)
    {
        if (string.IsNullOrWhiteSpace(s)) throw new ArgumentException("Value is required", paramName);
    }

    public static void InRange(int value, int min, int max, string paramName)
    {
        if (value < min || value > max) throw new ArgumentOutOfRangeException(paramName);
    }
}