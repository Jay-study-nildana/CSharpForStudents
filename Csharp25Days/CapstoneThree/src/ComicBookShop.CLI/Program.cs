using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ComicBookShop.CLI.Menus;
using ComicBookShop.Core.Entities;
using ComicBookShop.Core.Interfaces;
using ComicBookShop.Core.Services;
using ComicBookShop.Infrastructure.Configuration;
using ComicBookShop.Infrastructure.Logging;
using ComicBookShop.Infrastructure.Persistence;

namespace ComicBookShop.CLI;

/// <summary>
/// Application entry point — composition root.
/// Demonstrates DI container setup, configuration binding, and event wiring (Days 17, 18, 19).
/// </summary>
internal class Program
{
    static async Task Main(string[] args)
    {
        // ── 1. Load configuration (Day 18) ──────────────────────────────
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var settings = new AppSettings();
        configuration.GetSection("AppSettings").Bind(settings);

        // ── 2. Build DI container (Day 17) ──────────────────────────────
        var services = new ServiceCollection();
        ConfigureServices(services, settings);
        var provider = services.BuildServiceProvider();

        // ── 3. Seed sample data (Day 11) ────────────────────────────────
        await DataSeeder.SeedAsync(
            provider.GetRequiredService<IRepository<ComicBook>>(),
            provider.GetRequiredService<IRepository<Customer>>(),
            provider.GetRequiredService<IAppLogger>());

        // ── 4. Wire events (Day 19) ─────────────────────────────────────
        var inventoryManager = provider.GetRequiredService<InventoryManager>();
        var orderService = provider.GetRequiredService<OrderService>();
        var notifications = provider.GetRequiredService<NotificationService>();

        inventoryManager.StockChanged += notifications.OnStockChanged;
        inventoryManager.LowStockAlert += notifications.OnLowStockAlert;
        orderService.OrderPlaced += notifications.OnOrderPlaced;

        // ── 5. Run the interactive menu ─────────────────────────────────
        var mainMenu = provider.GetRequiredService<MainMenu>();
        await mainMenu.RunAsync();
    }

    /// <summary>
    /// Registers all services with the DI container.
    /// Constructor injection keeps classes loosely coupled (Day 17).
    /// </summary>
    private static void ConfigureServices(IServiceCollection services, AppSettings settings)
    {
        // Settings
        services.AddSingleton(settings);

        // Logging (Day 18)
        services.AddSingleton<IAppLogger>(new FileLogger(settings.LogDirectory));

        // Repositories — generic JSON persistence (Days 12, 15)
        services.AddSingleton<IRepository<ComicBook>>(_ => new JsonRepository<ComicBook>(settings.DataDirectory));
        services.AddSingleton<IRepository<Customer>>(_ => new JsonRepository<Customer>(settings.DataDirectory));
        services.AddSingleton<IRepository<Order>>(_ => new JsonRepository<Order>(settings.DataDirectory));

        // Domain services (Day 9 — interface-based)
        services.AddSingleton<IComicBookService, ComicBookService>();
        services.AddSingleton<ICustomerService, CustomerService>();

        // Inventory manager — concrete, for event-wiring access (Day 19)
        services.AddSingleton<InventoryManager>(sp =>
            new InventoryManager(
                sp.GetRequiredService<IRepository<ComicBook>>(),
                sp.GetRequiredService<IAppLogger>(),
                settings.LowStockThreshold));

        // Order service — registered as both concrete and interface
        services.AddSingleton<OrderService>();
        services.AddSingleton<IOrderService>(sp => sp.GetRequiredService<OrderService>());

        // Notification subscriber (Day 19)
        services.AddSingleton<NotificationService>();

        // CLI menus
        services.AddSingleton<ComicBookMenu>();
        services.AddSingleton<CustomerMenu>();
        services.AddSingleton<OrderMenu>();
        services.AddSingleton<ReportMenu>();
        services.AddSingleton<MainMenu>(sp => new MainMenu(
            sp.GetRequiredService<ComicBookMenu>(),
            sp.GetRequiredService<CustomerMenu>(),
            sp.GetRequiredService<OrderMenu>(),
            sp.GetRequiredService<ReportMenu>(),
            settings.ShopName));
    }
}
