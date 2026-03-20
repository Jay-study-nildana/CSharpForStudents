using LibraryManagement.Core.Interfaces;
using LibraryManagement.Core.Models;

namespace LibraryManagement.CLI.Menus;

/// <summary>Sub-menu for all member-related operations.</summary>
public sealed class MemberMenu
{
    private readonly ILibraryService _service;

    public MemberMenu(ILibraryService service) => _service = service;

    public async Task RunAsync()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("─── MEMBERS ─────────────────────────────");
            Console.WriteLine("  1. List all members");
            Console.WriteLine("  2. Search members");
            Console.WriteLine("  3. Register new member");
            Console.WriteLine("  0. Back");
            Console.WriteLine("─────────────────────────────────────────");
            Console.Write("Select: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": await ListAllAsync();     break;
                case "2": await SearchAsync();      break;
                case "3": await RegisterAsync();    break;
                case "0": return;
                default:  Console.WriteLine("Invalid option."); break;
            }
        }
    }

    // ── Actions ───────────────────────────────────────────────────────────────

    private async Task ListAllAsync()
    {
        var members = (await _service.GetAllMembersAsync()).ToList();
        if (members.Count == 0) { Console.WriteLine("No members registered."); return; }

        Console.WriteLine();
        Console.WriteLine($"{"#",-4} {"Name",-28} {"Email",-32} {"Member Since",-14} {"ID"}");
        Console.WriteLine(new string('─', 110));
        for (int i = 0; i < members.Count; i++)
        {
            var m = members[i];
            Console.WriteLine(
                $"{i + 1,-4} {m.Name,-28} {m.Email,-32} {m.MembershipDate:yyyy-MM-dd,-14} {m.Id}");
        }
    }

    private async Task SearchAsync()
    {
        Console.Write("Search (name / email): ");
        var query = Console.ReadLine() ?? string.Empty;

        var members = (await _service.SearchMembersAsync(query)).ToList();
        if (members.Count == 0) { Console.WriteLine("No members matched."); return; }

        Console.WriteLine($"\n{members.Count} result(s):");
        foreach (var m in members)
            Console.WriteLine($"  [{m.Id}]  {m.Name}  |  {m.Email}  |  Joined {m.MembershipDate:yyyy-MM-dd}");
    }

    private async Task RegisterAsync()
    {
        Console.Write("Name  : "); var name  = Console.ReadLine() ?? string.Empty;
        Console.Write("Email : "); var email = Console.ReadLine() ?? string.Empty;

        try
        {
            var member = await _service.RegisterMemberAsync(name, email);
            Console.WriteLine($"  Member registered. ID: {member.Id}");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  Error: {ex.Message}");
            Console.ResetColor();
        }
    }
}
