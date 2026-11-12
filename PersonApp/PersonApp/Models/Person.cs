using System.ComponentModel.DataAnnotations;

namespace PersonApp.Models;

public class Person {
  public int Id { get; set; }

  [Required(ErrorMessage = "Navn er påkrævet")]
  [StringLength(100, ErrorMessage = "Navn må højst være 100 tegn")]
  public string Name { get; set; } = string.Empty;

  public string ImageFileName { get; set; } = string.Empty;
}
