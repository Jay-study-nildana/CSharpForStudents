using System;
using System.Collections.Generic;
using System.Text.Json;

class Serialization_Polymorphic_SaveLoad
{
    // Base Document with virtual Serialize; real code might use proper polymorphic serialization.
    public abstract class Document
    {
        public string Title { get; set; }
        public Document(string title) => Title = title;
        public abstract string Serialize();
    }

    public class Invoice : Document
    {
        public decimal Amount { get; set; }
        public Invoice(string title, decimal amount) : base(title) => Amount = amount;
        public override string Serialize() => JsonSerializer.Serialize(new { Type = "Invoice", Title, Amount });
    }

    public class Letter : Document
    {
        public string Body { get; set; }
        public Letter(string title, string body) : base(title) => Body = body;
        public override string Serialize() => JsonSerializer.Serialize(new { Type = "Letter", Title, Body });
    }

    static void Main()
    {
        var docs = new List<Document>
        {
            new Invoice("Invoice #1", 99.95m),
            new Letter("Hello", "Dear friend...")
        };

        foreach (var d in docs)
        {
            var serialized = d.Serialize();
            Console.WriteLine($"{d.GetType().Name} serialized: {serialized}");
        }

        // Polymorphism allows saving a heterogeneous list of Document without switch on concrete types.
    }
}