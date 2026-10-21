using System.ComponentModel.DataAnnotations;

namespace EFApp.Models;

public class Person {

  public int Id { get; set; }

  [Required(ErrorMessage = "Navn er påkrævet")]
  [StringLength(100, ErrorMessage = "Navn må højst være 100 tegn")]
  public string Name { get; set; } = string.Empty;

  [Required(ErrorMessage = "Alder er påkrævet")]
  [Range(0, 150, ErrorMessage = "Alder skal være mellem 0 og 150")]
  public int Age { get; set; }

  [Required(ErrorMessage = "By er påkrævet")]
  [StringLength(100, ErrorMessage = "By må højst være 100 tegn")]
  public string City { get; set; } = string.Empty;
}
