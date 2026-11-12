using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorApp.Models.Products;
using RazorApp.Repositories;

namespace RazorApp.Pages.Products;

public class CreateModel : PageModel {
  private readonly IProductRepository _repository;

  public CreateModel(IProductRepository repository) {
    _repository = repository;
  }

  [BindProperty]
  public ProductFormViewModel Input { get; set; } = new();

  public void OnGet() {
  }

  public IActionResult OnPost() {
    if (!ModelState.IsValid)
      return Page();

    Product product = new() { Name = Input.Name };
    _repository.Add(product);

    TempData["Success"] = $"Produktet \"{product.Name}\" er oprettet.";
    return RedirectToPage("Index");
  }
}
