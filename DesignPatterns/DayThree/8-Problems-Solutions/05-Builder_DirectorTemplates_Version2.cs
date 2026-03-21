// 05-Builder_DirectorTemplates.cs
// Builder + Director: Director builds preset templates using InvoiceBuilder.
// DI/Lifetime: Builders transient; Director stateless (Singleton/Transient ok).
// Testability: Test Director by injecting a test builder that records steps.

using System;
using System.Collections.Generic;

public class Invoice
{
    public string Title { get; }
    public IReadOnlyList<string> Items { get; }
    public decimal Total { get; }

    internal Invoice(string title, List<string> items, decimal total)
    {
        Title = title; Items = items.AsReadOnly(); Total = total;
    }
}

public interface IInvoiceBuilder
{
    IInvoiceBuilder WithTitle(string title);
    IInvoiceBuilder AddItem(string description, decimal amount);
    Invoice Build();
}

public class InvoiceBuilder : IInvoiceBuilder
{
    private string _title = "Untitled Invoice";
    private readonly List<string> _items = new();
    private decimal _total = 0m;

    public IInvoiceBuilder WithTitle(string title) { _title = title; return this; }
    public IInvoiceBuilder AddItem(string description, decimal amount) { _items.Add(description); _total += amount; return this; }
    public Invoice Build() => new Invoice(_title, new List<string>(_items), _total);
}

public class InvoiceDirector
{
    public Invoice CreateStandardInvoice(IInvoiceBuilder builder)
    {
        return builder
            .WithTitle("Standard Invoice")
            .AddItem("Service A", 100m)
            .AddItem("Service B", 150m)
            .Build();
    }

    public Invoice CreateCreditMemo(IInvoiceBuilder builder)
    {
        return builder
            .WithTitle("Credit Memo")
            .AddItem("Return Adjustment", -50m)
            .Build();
    }
}