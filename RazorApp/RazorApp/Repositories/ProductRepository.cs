using RazorApp.Models.Products;

namespace RazorApp.Repositories;

public class ProductRepository : IProductRepository {
  private readonly List<Product> _products = [];
  private int _nextId = 1;

  public ProductRepository() {
    SeedData();
  }

  public IEnumerable<Product> GetAll() => _products.ToList();

  public Product? GetById(int id) => _products.FirstOrDefault(p => p.Id == id);

  public Product Add(Product product) {
    product.Id = _nextId++;

    _products.Add(product);

    return product;
  }

  public bool Update(Product product) {
    Product? existing = GetById(product.Id);

    if (existing is null)
      return false;

    existing.Name = product.Name;

    return true;
  }

  public bool Delete(int id) {
    Product? product = GetById(id);
    
    return product is not null && _products.Remove(product);
  }

  public bool Exists(int id) => GetById(id) is not null;

  private void SeedData() {
    Add(new Product { Name = "Skærm" });
    Add(new Product { Name = "Tastatur" });
    Add(new Product { Name = "Mus" });
  }
}
