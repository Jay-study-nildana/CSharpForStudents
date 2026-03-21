# Day 11 — Integration / Refactor Lab (Apply Multiple Patterns)

Objectives
- Apply multiple design patterns to improve a legacy-style codebase: Dependency Injection (DI), Repository, Factory/Builder, Strategy, Observer, and others.
- Practice spotting pattern candidates, designing incremental refactors, and communicating design rationale.
- Deliver: a team design doc, UML, and test plan mapping each pattern to specific problems.

Session plan (90–120 minutes)
1. 10 min — Instructor demo: quick walkthrough of a legacy class and candidate refactors.
2. 60–80 min — Team refactor planning and lightweight prototyping (conceptual + code sketches).
3. 10–15 min — Team demos (3–4 minutes each): design notes, UML, and testing approach.
4. Homework — finalize design doc and pattern-to-problem mapping.

Spotting pattern candidates (rules of thumb)
- High coupling / hard-coded dependencies → introduce DI + interfaces.
- Repeated data-access code → extract Repository.
- Many creation branches (complex construction) → Factory or Builder.
- Many conditional behaviors switched on type or flags → Strategy or Command.
- Cross-cutting notifications (logging, UI updates) → Observer / Event Aggregator.
- Chains of responsibility (validation/processing) → Chain of Responsibility or Pipeline.

Refactor workflow (incremental and safe)
1. Identify a small, high-value target (one class or module).
2. Write tests around current behavior (characterization tests) to lock behavior.
3. Introduce interfaces for collaborators (non-breaking).
4. Use the DI pattern (constructor injection) and swap in test doubles.
5. Refactor internals: extract repository/factory/strategy as separate components.
6. Keep changes small and compile/green-tests after each step.
7. Add integration tests and update design artifacts (UML + rationale).

Example: Legacy service (before)
```csharp
// LegacyBillingService.cs — tightly coupled implementation
public class LegacyBillingService
{
    public void Charge(Order order)
    {
        var db = new SqlConnection("..."); // hard-coded connection
        // build invoice inline
        decimal total = 0;
        foreach (var li in order.Items) total += li.Price * li.Qty;
        // persist invoice
        // send email via new SmtpClient("smtp.example.com");
        // update order status
    }
}