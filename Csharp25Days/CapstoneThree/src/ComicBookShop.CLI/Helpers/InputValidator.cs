namespace ComicBookShop.CLI.Helpers;

/// <summary>
/// Pure validation methods for user input.
/// Demonstrates static utility methods and defensive checks (Days 4, 14).
/// </summary>
public static class InputValidator
{
    public static bool IsValidEmail(string email) =>
        !string.IsNullOrWhiteSpace(email)
        && email.Contains('@')
        && email.Contains('.')
        && email.IndexOf('@') < email.LastIndexOf('.');

    public static bool IsValidPrice(string input, out decimal price)
    {
        price = 0;
        return decimal.TryParse(input, out price) && price > 0;
    }

    public static bool IsValidQuantity(string input, out int quantity)
    {
        quantity = 0;
        return int.TryParse(input, out quantity) && quantity > 0;
    }

    public static bool IsValidYear(string input, out int year)
    {
        year = 0;
        return int.TryParse(input, out year)
               && year >= 1900
               && year <= DateTime.Now.Year + 1;
    }

    public static bool IsValidIsbn(string isbn) =>
        !string.IsNullOrWhiteSpace(isbn) && isbn.Length >= 10;
}
