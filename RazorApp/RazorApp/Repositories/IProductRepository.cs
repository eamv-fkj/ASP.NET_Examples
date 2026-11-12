using RazorApp.Models.Products;

namespace RazorApp.Repositories;

public interface IProductRepository {
  IEnumerable<Product> GetAll();
  Product? GetById(int id);
  Product Add(Product product);
  bool Update(Product product);
  bool Delete(int id);
  bool Exists(int id);
}
