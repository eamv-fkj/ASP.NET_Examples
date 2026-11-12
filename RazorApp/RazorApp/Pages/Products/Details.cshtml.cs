using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorApp.Models.Products;
using RazorApp.Repositories;

namespace RazorApp.Pages.Products;

public class DetailsModel : PageModel {
  private readonly IProductRepository _repository;

  public DetailsModel(IProductRepository repository) {
    _repository = repository;
  }

  public Product? Product { get; private set; }

  public IActionResult OnGet(int id) {
    Product = _repository.GetById(id);

    if (Product is null)
      return NotFound();

    return Page();
  }
}
