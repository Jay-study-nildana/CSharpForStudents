// ════════════════════════════════════════════════════════════════════
//  SmartStore — Design Patterns Capstone
//  Demonstrates 12 GoF patterns + SOLID, DI, Repository, Unit of Work
// ════════════════════════════════════════════════════════════════════
//  CREATIONAL  : Builder · Factory Method · Abstract Factory · Singleton (DI)
//  STRUCTURAL  : Adapter · Facade · Decorator · Proxy · Composite · Bridge
//  BEHAVIORAL  : Strategy · Template Method · Observer · Mediator
//               Command (+ Undo) · Chain of Responsibility
//  DATA ACCESS : Repository · Unit of Work
// ════════════════════════════════════════════════════════════════════

// ── Composition root (DI + lifetime management) ──────────────────────
// Pass true for dark theme, false for light theme
var container = new AppServiceContainer(useDarkTheme: false);

// Theme-aware renderers created by the Abstract Factory
var header = container.ThemeFactory.CreateHeaderRenderer();
var table  = container.ThemeFactory.CreateTableRenderer();
var status = container.ThemeFactory.CreateStatusRenderer();

// ══════════════════════════════════════════════════════════════════════
// SECTION 1 — ABSTRACT FACTORY: Themed Console UI
// ══════════════════════════════════════════════════════════════════════
header.Render("SMARTSTORE — Design Patterns Capstone Demo");
table.RenderRow("Patterns demonstrated", "14 (creational + structural + behavioral)");
table.RenderRow("Data access patterns",  "Repository + Unit of Work");
table.RenderRow("Theme",                 container.ThemeFactory.GetType().Name);
Console.WriteLine();
Console.WriteLine("  Switch theme: change 'useDarkTheme: false' to 'true' in Program.cs");

Pause();

// ══════════════════════════════════════════════════════════════════════
// SECTION 2 — PROXY: Cached Product Catalog
// ══════════════════════════════════════════════════════════════════════
header.Render("PROXY — Cached Product Repository");
Console.WriteLine("  First call → cache miss, loads from real repository:");
var products = container.ProductCatalog.GetAll().ToList();

Console.WriteLine("\n  Second call → cache hit, no round-trip to repository:");
_ = container.ProductCatalog.GetAll().ToList();

Console.WriteLine("\n  Product Catalogue:");
foreach (var p in products)
    Console.WriteLine($"    {p}");

Pause();

// ══════════════════════════════════════════════════════════════════════
// SECTION 3 — BUILDER + COMPOSITE: Building Orders
// ══════════════════════════════════════════════════════════════════════
header.Render("BUILDER + COMPOSITE — Constructing Orders");

// Look up products from the cached proxy
var laptop   = container.ProductCatalog.GetById(1)!;
var mouse    = container.ProductCatalog.GetById(2)!;
var keyboard = container.ProductCatalog.GetById(3)!;
var monitor  = container.ProductCatalog.GetById(5)!;
var webcam   = container.ProductCatalog.GetById(8)!;
var usbHub   = container.ProductCatalog.GetById(7)!;

// Customers
var alice = new Customer { Id = 101, Name = "Alice",   Type = CustomerType.Regular };
var bob   = new Customer { Id = 102, Name = "Bob",     Type = CustomerType.Vip     };
var carol = new Customer { Id = 103, Name = "Carol",   Type = CustomerType.Premium };

// ── BUILDER: Alice builds a simple order ──
Console.WriteLine("\n  [Builder] Alice's order (individual items):");
var order1 = new OrderBuilder()
    .ForCustomer(alice)
    .WithItem(laptop, 1)
    .WithItem(mouse,  1)
    .WithNotes("Gift wrap requested")
    .Build();
order1.Display();

// ── BUILDER + COMPOSITE: Bob builds an order with a bundle ──
Console.WriteLine("\n  [Builder + Composite] Bob's order (bundle item):");

var homeOfficeBundle = new BundleOrderItem("Home-Office Bundle", discountPercent: 10);
homeOfficeBundle.Add(new OrderItem { Product = monitor,  Quantity = 1, UnitPrice = monitor.Price  });
homeOfficeBundle.Add(new OrderItem { Product = keyboard, Quantity = 1, UnitPrice = keyboard.Price });
homeOfficeBundle.Add(new OrderItem { Product = webcam,   Quantity = 1, UnitPrice = webcam.Price   });

var order2 = new OrderBuilder()
    .ForCustomer(bob)
    .WithBundle(homeOfficeBundle)
    .WithItem(usbHub, 2)
    .Build();
order2.Display();

// ── Carol: a rejected (invalid) build ──
Console.WriteLine("\n  [Builder] Demonstrating invalid build (no items) → exception:");
try
{
    var _ = new OrderBuilder().ForCustomer(carol).Build();
}
catch (InvalidOperationException ex)
{
    status.RenderStatus($"Caught expected exception: {ex.Message}", false);
}

Pause();

// ══════════════════════════════════════════════════════════════════════
// SECTION 4 — CHAIN OF RESPONSIBILITY: Order Validation
// ══════════════════════════════════════════════════════════════════════
header.Render("CHAIN OF RESPONSIBILITY — Order Validation");

var validationChain = container.BuildValidationChain(minimumOrderValue: 10m);

Console.WriteLine("\n  Validating Order 1 (Alice — should pass all rules):");
var result1 = validationChain.Handle(order1);
status.RenderStatus($"Order #{order1.Id} validation: {(result1.IsValid ? "PASSED" : result1.ErrorMessage)}",
                    result1.IsValid);

// ── Make an order that will fail at the stock rule ──
Console.WriteLine("\n  Validating a greedy order (999 laptops — should fail stock rule):");
var greedyOrder = new OrderBuilder()
    .ForCustomer(carol)
    .WithItem(laptop, 999)
    .Build();
var resultGreedy = validationChain.Handle(greedyOrder);
status.RenderStatus($"Greedy order validation: {resultGreedy.ErrorMessage}", resultGreedy.IsValid);

// ── Make an order that fails the minimum value rule ──
Console.WriteLine("\n  Validating a tiny order ($4.99 notebook — below $10 minimum):");
var notebook = container.ProductCatalog.GetById(6)!;
var tinyOrder = new OrderBuilder()
    .ForCustomer(carol)
    .WithItem(notebook, 1)
    .Build();
var resultTiny = validationChain.Handle(tinyOrder);
status.RenderStatus($"Tiny order validation: {resultTiny.ErrorMessage}", resultTiny.IsValid);

Pause();

// ══════════════════════════════════════════════════════════════════════
// SECTION 5 — STRATEGY: Pricing
// ══════════════════════════════════════════════════════════════════════
header.Render("STRATEGY — Interchangeable Pricing Algorithms");

var strategies = new IPricingStrategy[]
{
    new RegularPricingStrategy(),
    new DiscountPricingStrategy(15),
    new VipPricingStrategy()
};

Console.WriteLine($"\n  Order subtotal: ${order1.SubTotal:F2}  (Customer: {alice})");
foreach (var s in strategies)
{
    var discount = s.CalculateDiscount(order1);
    table.RenderRow(s.Name, $"Discount = ${discount:F2}  →  Total = ${order1.SubTotal - discount:F2}");
}

Console.WriteLine($"\n  Order subtotal: ${order2.SubTotal:F2}  (Customer: {bob})");
foreach (var s in strategies)
{
    var discount = s.CalculateDiscount(order2);
    table.RenderRow(s.Name, $"Discount = ${discount:F2}  →  Total = ${order2.SubTotal - discount:F2}");
}

// Pick the real strategies based on customer type
var strategy1 = container.GetPricingStrategy(order1.Customer);
var strategy2 = container.GetPricingStrategy(order2.Customer);
order1.Discount = strategy1.CalculateDiscount(order1);
order2.Discount = strategy2.CalculateDiscount(order2);
Console.WriteLine($"\n  Applied strategy for {alice}: '{strategy1.Name}' → Discount: ${order1.Discount:F2}");
Console.WriteLine($"  Applied strategy for {bob}:   '{strategy2.Name}' → Discount: ${order2.Discount:F2}");

Pause();

// ══════════════════════════════════════════════════════════════════════
// SECTION 6 — COMMAND + UNDO: Place and Cancel Orders
// ══════════════════════════════════════════════════════════════════════
header.Render("COMMAND (+ UNDO) — Encapsulated Requests with History");

using var uow1 = container.CreateUnitOfWork();

Console.WriteLine("\n  [Command] Executing PlaceOrderCommand for Order 1...");
container.CommandHistory.Execute(new PlaceOrderCommand(uow1, order1));
table.RenderRow("Order 1 status after place", order1.Status.ToString());

Console.WriteLine("\n  [Command] Executing PlaceOrderCommand for Order 2...");
container.CommandHistory.Execute(new PlaceOrderCommand(uow1, order2));
table.RenderRow("Order 2 status after place", order2.Status.ToString());

Console.WriteLine("\n  [Command] Executing CancelOrderCommand for Order 1...");
container.CommandHistory.Execute(new CancelOrderCommand(uow1, order1));
table.RenderRow("Order 1 status after cancel", order1.Status.ToString());

Console.WriteLine("\n  [Command] History stack:");
container.CommandHistory.Print();

Console.WriteLine("\n  [Command] Undoing last command (cancel)...");
container.CommandHistory.Undo();
table.RenderRow("Order 1 status after undo", order1.Status.ToString());

Console.WriteLine("\n  [Command] History stack after undo:");
container.CommandHistory.Print();

Pause();

// ══════════════════════════════════════════════════════════════════════
// SECTION 7 — TEMPLATE METHOD: Order Processing Pipeline
// ══════════════════════════════════════════════════════════════════════
header.Render("TEMPLATE METHOD — Invariant Processing Skeleton");

Console.WriteLine("\n  Processing Order 1 with StandardOrderProcessor:");
OrderProcessingTemplate standardProcessor = new StandardOrderProcessor();
standardProcessor.Process(order1);
table.RenderRow("Order 1 status", order1.Status.ToString());
table.RenderRow("Order 1 notes",  order1.Notes ?? "(none)");

Console.WriteLine("\n  Processing Order 2 with ExpressOrderProcessor:");
OrderProcessingTemplate expressProcessor = new ExpressOrderProcessor();
expressProcessor.Process(order2);
table.RenderRow("Order 2 status", order2.Status.ToString());
table.RenderRow("Order 2 notes",  order2.Notes ?? "(none)");

Pause();

// ══════════════════════════════════════════════════════════════════════
// SECTION 8 — OBSERVER: Event Notifications
// ══════════════════════════════════════════════════════════════════════
header.Render("OBSERVER — Publish / Subscribe Event System");

Console.WriteLine("\n  Firing 'OrderConfirmed' event for Order 1:");
container.EventManager.Notify(order1, "OrderConfirmed");

Console.WriteLine("\n  Firing 'OrderShipped' event for Order 2:");
container.EventManager.Notify(order2, "OrderShipped");

Console.WriteLine("\n  Unsubscribing InventoryObserver, then firing again:");
var inv = new InventoryObserver(); // create a second one to demo unsubscribe
container.EventManager.Subscribe(inv);
container.EventManager.Unsubscribe(inv);
container.EventManager.Notify(order1, "OrderUpdated");

Pause();

// ══════════════════════════════════════════════════════════════════════
// SECTION 9 — BRIDGE: Notifications via Different Channels
// ══════════════════════════════════════════════════════════════════════
header.Render("BRIDGE — Decoupled Notification Abstraction & Delivery");

Console.WriteLine("\n  Same 'OrderConfirmed' abstraction, different channel implementations:");

// Console channel
INotificationChannel consoleChannel = new ConsoleNotificationChannel();
OrderNotification confirmedViaConsole = new OrderConfirmedNotification(consoleChannel);
confirmedViaConsole.Send(order1);

// Email channel
INotificationChannel emailChannel = new EmailNotificationChannel();
OrderNotification confirmedViaEmail = new OrderConfirmedNotification(emailChannel);
confirmedViaEmail.Send(order1);

Console.WriteLine("\n  'OrderCancelled' abstraction via Email channel:");
OrderNotification cancelledViaEmail = new OrderCancelledNotification(emailChannel);
cancelledViaEmail.Send(order2);

Pause();

// ══════════════════════════════════════════════════════════════════════
// SECTION 10 — ADAPTER + FACADE: Full Checkout Pipeline
// ══════════════════════════════════════════════════════════════════════
header.Render("FACADE + ADAPTER — Simplified Checkout Pipeline");

// Build fresh orders for the facade demo (so we can re-confirm)
var chair  = container.ProductCatalog.GetById(4)!;
var carol2 = new Customer { Id = 103, Name = "Carol", Type = CustomerType.Premium };

var order3 = new OrderBuilder()
    .ForCustomer(carol2)
    .WithItem(chair, 1)
    .WithItem(mouse, 2)
    .Build();

// Place order3 in the repo first
using var uow2 = container.CreateUnitOfWork();
uow2.Orders.Add(order3);
uow2.Commit();

Console.WriteLine($"\n  Order to check out:");
order3.Display();

// Build facade with:
//   - Adapter  (LegacyPaymentAdapter wraps LegacyPaymentProcessor)
//   - Strategy (Premium = 10% off)
//   - Chain of Responsibility
//   - Repository + Unit of Work
//   - Observer (EventManager)
using var uow3 = container.CreateUnitOfWork();
var facade = new CheckoutFacade(
    uow:             uow3,
    payment:         new LegacyPaymentAdapter(),          // ADAPTER
    pricing:         container.GetPricingStrategy(carol2), // STRATEGY (chosen by DI container)
    eventManager:    container.EventManager,               // OBSERVER
    validationChain: container.BuildValidationChain()      // CHAIN
);

var success = facade.Checkout(order3);
status.RenderStatus($"Checkout for Order #{order3.Id}: {(success ? "SUCCESS" : "FAILED")}", success);

Pause();

// ══════════════════════════════════════════════════════════════════════
// SECTION 11 — MEDIATOR: Checkout Component Coordination
// ══════════════════════════════════════════════════════════════════════
header.Render("MEDIATOR — Decoupled Component Coordination");

Console.WriteLine("  Components: Cart, Payment, Summary — no direct cross-references.");
Console.WriteLine("  The Mediator orchestrates the flow when events fire.\n");

var mediator = new ConcreteCheckoutMediator();
var cart     = new CartComponent(mediator);
var payment  = new PaymentComponent(mediator);
var summary  = new SummaryComponent(mediator);

mediator.Cart    = cart;
mediator.Payment = payment;
mediator.Summary = summary;

// Trigger the chain: LoadOrder → mediator notifies Payment → mediator notifies Summary
cart.LoadOrder(order3);

Pause();

// ══════════════════════════════════════════════════════════════════════
// SECTION 12 — FACTORY METHOD: Dynamic Channel Creation
// ══════════════════════════════════════════════════════════════════════
header.Render("FACTORY METHOD — Polymorphic Channel Creation");

Console.WriteLine("  Using ConsoleNotificationFactory:");
NotificationChannelFactory factory = new ConsoleNotificationFactory();
factory.SendNotification("Alice", "Order Dispatch", "Your order is on its way!");

Console.WriteLine("\n  Switching to EmailNotificationFactory (same calling code):");
factory = new EmailNotificationFactory();
factory.SendNotification("Alice", "Order Dispatch", "Your order is on its way!");

Pause();

// ══════════════════════════════════════════════════════════════════════
// SECTION 13 — DECORATOR: Logging Repository
// ══════════════════════════════════════════════════════════════════════
header.Render("DECORATOR — Transparent Logging on Repository");

Console.WriteLine("  The order repository is wrapped with LoggingOrderRepositoryDecorator.");
Console.WriteLine("  Every UoW created by the DI container includes this decorator automatically.\n");
Console.WriteLine("  Fetching all orders via the decorated UoW:");
using var uow4 = container.CreateUnitOfWork();
var allOrders = uow4.Orders.GetAll().ToList();
Console.WriteLine($"\n  {allOrders.Count} order(s) in store:");
foreach (var o in allOrders)
    table.RenderRow($"Order #{o.Id}", $"{o.Customer.Name,-15} {o.Status,-12} ${o.Total:F2}");

Pause();

// ══════════════════════════════════════════════════════════════════════
// SECTION 14 — REPOSITORY + UNIT OF WORK SUMMARY
// ══════════════════════════════════════════════════════════════════════
header.Render("REPOSITORY + UNIT OF WORK — Data Access Summary");

Console.WriteLine("  Orders persisted in the in-memory store:\n");
foreach (var o in allOrders)
    o.Display();

Pause();

// ══════════════════════════════════════════════════════════════════════
// SECTION 15 — AUDIT LOG (collected by AuditLogObserver)
// ══════════════════════════════════════════════════════════════════════
header.Render("AUDIT LOG — Collected by AuditLogObserver");

var log = container.AuditLog.GetLog();
if (log.Count == 0)
{
    Console.WriteLine("  (no audit entries)");
}
else
{
    Console.WriteLine($"  {log.Count} audit entries:\n");
    foreach (var entry in log)
        Console.WriteLine($"    {entry}");
}

Pause();

// ══════════════════════════════════════════════════════════════════════
// PATTERNS INDEX
// ══════════════════════════════════════════════════════════════════════
header.Render("PATTERNS INDEX — Where to Find Each Pattern");

table.RenderRow("Builder",               "Patterns/Creational/OrderBuilder.cs");
table.RenderRow("Factory Method",        "Patterns/Creational/NotificationChannelFactory.cs");
table.RenderRow("Abstract Factory",      "Patterns/Creational/ThemeFactory.cs");
table.RenderRow("Singleton (DI)",        "DI/AppServiceContainer.cs");
table.RenderRow("Adapter",               "Patterns/Structural/LegacyPaymentAdapter.cs");
table.RenderRow("Facade",                "Patterns/Structural/CheckoutFacade.cs");
table.RenderRow("Decorator",             "Patterns/Structural/LoggingOrderRepositoryDecorator.cs");
table.RenderRow("Proxy",                 "Patterns/Structural/CachedProductRepositoryProxy.cs");
table.RenderRow("Composite",             "Patterns/Structural/BundleOrderItem.cs");
table.RenderRow("Bridge",                "Patterns/Structural/NotificationBridge.cs");
table.RenderRow("Strategy",              "Patterns/Behavioral/Strategies/PricingStrategies.cs");
table.RenderRow("Template Method",       "Patterns/Behavioral/OrderProcessingTemplate.cs");
table.RenderRow("Observer",              "Patterns/Behavioral/OrderEventManager.cs");
table.RenderRow("Mediator",              "Patterns/Behavioral/CheckoutMediator.cs");
table.RenderRow("Command + Undo",        "Patterns/Behavioral/Commands/OrderCommands.cs");
table.RenderRow("Chain of Responsibility","Patterns/Behavioral/ValidationChain/ValidationHandlers.cs");
table.RenderRow("Repository",            "Infrastructure/InMemoryOrderRepository.cs");
table.RenderRow("Unit of Work",          "Infrastructure/InMemoryUnitOfWork.cs");

Console.WriteLine();
status.RenderStatus("All pattern demonstrations complete.", true);
Console.WriteLine();

// ── Helper ─────────────────────────────────────────────────────────────
static void Pause()
{
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.Write("\n  Press [Enter] to continue...");
    Console.ResetColor();
    Console.ReadLine();
}
