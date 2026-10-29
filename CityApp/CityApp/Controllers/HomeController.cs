using CityApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CityApp.Controllers;

public class HomeController : Controller {

  public IActionResult Index() {
    CityViewModel model = new() {
      Cities = GetCities()
    };

    return View(model);
  }

  [HttpPost]
  public IActionResult Index(CityViewModel model) {
    if (!ModelState.IsValid) {
      model.Cities = GetCities();
      return View(model);
    }

    return RedirectToAction("Success", new { id = model.SelectedItemId });
  }

  public IActionResult Success(string id) {
    return View(model: id);
  }

  private static IEnumerable<SelectListItem> GetCities() {
    return [
      new() { Value = "1", Text = "Herning" },
      new() { Value = "2", Text = "Holstebro" },
      new() { Value = "3", Text = "Ikast" }
    ];
  }
}
