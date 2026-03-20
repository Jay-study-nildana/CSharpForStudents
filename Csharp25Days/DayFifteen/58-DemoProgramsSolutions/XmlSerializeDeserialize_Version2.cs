// XmlSerializeDeserialize.cs
// Problem: XmlSerializeDeserialize
// Serialize/deserialize Product[] using XmlSerializer to a file.
// Complexity: O(n); XML is verbose but interoperable and supports schema (XSD) if needed.

using System;
using System.IO;
using System.Xml.Serialization;

[Serializable]
public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    // Parameterless constructor required for XmlSerializer
    public Product() { }
    public Product(int id, string name, decimal price) { Id = id; Name = name; Price = price; }
}

class XmlSerializeDeserialize
{
    public static void Save(string path, Product[] products)
    {
        var xs = new XmlSerializer(typeof(Product[]));
        using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
        xs.Serialize(fs, products);
    }

    public static Product[] Load(string path)
    {
        var xs = new XmlSerializer(typeof(Product[]));
        using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        return (Product[])xs.Deserialize(fs)!;
    }

    static void Main()
    {
        string path = "products.xml";
        var products = new[] { new Product(1, "Shoe", 49.99m), new Product(2, "Hat", 15.5m) };
        Save(path, products);
        var read = Load(path);
        Console.WriteLine($"Read {read.Length} products. First: {read[0].Name} - {read[0].Price:C}");
    }
}