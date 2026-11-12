namespace SecureApp.Models;

using Microsoft.AspNetCore.Identity;

public class AppUser : IdentityUser {
  public string? FirstName { get; set; }
  public string? LastName { get; set; }

  // Collection Navigation Property
  public ICollection<Order> Orders { get; set; } = [];
}
