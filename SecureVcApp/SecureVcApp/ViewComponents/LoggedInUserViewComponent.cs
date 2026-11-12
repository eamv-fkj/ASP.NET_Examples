using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SecureVcApp.Models;

namespace SecureVcApp.ViewComponents;

public class LoggedInUserViewComponent : ViewComponent {
  private readonly UserManager<AppUser> _userManager;

  public LoggedInUserViewComponent(UserManager<AppUser> userManager) {
    _userManager = userManager;
  }

  public IViewComponentResult Invoke() {
    AppUser? user = _userManager.GetUserAsync(UserClaimsPrincipal).Result;

    return View(user);
  }
}
