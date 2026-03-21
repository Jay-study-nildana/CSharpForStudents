namespace SmartStore.Patterns.Structural;

// ================================================================
// PROXY PATTERN
// ================================================================
// CachedProductRepositoryProxy controls access to the real product
// repository and caches results to avoid repeated "expensive" loads.
//
// Intent   : Provide a surrogate or placeholder for another object
//            to control access to it.
// Problem  : Every time we browse products the cost of hitting the
//            real data source is high. We want lazy-load + caching.
// Solution : The proxy implements IProductRepository, intercepts calls,
//            loads from the real repo on first access, and serves from
//            cache on subsequent accesses.
// ================================================================
public class CachedProductRepositoryProxy : IProductRepository
{
    private readonly IProductRepository _real;
    private List<Product>? _cache;

    public CachedProductRepositoryProxy(IProductRepository real) => _real = real;

    public IEnumerable<Product> GetAll()
    {
        if (_cache is null)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  [Proxy] Cache MISS — loading products from real repository...");
            Console.ResetColor();
            _cache = _real.GetAll().ToList();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  [Proxy] Cache HIT — returning cached product list.");
            Console.ResetColor();
        }
        return _cache;
    }

    public Product? GetById(int id)
    {
        if (_cache is null) GetAll(); // warm cache first
        return _cache!.FirstOrDefault(p => p.Id == id);
    }

    public IEnumerable<Product> GetByCategory(string category)
    {
        if (_cache is null) GetAll();
        return _cache!.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>Explicitly invalidates the cache (e.g., after stock changes).</summary>
    public void InvalidateCache()
    {
        _cache = null;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("  [Proxy] Cache invalidated.");
        Console.ResetColor();
    }
}
