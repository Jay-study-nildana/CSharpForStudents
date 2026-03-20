using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

class Defensive_Copying_DTO
{
    // Immutable-like DTO that defensively copies collection inputs
    public class UserDto
    {
        public string Username { get; }
        private readonly IReadOnlyList<string> _roles;
        public IReadOnlyList<string> Roles => _roles;

        public UserDto(string username, IEnumerable<string> roles)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            // Defensive copy to prevent external mutation
            var list = new List<string>(roles ?? Array.Empty<string>());
            _roles = new ReadOnlyCollection<string>(list);
        }
    }

    static void Main()
    {
        var external = new List<string> { "User", "Admin" };
        var dto = new UserDto("bob", external);
        external[0] = "Hacker"; // should not affect dto
        Console.WriteLine($"External[0]={external[0]}, DTO.Roles[0]={dto.Roles[0]}");
        Console.WriteLine("Defensive copying prevents callers from mutating DTO internals.");
    }
}