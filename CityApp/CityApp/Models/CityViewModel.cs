using Microsoft.AspNetCore.Mvc.Rendering;

namespace CityApp.Models;

public class CityViewModel {
  public string SelectedItemId { get; set; } = string.Empty;

  public IEnumerable<SelectListItem> Cities { get; set; } = [];
}
