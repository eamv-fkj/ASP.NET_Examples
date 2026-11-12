using System.ComponentModel.DataAnnotations;

namespace SecureVcApp.ViewModels.Account;

public class AccountEditViewModel {
  [Required]
  public string Id { get; set; } = string.Empty;

  [Required(ErrorMessage = "Fornavn er påkrævet")]
  public string FirstName { get; set; } = string.Empty;

  [Required(ErrorMessage = "Efternavn er påkrævet")]
  public string LastName { get; set; } = string.Empty;

  [Required(ErrorMessage = "Email er påkrævet")]
  [EmailAddress(ErrorMessage = "Ugyldig email")]
  public string Email { get; set; } = string.Empty;
}
