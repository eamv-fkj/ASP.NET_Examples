using Microsoft.AspNetCore.Mvc;

namespace ProfileApp.Controllers;

public class HomeController : Controller {

  public IActionResult Index() {
    return View();
  }
}
