using System.ComponentModel.DataAnnotations;

namespace ProfileApp.ViewModels.Account;

public class RegisterViewModel {
  [Required(ErrorMessage = "Fornavn er påkrævet")]
  public string FirstName { get; set; } = string.Empty;

  [Required(ErrorMessage = "Efternavn er påkrævet")]
  public string LastName { get; set; } = string.Empty;

  [Required(ErrorMessage = "Email er påkrævet")]
  [EmailAddress(ErrorMessage = "Ugyldig email")]
  public string Email { get; set; } = string.Empty;

  [Required(ErrorMessage = "Password er påkrævet")]
  [DataType(DataType.Password)]
  [StringLength(100, ErrorMessage = "Password skal være mindst otte tegn lang", MinimumLength = 8)]
  public string Password { get; set; } = string.Empty;

  [Required(ErrorMessage = "Bekræft password")]
  [DataType(DataType.Password)]
  [Compare("Password", ErrorMessage = "Passwords ser ikke ens")]
  public string ConfirmPassword { get; set; } = string.Empty;
}
