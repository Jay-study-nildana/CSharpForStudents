namespace SmartStore.Infrastructure;

// -------------------------------------------------------
// REPOSITORY PATTERN — In-Memory implementation
// Seeded with realistic sample data for the demo.
// -------------------------------------------------------
public class InMemoryProductRepository : IProductRepository
{
    private static readonly List<Product> _products = new()
    {
        new Product { Id = 1,  Name = "Laptop Pro 15",   Price =  999.99m, Stock = 10, Category = "Electronics" },
        new Product { Id = 2,  Name = "Wireless Mouse",  Price =   29.99m, Stock = 50, Category = "Electronics" },
        new Product { Id = 3,  Name = "Mechanical Keyboard", Price = 79.99m, Stock = 30, Category = "Electronics" },
        new Product { Id = 4,  Name = "Ergonomic Chair", Price =  249.99m, Stock =  5, Category = "Furniture"   },
        new Product { Id = 5,  Name = "4K Monitor",      Price =  399.99m, Stock =  8, Category = "Electronics" },
        new Product { Id = 6,  Name = "Notebook A5",     Price =    4.99m, Stock = 100, Category = "Stationery" },
        new Product { Id = 7,  Name = "7-Port USB Hub",  Price =   19.99m, Stock = 40, Category = "Electronics" },
        new Product { Id = 8,  Name = "HD Webcam",       Price =   79.99m, Stock = 15, Category = "Electronics" },
    };

    public Product? GetById(int id) =>
        _products.FirstOrDefault(p => p.Id == id);

    public IEnumerable<Product> GetAll() =>
        _products.AsReadOnly();

    public IEnumerable<Product> GetByCategory(string category) =>
        _products.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
}
