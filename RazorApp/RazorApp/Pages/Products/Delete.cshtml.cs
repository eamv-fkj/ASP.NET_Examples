using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorApp.Models.Products;
using RazorApp.Repositories;

namespace RazorApp.Pages.Products;

public class DeleteModel : PageModel {
  private readonly IProductRepository _repository;

  public DeleteModel(IProductRepository repository) {
    _repository = repository;
  }

  public Product? Product { get; private set; }

  public IActionResult OnGet(int id) {
    Product = _repository.GetById(id);

    if (Product is null)
      return NotFound();

    return Page();
  }

  public IActionResult OnPost(int id) {
    Product? product = _repository.GetById(id);

    if (product is null)
      return NotFound();

    _repository.Delete(id);

    TempData["Success"] = $"Produktet \"{product.Name}\" er slettet.";
    
    return RedirectToPage("Index");
  }
}
