using System.ComponentModel.DataAnnotations;

namespace WizardApp.Models;

public class PersonalInfoViewModel {
  [Required(ErrorMessage = "Navn er påkrævet.")]
  public string Name { get; set; } = string.Empty;

  [Required(ErrorMessage = "Email er påkrævet.")]
  [EmailAddress(ErrorMessage = "Indtast en gyldig email-adresse.")]
  public string Email { get; set; } = string.Empty;
}
