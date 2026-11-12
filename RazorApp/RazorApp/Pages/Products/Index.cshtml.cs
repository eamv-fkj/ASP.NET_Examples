using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorApp.Models.Products;
using RazorApp.Repositories;

namespace RazorApp.Pages.Products;

public class IndexModel : PageModel {
  private readonly IProductRepository _repository;

  public IndexModel(IProductRepository repository) {
    _repository = repository;
  }

  public List<Product> Products { get; private set; } = [];

  public void OnGet() {
    Products = _repository.GetAll().ToList();
  }
}
