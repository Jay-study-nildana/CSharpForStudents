# Day 23 — Clean code, naming, and small refactors

Objective: Introduce clean-code principles, practical naming conventions, small refactor patterns (Extract Method, Rename), and common code smells to watch for.

Why this matters
Clean code improves readability, reduces bugs, and makes future changes faster and safer. Small, disciplined refactors keep your capstone and production code maintainable without heavy rewrites.

Key Principles (summary)
- Make intent obvious: Code should read like prose; prefer `calculateInvoiceTotal()` over vague names.
- Small functions: One level of abstraction per function.
- Favor meaningful names: Names communicate behavior, not implementation.
- Reduce duplication: DRY — duplicate logic is a maintenance tax.
- Keep side effects visible: Minimize hidden state changes.
- Prefer testable units: Small, pure functions are easy to test.
- Use comments to explain "why", not "what".

Naming conventions (practical rules)
- Methods: Use verb or verb phrase (e.g., `GetCustomerOrders`, `CalculateTax`).
- Properties/fields: Nouns or noun phrases (e.g., `OrderTotal`, `IsActive`). Private fields: `_camelCase`.
- Classes/interfaces: Nouns for classes (`InvoiceBuilder`), adjectives for interfaces can be avoided—prefer `IInvoiceService`.
- Booleans: Use prefixes that read clearly: `Is`, `Has`, `Can`, `Supports` (e.g., `IsArchived`, not `ArchivedFlag`).
- Avoid encoding types in names: prefer `CustomerRepository` not `CustomerRepoList`.
- Keep names short but precise. If something needs a long comment to explain, rename it.

Small refactor patterns

1) Extract Method
When a method is doing multiple tasks or is too long, extract a clearly named method so each method does one thing.

Before:
```csharp
// Messy: long method that mixes validation, calculation, and persistence
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

After (Extract Method):
```csharp
public void ProcessOrder(Order order)
{
    Validate(order);
    CalculateTotals(order);
    Persist(order);
}

private void Validate(Order order)
{
    if (order == null) throw new ArgumentNullException(nameof(order));
    if (order.Items.Count == 0) throw new InvalidOperationException("Empty order.");
}

private void CalculateTotals(Order order)
{
    decimal subtotal = 0;
    foreach (var item in order.Items)
    {
        subtotal += item.Price * item.Quantity;
    }

    order.Subtotal = subtotal;
    order.Tax = subtotal * 0.08m;
    order.Total = order.Subtotal + order.Tax;
}

private void Persist(Order order)
{
    _db.Orders.Add(order);
    _db.SaveChanges();
}
```
Benefits: improved readability, easier unit testing, clearer intent.

2) Rename
Good renames prevent misunderstandings. Use a single-responsibility IDE rename so references update safely.

Before:
```csharp
int d; // what is d?
d = CalculateDiscount(customer);
```

After:
```csharp
int loyaltyDiscount = CalculateDiscount(customer);
```
Renaming improves comprehension at the call site and avoids misleading comments.

Common code smells (what to watch for)
- Long method: hard to follow; likely needs Extract Method.
- Large class / God object: too many responsibilities; consider splitting.
- Duplicate code: copy-paste logic across classes; extract shared functions or introduce a shared service.
- Long parameter list: hides dependencies; consider grouping parameters or introducing a small parameter object.
- Primitive obsession: overuse of primitives instead of small types/objects (e.g., using `string` for email).
- Feature envy: one class uses many methods of another — consider moving behavior.
- Switch or if-else chains: repeated conditions across code; consider polymorphism or strategy pattern.
- Shotgun surgery: small change requires edits in many places; indicates poor modularization.
- Dead code and commented-out blocks: remove them; source control preserves history.
- Misleading names: variables or methods whose names don't match intent.

Guided exercise (in-class code review)
Audit this short method for readability and list suggested refactors.

Snippet:
```csharp
public void RunReport(User u, DateTime s, DateTime e, bool detailed)
{
    var rows = _repo.Get(u.Id, s, e);
    decimal total = 0;
    foreach(var r in rows)
    {
        total += r.Amount;
        if (detailed)
        {
            Console.WriteLine(r.Description + " " + r.Amount);
        }
    }
    Console.WriteLine("Total: " + total);
}
```

Suggested audit notes
- Rename `u` to `user`, `s`/`e` to `start`/`end`, `rows` to `transactions`.
- Separate concerns: fetching data, formatting output, and summing amounts should be separate methods.
- Avoid Console calls in business logic — inject a renderer or return a DTO to the caller.
- Extract `PrintDetailedTransaction` and `CalculateTotal` methods.
- Make method testable by returning results instead of writing to console.

Refactored approach (sketch):
- `ITransactionRepository.GetTransactions(userId, start, end)`
- `decimal CalculateTotal(IEnumerable<Transaction> txs)`
- `IReportRenderer.RenderSummary(total)` and `RenderDetails(Transaction tx)`

Homework (mini-review of your capstone)
1. Identify three places where a long method or class can be split; propose an Extract Method or Extract Class.
2. Find two ambiguous names and propose clearer renames (explain why).
3. Spot one code smell (duplication, long param list, primitive obsession, etc.) and sketch a refactor (2–3 sentences).

Quick checklist for pull requests/code reviews
- Does the code read like a story? If not, name or extract methods to clarify.
- Are names precise and intention-revealing?
- Is behavior covered by small focused unit tests?
- Any duplication or obvious smells?
- Are side effects and dependencies explicit and injected?

Further reading (short)
- “Clean Code” by Robert C. Martin — concepts and refactor patterns.
- Martin Fowler — Refactoring catalog (extract method, rename, introduce parameter object).
- C# naming conventions (Microsoft docs) — align with team standards.

Wrap-up
Make small, frequent refactors. Extract method and rename are safe, high-impact patterns that dramatically increase clarity. Prioritize readability over cleverness; future you (and your reviewer) will thank you.