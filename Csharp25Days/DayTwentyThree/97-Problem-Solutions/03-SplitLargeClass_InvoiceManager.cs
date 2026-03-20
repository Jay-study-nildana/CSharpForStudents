// 03-SplitLargeClass_InvoiceManager.cs
// Split responsibilities into InvoiceCalculator, InvoiceRepository, InvoicePrinter.
using System;
using System.Linq;
using System.Collections.Generic;

public class Invoice { public int Id; public List<LineItem> Items = new(); public decimal Total; public decimal Tax; }
public class LineItem { public decimal Price; public int Quantity; }

public interface IInvoiceRepository { void Save(Invoice invoice); }
public interface IInvoicePrinter { void Print(Invoice invoice); }

public class InvoiceCalculator
{
    public void Calculate(Invoice invoice)
    {
        invoice.Total = invoice.Items.Sum(i => i.Price * i.Quantity);
        invoice.Tax = invoice.Total * 0.1m;
    }
}

public class InvoiceService
{
    private readonly InvoiceCalculator _calculator;
    private readonly IInvoiceRepository _repo;
    private readonly IInvoicePrinter _printer;

    public InvoiceService(InvoiceCalculator calculator, IInvoiceRepository repo, IInvoicePrinter printer)
    {
        _calculator = calculator; _repo = repo; _printer = printer;
    }

    public void SaveAndPrint(Invoice invoice)
    {
        _calculator.Calculate(invoice);
        _repo.Save(invoice);
        _printer.Print(invoice);
    }
}

// Example concrete printer
public class ConsoleInvoicePrinter : IInvoicePrinter
{
    public void Print(Invoice invoice)
    {
        Console.WriteLine($"Invoice: {invoice.Id} Total: {invoice.Total:C} Tax: {invoice.Tax:C}");
    }
}