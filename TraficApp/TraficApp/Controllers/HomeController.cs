using Microsoft.AspNetCore.Mvc;

namespace TraficApp.Controllers;

public class HomeController : Controller {

  public IActionResult Index() {
    return View(model: "Red");
  }

  [HttpPost]
  public IActionResult Index(string color, string command) {
    ViewData["Message"] = null;

    if (string.IsNullOrEmpty(color))
      color = "Red";

    if (command == "Go") {
      if (color == "Green")
        ViewData["Message"] = "Det lykkedes!";

      return View(model: color);
    }

    color = command switch {
      "Rød" => "Red",
      "Gul" => "Yellow",
      "Grøn" => "Green",
      _ => color
    };

    return View(model: color);
  }
}
