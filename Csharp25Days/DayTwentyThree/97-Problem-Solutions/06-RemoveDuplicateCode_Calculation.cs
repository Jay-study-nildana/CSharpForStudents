// 06-RemoveDuplicateCode_Calculation.cs
// Extracted TaxService used by OrderService and InvoiceService to avoid duplication.
using System;

public class Order { public decimal Total; public decimal Tax; }
public class Invoice { public decimal Amount; public decimal Tax; }

public class TaxService
{
    private readonly decimal _rate;
    public TaxService(decimal rate) => _rate = rate;
    public decimal CalculateTax(decimal baseAmount) => Math.Round(baseAmount * _rate, 2);
}

public class OrderService
{
    private readonly TaxService _taxService;
    public OrderService(TaxService taxService) => _taxService = taxService;
    public void ApplyTax(Order order) => order.Tax = _taxService.CalculateTax(order.Total);
}

public class InvoiceService
{
    private readonly TaxService _taxService;
    public InvoiceService(TaxService taxService) => _taxService = taxService;
    public void ApplyTax(Invoice invoice) => invoice.Tax = _taxService.CalculateTax(invoice.Amount);
}