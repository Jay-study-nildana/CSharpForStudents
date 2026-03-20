namespace LibraryManagement.CLI;

/// <summary>
/// Immutable configuration record loaded from appsettings.json.
/// Demonstrates record types (Day 10) and configuration separation (Day 18).
/// </summary>
public record AppSettings
{
    public string DataDirectory          { get; init; } = "data";
    public string LogDirectory           { get; init; } = "logs";
    public int    DefaultLoanDurationDays { get; init; } = 14;
    public int    MaxLoansPerMember       { get; init; } = 3;
}
