// JsonSerializeDeserialize.cs
// Problem: JsonSerializeDeserialize
// Serialize/deserialize List<Product> using System.Text.Json with options (camelCase, ignore null).
// Complexity: O(n) serialization cost; schema evolution: prefer additive changes and DTOs.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

record Product(int Id, string Name, decimal Price);

class JsonSerializeDeserialize
{
    static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true
    };

    public static async Task SaveProductsAsync(string path, IEnumerable<Product> products)
    {
        using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 81920, useAsync: true);
        await JsonSerializer.SerializeAsync(fs, products, Options);
        await fs.FlushAsync();
    }

    public static async Task<List<Product>> LoadProductsAsync(string path)
    {
        using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 81920, useAsync: true);
        return (await JsonSerializer.DeserializeAsync<List<Product>>(fs, Options)) ?? new List<Product>();
    }

    static async Task Main()
    {
        string path = "products.json";
        var products = new List<Product> { new(1, "Shoe", 49.99m), new(2, "Hat", 15.50m) };
        await SaveProductsAsync(path, products);
        var loaded = await LoadProductsAsync(path);
        Console.WriteLine("Loaded products:");
        loaded.ForEach(p => Console.WriteLine($"{p.Id}: {p.Name} - {p.Price:C}"));
    }
}