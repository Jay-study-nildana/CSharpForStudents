namespace SmartStore.DI;

// ================================================================
// DEPENDENCY INJECTION + SINGLETON PATTERN
// ================================================================
// AppServiceContainer is the application's composition root.
// It wires all dependencies and manages object lifetimes:
//
//   SINGLETON  — one shared instance (EventManager, CommandHistory,
//                AuditLog, product repository, theme factory)
//   TRANSIENT  — new instance per operation (UnitOfWork)
//
// WHY DI over static access / service-locator:
//   • Dependencies are explicit (constructor injection)
//   • Easy to swap implementations (e.g., real DB vs in-memory)
//   • Enables unit-testing with mock/fake collaborators
//
// This replaces a DI framework (like Microsoft.Extensions.DI) to keep
// the capstone self-contained and educational.
// ================================================================
public class AppServiceContainer
{
    // ── Singleton repositories (shared state across the whole demo) ──
    private readonly InMemoryOrderRepository   _orderRepo   = new();
    private readonly InMemoryProductRepository _productRepo = new();

    // ── Singletons exposed to the app ──
    public CachedProductRepositoryProxy ProductCatalog  { get; }   // Proxy wrapping real repo
    public OrderEventManager            EventManager    { get; }
    public CommandHistory               CommandHistory  { get; }
    public AuditLogObserver             AuditLog        { get; }
    public IThemeFactory                ThemeFactory    { get; }

    public AppServiceContainer(bool useDarkTheme = false)
    {
        // Proxy (Structural) wraps the real product repository
        ProductCatalog = new CachedProductRepositoryProxy(_productRepo);

        // Observer singletons
        EventManager   = new OrderEventManager();
        CommandHistory = new CommandHistory();
        AuditLog       = new AuditLogObserver();

        // Wire up observers (Observer pattern)
        EventManager.Subscribe(new EmailObserver());
        EventManager.Subscribe(new InventoryObserver());
        EventManager.Subscribe(AuditLog);

        // Abstract Factory — theme selected at startup (config-driven)
        ThemeFactory = useDarkTheme
            ? new DarkThemeFactory()
            : new LightThemeFactory();
    }

    // ── TRANSIENT ── new UnitOfWork per operation
    // The decorator is applied here — every UoW gets logging "for free"
    public IUnitOfWork CreateUnitOfWork()
    {
        IOrderRepository decorated = new LoggingOrderRepositoryDecorator(_orderRepo); // Decorator
        return new InMemoryUnitOfWork(decorated, ProductCatalog);
    }

    // ── STRATEGY FACTORY ── selects the right pricing strategy by customer type
    public IPricingStrategy GetPricingStrategy(Customer customer) =>
        customer.Type switch
        {
            CustomerType.Vip     => new VipPricingStrategy(),
            CustomerType.Premium => new DiscountPricingStrategy(10),
            _                    => new RegularPricingStrategy()
        };

    // ── Builds the validation chain (Chain of Responsibility) ──
    public OrderValidationHandler BuildValidationChain(decimal minimumOrderValue = 10m)
    {
        var emptyCart  = new EmptyCartValidationHandler();
        var stock      = new StockValidationHandler();
        var minValue   = new MinimumOrderValueHandler(minimumOrderValue);

        emptyCart.SetNext(stock).SetNext(minValue);
        return emptyCart;   // return the head of the chain
    }
}
