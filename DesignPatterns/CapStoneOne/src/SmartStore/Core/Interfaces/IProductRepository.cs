namespace SmartStore.Core.Interfaces;

// -------------------------------------------------------
// REPOSITORY PATTERN
// -------------------------------------------------------
public interface IProductRepository
{
    Product? GetById(int id);
    IEnumerable<Product> GetAll();
    IEnumerable<Product> GetByCategory(string category);
}
