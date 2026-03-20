// 04-IntroduceParameterObject.cs
// Introduced UserCreationOptions to group related parameters.
using System;

public class UserCreationOptions
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
    public bool IsAdmin { get; init; }
    public DateTime BirthDate { get; init; }
}

public class UserService
{
    public void CreateUser(UserCreationOptions options)
    {
        if (options == null) throw new ArgumentNullException(nameof(options));
        // validate and create user using options
        // keeps signature short and groups related parameters
    }
}