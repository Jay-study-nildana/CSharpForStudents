using LibraryManagement.Core.Interfaces;

namespace LibraryManagement.CLI.Menus;

/// <summary>Sub-menu for all loan-related operations (borrow, return, view).</summary>
public sealed class LoanMenu
{
    private readonly ILibraryService _service;

    public LoanMenu(ILibraryService service) => _service = service;

    public async Task RunAsync()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("─── LOANS ───────────────────────────────");
            Console.WriteLine("  1. View active loans");
            Console.WriteLine("  2. View overdue loans");
            Console.WriteLine("  3. Borrow a book");
            Console.WriteLine("  4. Return a book");
            Console.WriteLine("  5. View loans by member");
            Console.WriteLine("  0. Back");
            Console.WriteLine("─────────────────────────────────────────");
            Console.Write("Select: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": await ViewActiveAsync();        break;
                case "2": await ViewOverdueAsync();       break;
                case "3": await BorrowAsync();            break;
                case "4": await ReturnAsync();            break;
                case "5": await ViewByMemberAsync();      break;
                case "0": return;
                default:  Console.WriteLine("Invalid option."); break;
            }
        }
    }

    // ── Actions ───────────────────────────────────────────────────────────────

    private async Task ViewActiveAsync()
    {
        // Uses the enriched DTO so the user sees Book titles and Member names (Day 26)
        var summaries = (await _service.GetActiveLoanSummariesAsync()).ToList();
        if (summaries.Count == 0) { Console.WriteLine("No active loans."); return; }

        Console.WriteLine();
        Console.WriteLine($"{"#",-4} {"Book Title",-30} {"Member",-22} {"Borrowed",-12} {"Due",-12} {"Loan ID"}");
        Console.WriteLine(new string('─', 110));
        for (int i = 0; i < summaries.Count; i++)
        {
            var s = summaries[i];
            Console.ForegroundColor = s.IsOverdue ? ConsoleColor.Red : ConsoleColor.White;
            Console.WriteLine(
                $"{i + 1,-4} {Trunc(s.BookTitle, 30),-30} {Trunc(s.MemberName, 22),-22} " +
                $"{s.BorrowedDate:yyyy-MM-dd,-12} {s.DueDate:yyyy-MM-dd,-12} {s.LoanId}");
            Console.ResetColor();
        }
    }

    private async Task ViewOverdueAsync()
    {
        var summaries = (await _service.GetOverdueLoanSummariesAsync()).ToList();
        if (summaries.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("No overdue loans. Great job!");
            Console.ResetColor();
            return;
        }

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n  {summaries.Count} overdue loan(s):");
        Console.ResetColor();

        foreach (var s in summaries)
        {
            var daysOver = (int)(DateTime.UtcNow - s.DueDate).TotalDays;
            Console.WriteLine(
                $"  Loan {s.LoanId}");
            Console.WriteLine(
                $"    Book  : {s.BookTitle}");
            Console.WriteLine(
                $"    Member: {s.MemberName}");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(
                $"    Due   : {s.DueDate:yyyy-MM-dd}  ({daysOver} day(s) overdue)");
            Console.ResetColor();
            Console.WriteLine();
        }
    }

    private async Task BorrowAsync()
    {
        Console.Write("Book ID   : ");
        if (!Guid.TryParse(Console.ReadLine(), out var bookId))
        {
            Console.WriteLine("Invalid Book ID.");
            return;
        }

        Console.Write("Member ID : ");
        if (!Guid.TryParse(Console.ReadLine(), out var memberId))
        {
            Console.WriteLine("Invalid Member ID.");
            return;
        }

        try
        {
            var loan = await _service.BorrowBookAsync(bookId, memberId);
            Console.WriteLine($"  Loan created. ID: {loan.Id}  Due: {loan.DueDate:yyyy-MM-dd}");
        }
        catch (Exception ex) { PrintError(ex.Message); }
    }

    private async Task ReturnAsync()
    {
        Console.Write("Loan ID : ");
        if (!Guid.TryParse(Console.ReadLine(), out var loanId))
        {
            Console.WriteLine("Invalid Loan ID.");
            return;
        }

        try
        {
            var loan = await _service.ReturnBookAsync(loanId);
            Console.WriteLine($"  Book returned on {loan.ReturnedDate:yyyy-MM-dd}.");
        }
        catch (Exception ex) { PrintError(ex.Message); }
    }

    private async Task ViewByMemberAsync()
    {
        Console.Write("Member ID : ");
        if (!Guid.TryParse(Console.ReadLine(), out var memberId))
        {
            Console.WriteLine("Invalid Member ID.");
            return;
        }

        try
        {
            var loans = (await _service.GetLoansByMemberAsync(memberId)).ToList();
            if (loans.Count == 0) { Console.WriteLine("No active loans for this member."); return; }

            Console.WriteLine($"\n  {loans.Count} active loan(s):");
            foreach (var l in loans)
                Console.WriteLine(
                    $"  [{l.Id}] Book: {l.BookId}  Due: {l.DueDate:yyyy-MM-dd}  {l.Status}");
        }
        catch (Exception ex) { PrintError(ex.Message); }
    }

    private static string Trunc(string s, int max) =>
        s.Length <= max ? s : s[..(max - 1)] + "…";

    private static void PrintError(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"  Error: {msg}");
        Console.ResetColor();
    }
}
