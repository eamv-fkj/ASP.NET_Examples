using System.ComponentModel.DataAnnotations;

namespace SecureApp.ViewModels.Order;

public class OrderCreateViewModel {
  [Required(ErrorMessage = "Produktnavn er påkrævet")]
  public string ProductName { get; set; } = string.Empty;

  [Range(1, 100, ErrorMessage = "Antallet skal være mellem 1 og 100")]
  public int Quantity { get; set; }
}
