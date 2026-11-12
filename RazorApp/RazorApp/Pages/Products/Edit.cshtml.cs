using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorApp.Models.Products;
using RazorApp.Repositories;

namespace RazorApp.Pages.Products;

public class EditModel : PageModel {
  private readonly IProductRepository _repository;

  public EditModel(IProductRepository repository) {
    _repository = repository;
  }

  [BindProperty]
  public ProductFormViewModel Input { get; set; } = new();

  public IActionResult OnGet(int id) {
    Product? product = _repository.GetById(id);

    if (product is null)
      return NotFound();

    Input.Name = product.Name;
    return Page();
  }

  public IActionResult OnPost(int id) {
    if (!ModelState.IsValid)
      return Page();

    Product? existing = _repository.GetById(id);

    if (existing is null)
      return NotFound();

    existing.Name = Input.Name;
    _repository.Update(existing);

    TempData["Success"] = $"Produktet \"{existing.Name}\" er opdateret.";
    return RedirectToPage("Index");
  }
}
