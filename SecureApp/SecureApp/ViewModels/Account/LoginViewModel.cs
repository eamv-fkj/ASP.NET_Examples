using System.ComponentModel.DataAnnotations;

namespace SecureApp.ViewModels.Account;

public class LoginViewModel {
  [Required(ErrorMessage = "Email er påkrævet")]
  [EmailAddress(ErrorMessage = "Ugyldig email")]
  public string Email { get; set; } = string.Empty;

  [Required(ErrorMessage = "Adgangskode er påkrævet")]
  [DataType(DataType.Password)]
  public string Password { get; set; } = string.Empty;

  public bool RememberMe { get; set; }
}
