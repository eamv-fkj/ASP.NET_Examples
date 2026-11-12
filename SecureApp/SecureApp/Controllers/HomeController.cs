using Microsoft.AspNetCore.Mvc;

namespace SecureApp.Controllers;

public class HomeController : Controller {

  public IActionResult Index() {
    return View();
  }
}
