namespace SecureApp.Models;

public class Order {
  public int Id { get; set; }
  public string ProductName { get; set; } = string.Empty;
  public int Quantity { get; set; }
  public DateTime OrderDate { get; set; }

  // Foreign Key til ApplicationUser
  public string? UserId { get; set; }

  // Reference Navigation Property
  public AppUser? User { get; set; }
}