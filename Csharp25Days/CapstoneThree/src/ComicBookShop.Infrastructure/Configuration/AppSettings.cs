namespace ComicBookShop.Infrastructure.Configuration;

/// <summary>
/// POCO for binding appsettings.json → strongly-typed configuration.
/// Demonstrates configuration and environment-based settings (Day 18).
/// </summary>
public class AppSettings
{
    public string DataDirectory { get; set; } = "data";
    public string LogDirectory { get; set; } = "logs";
    public string LogLevel { get; set; } = "Information";
    public int LowStockThreshold { get; set; } = 5;
    public string ShopName { get; set; } = "Comic Book Shop";
}
