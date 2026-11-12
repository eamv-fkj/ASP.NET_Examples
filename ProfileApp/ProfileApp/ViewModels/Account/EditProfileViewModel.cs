using System.ComponentModel.DataAnnotations;

namespace ProfileApp.ViewModels.Account {

  public class EditProfileViewModel {
    [Required]
    public string Id { get; set; } = string.Empty;

    [Required(ErrorMessage = "Dit nuværende password er påkrævet")]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Du skal vælge et nyt password")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Du skal gentage dit nye password")]
    [Compare("NewPassword", ErrorMessage = "Passwords ser ikke ens")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;
  }
}
