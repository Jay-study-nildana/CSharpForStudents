# Day 23 — Clean code exercises (10 problems)

Purpose: Practice clean code principles: naming, small refactors (Extract Method, Rename), and spotting/fixing common code smells. For each problem below, refactor the code, explain the change, and provide the improved code. Solutions are provided in the matching .cs files (filenames exactly match the problem titles below).

Instructions for students:
- Work on each problem for 10–15 minutes.
- Focus on intent-revealing names, single responsibility, small methods, and testability.
- For refactors that introduce new types (value objects, parameter objects, interfaces), keep them small and focused.

Problems

1) 01-ExtractMethod_ProcessOrder
Problem:
Refactor this long method by applying Extract Method so each step is obvious and testable.

```csharp
public void ProcessOrder(Order order)
{
    if (order == null) throw new ArgumentNullException(nameof(order));
    if (order.Items.Count == 0) throw new InvalidOperationException("Empty order.");

    decimal subtotal = 0;
    foreach (var item in order.Items)
    {
        subtotal += item.Price * item.Quantity;
    }

    decimal tax = subtotal * 0.08m;
    decimal total = subtotal + tax;

    order.Subtotal = subtotal;
    order.Tax = tax;
    order.Total = total;

    // persist
    _db.Orders.Add(order);
    _db.SaveChanges();
}
```

2) 02-Rename_VariableClarity
Problem:
Improve variable names and make the code self-documenting. The snippet below uses unclear names.

```csharp
int d;
d = CalculateDiscount(customer);
Console.WriteLine("Discount: " + d);
```

3) 03-SplitLargeClass_InvoiceManager
Problem:
This class mixes calculation, formatting, and persistence. Propose and implement a split into at least two smaller classes with clear responsibilities.

```csharp
public class InvoiceManager
{
    public void SaveAndPrint(Invoice invoice)
    {
        // calculate
        invoice.Total = invoice.Items.Sum(i => i.Price * i.Quantity);
        invoice.Tax = invoice.Total * 0.1m;
        // format
        Console.WriteLine("Invoice: " + invoice.Id + " Total:" + invoice.Total);
        // persist
        _db.Invoices.Add(invoice);
        _db.SaveChanges();
    }
}
```

4) 04-IntroduceParameterObject
Problem:
Refactor the method with many related parameters by introducing a parameter object.

```csharp
public void CreateUser(string firstName, string lastName, string email, string phone, bool isAdmin, DateTime birthDate)
{
    // ...
}
```

5) 05-ReplacePrimitiveWithType_EmailValidation
Problem:
Replace a primitive `string` email parameter with a small value object that validates email format and prevents invalid states.

```csharp
public void RegisterUser(string email)
{
    if (!email.Contains("@")) throw new ArgumentException("invalid");
    // ...
}
```

6) 06-RemoveDuplicateCode_Calculation
Problem:
Two services calculate tax using identical logic. Extract a shared service to remove duplication.

Service A:
```csharp
decimal tax = order.Total * 0.08m;
order.Tax = tax;
```

Service B:
```csharp
decimal tax = invoice.Amount * 0.08m;
invoice.Tax = tax;
```

7) 07-ReplaceIfElseWithPolymorphism_PaymentProcessor
Problem:
Replace the conditional handling of payment types with polymorphism (strategy or polymorphic handlers).

```csharp
public void ProcessPayment(Payment p)
{
    if (p.Type == PaymentType.CreditCard) { /* process CC */ }
    else if (p.Type == PaymentType.PayPal) { /* process PayPal */ }
    else if (p.Type == PaymentType.BankTransfer) { /* process bank */ }
}
```

8) 08-MakeMethodsTestable_ConsoleOutput
Problem:
A method writes directly to `Console`. Refactor so core logic returns data and output is handled by an injected renderer (interface), improving testability.

```csharp
public void RunReport(User u, DateTime start, DateTime end, bool detailed)
{
    var rows = _repo.Get(u.Id, start, end);
    decimal total = 0;
    foreach(var r in rows)
    {
        total += r.Amount;
        if (detailed) Console.WriteLine(r.Description + " " + r.Amount);
    }
    Console.WriteLine("Total: " + total);
}
```

9) 09-ShortenMethod_SplitLoops
Problem:
A method mixes data transformation, accumulation and printing across multiple loops. Refactor to shorter methods with clear names.

```csharp
public void Analyze(DataSet ds)
{
    // long method with several loops, accumulators, and prints...
}
```

(Provide a short representative snippet like the one in 08 and refactor it.)

10) 10-DetectAndFixCodeSmells_ReviewSnippet
Problem:
Review this snippet, list at least three code smells you see, and provide a refactored version.

```csharp
public void DoWork(Order o)
{
    var items = o.items;
    int i = 0;
    foreach(var it in items)
    {
        if(it.qty > 0) i += it.qty * it.price;
    }
    o.total = i;
    // write out
    Console.WriteLine("Total:" + o.total);
}
```

---

Solutions
Each solution file is provided as a C# file named to match the problem title:
- 01-ExtractMethod_ProcessOrder.cs
- 02-Rename_VariableClarity.cs
- 03-SplitLargeClass_InvoiceManager.cs
- 04-IntroduceParameterObject.cs
- 05-ReplacePrimitiveWithType_EmailValidation.cs
- 06-RemoveDuplicateCode_Calculation.cs
- 07-ReplaceIfElseWithPolymorphism_PaymentProcessor.cs
- 08-MakeMethodsTestable_ConsoleOutput.cs
- 09-ShortenMethod_SplitLoops.cs
- 10-DetectAndFixCodeSmells_ReviewSnippet.cs

Use these during class review or distribute them as the instructor's solutions. Each solution is intentionally small and focused so students can compare their refactors with a clear example.
