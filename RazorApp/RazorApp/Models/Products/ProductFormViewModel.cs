using System.ComponentModel.DataAnnotations;

namespace RazorApp.Models.Products;

public class ProductFormViewModel {
  [Required(ErrorMessage = "Navn er påkrævet.")]
  [StringLength(100, MinimumLength = 2,
      ErrorMessage = "Navn skal være mellem 2 og 100 tegn.")]
  public string Name { get; set; } = string.Empty;
}
