using Microsoft.AspNetCore.Mvc;

namespace CounterApp.Controllers;

public class HomeController : Controller {

  public IActionResult Index() {
    return View(0);
  }

  [HttpPost]
  public IActionResult Index(int number, string command) {
    number = command switch {
      "minus" => number - 1,
      "plus" => number + 1,
      "reset" => 0,
      _ => number
    };

    return View(number);
  }
}
