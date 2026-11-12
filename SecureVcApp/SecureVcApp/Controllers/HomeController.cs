using Microsoft.AspNetCore.Mvc;

namespace SecureVcApp.Controllers;

public class HomeController : Controller {

  public IActionResult Index() {
    return View();
  }
}
