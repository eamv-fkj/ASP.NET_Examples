using System.ComponentModel.DataAnnotations;

namespace SecureVcApp.ViewModels.Order;

public class OrderEditViewModel {
  [Range(1, int.MaxValue, ErrorMessage = "Ugyldigt ordre-id")]
  public int Id { get; set; }

  [Required(ErrorMessage = "Produktnavn er påkrævet")]
  public string ProductName { get; set; } = string.Empty;

  [Range(1, 100, ErrorMessage = "Antallet skal være mellem 1 og 100")]
  public int Quantity { get; set; }
}
