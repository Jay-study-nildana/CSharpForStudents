# DiscountApp — demo and explanation

How Checkout gets linked to IDiscountPolicy (short and step-by-step)

Short answer
- The DI container (ServiceProvider created from ServiceCollection) does the linking. When you request a Checkout instance, the provider inspects Checkout’s constructor, sees the IDiscountPolicy parameter, resolves an IDiscountPolicy (using whatever you registered), and calls the Checkout constructor with that instance.

Step-by-step
1. Registration: services.AddTransient<IDiscountPolicy>(sp => new PercentageDiscountPolicy(0.15m));  
    — tells the container how to create an IDiscountPolicy.
2. Register Checkout: services.AddTransient<Checkout>();  
    — tells the container how to create a Checkout (it will discover the constructor).
3. Build: var provider = services.BuildServiceProvider();  
4. Resolve: provider.GetRequiredService<Checkout>()  
    — provider finds Checkout’s constructor, sees it needs IDiscountPolicy, resolves that dependency (using the registered factory), and constructs the Checkout with the resolved instance.
5. If a required dependency is missing, resolving will throw an exception (InvalidOperationException).

Notes
- You registered a factory because PercentageDiscountPolicy requires a decimal parameter. If PercentageDiscountPolicy had a parameterless or resolvable constructor, you could register type-to-type: services.AddTransient<IDiscountPolicy, PercentageDiscountPolicy>().
- Lifetimes matter: AddTransient creates a new instance each resolve; AddScoped and AddSingleton change lifetime behavior.
- This is also called as "Strategy Pattern". - Swap behavior at runtime (strategy).

---

## Program.cs

```csharp name=Program.cs
// --- Program starts here (top-level statements) ---
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Discount policy demo");

// Example using manual constructor injection
decimal sampleTotal = 100m;
var manualCheckout = new Checkout(new NoDiscountPolicy());
decimal discountedAmount = manualCheckout.FinalDiscountedAmount(sampleTotal);
Console.WriteLine($"Manual DI -> NoDiscount on {sampleTotal}: {discountedAmount}");

// Example using Microsoft.Extensions.DependencyInjection
var services = new ServiceCollection();

// Register a PercentageDiscountPolicy (15%) as the IDiscountPolicy
services.AddTransient<IDiscountPolicy>(sp => new PercentageDiscountPolicy(0.15m));
// Register Checkout so the container will inject IDiscountPolicy into it
services.AddTransient<Checkout>();

using var provider = services.BuildServiceProvider();
var diCheckout = provider.GetRequiredService<Checkout>();
decimal discountedAmount2 = diCheckout.FinalDiscountedAmount(sampleTotal);
Console.WriteLine($"Container DI -> PercentageDiscount (15%) on {sampleTotal}: {discountedAmount2}");

// Interactive demo
Console.WriteLine();
Console.Write("Enter total amount (e.g. 125.50): ");
var input = Console.ReadLine();
if (!decimal.TryParse(input, out var total))
{
    Console.WriteLine("Invalid amount. Exiting.");
    return;
}

Console.WriteLine("Choose customer type:");
Console.WriteLine("  0 (New Customer)= None");
Console.WriteLine("  1 (Regular Customer)= 10%");
Console.WriteLine("  2 (VIP Customer)= 20%");
Console.Write("Selection: ");
var choice = Console.ReadLine();

IDiscountPolicy policy = choice switch
{
    "1" => new PercentageDiscountPolicy(0.10m),
    "2" => new PercentageDiscountPolicy(0.20m),
    _ => new NoDiscountPolicy()
};

var checkout = new Checkout(policy);
Console.WriteLine($"Final total: {checkout.FinalDiscountedAmount(total)}");


public interface IDiscountPolicy
{
    decimal Apply(decimal total);
}

public class NoDiscountPolicy : IDiscountPolicy
{
    public decimal Apply(decimal total) => total;
}

public class PercentageDiscountPolicy : IDiscountPolicy
{
    private readonly decimal _p;
    public PercentageDiscountPolicy(decimal p) => _p = p;
    public decimal Apply(decimal t) => t * (1 - _p);
}

public class Checkout
{
    private readonly IDiscountPolicy _policy;
    public Checkout(IDiscountPolicy p) => _policy = p;
    public decimal FinalDiscountedAmount(decimal total) => _policy.Apply(total);
}


```


