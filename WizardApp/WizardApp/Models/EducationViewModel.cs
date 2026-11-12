using System.ComponentModel.DataAnnotations;

namespace WizardApp.Models;

public class EducationViewModel {
  [Required(ErrorMessage = "Uddannelse er påkrævet.")]
  public string Education { get; set; } = string.Empty;

  [Required(ErrorMessage = "Semester er påkrævet.")]
  [Range(1, 5, ErrorMessage = "Semester skal være mellem 1 og 5.")]
  public int Semester { get; set; } = 1;
}
