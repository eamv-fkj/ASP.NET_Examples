using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SecureVcApp.Models;
using SecureVcApp.ViewModels.Account;

namespace SecureVcApp.Controllers;

public class AccountController : Controller {
  private readonly UserManager<AppUser> _userManager;
  private readonly SignInManager<AppUser> _signInManager;

  public AccountController(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager)
  {
    _userManager = userManager;
    _signInManager = signInManager;
  }

  // GET: /Account/Index
  public IActionResult Index() {
    List<AppUser> users = _userManager.Users.OrderBy(u => u.FirstName).ToList();

    return View(users);
  }

  // GET: /Account/Details/5
  public IActionResult Details(string id) {
    if (string.IsNullOrEmpty(id))
      return NotFound();

    AppUser? user = _userManager.FindByIdAsync(id).Result;

    if (user is null)
      return NotFound();

    return View(user);
  }

  // GET: /Account/Edit/5
  public IActionResult Edit(string id) {
    if (string.IsNullOrEmpty(id))
      return NotFound();

    AppUser? user = _userManager.FindByIdAsync(id).Result;

    if (user is null)
      return NotFound();

    AccountEditViewModel model = new() {
      Id = user.Id,
      FirstName = user.FirstName ?? string.Empty,
      LastName = user.LastName ?? string.Empty,
      Email = user.Email ?? string.Empty
    };

    return View(model);
  }

  // POST: /Account/Edit/5
  [HttpPost]
  public IActionResult Edit(AccountEditViewModel model) {
    if (!ModelState.IsValid)
      return View(model);

    if (string.IsNullOrEmpty(model.Id))
      return NotFound();

    AppUser? user = _userManager.FindByIdAsync(model.Id).Result;

    if (user is null)
      return NotFound();

    user.FirstName = model.FirstName;
    user.LastName = model.LastName;
    user.Email = model.Email;
    user.UserName = model.Email;

    IdentityResult result = _userManager.UpdateAsync(user).Result;

    if (!result.Succeeded) {
      AddErrors(result);

      return View(model);
    }

    TempData["Success"] = "Account opdateret.";

    return RedirectToAction(nameof(Index));
  }

  // GET: /Account/Delete/5
  [Authorize(Roles = "Admin, Owner")]
  public IActionResult Delete(string id) {
    if (string.IsNullOrEmpty(id))
      return NotFound();

    AppUser? user = _userManager.FindByIdAsync(id).Result;

    if (user is null)
      return NotFound();

    return View(user);
  }

  // POST: /Account/Delete/5
  [HttpPost, ActionName("Delete")]
  [Authorize(Roles = "Admin, Owner")]
  public IActionResult DeleteConfirmed([FromForm] string id) {
    if (string.IsNullOrEmpty(id))
      return NotFound();

    AppUser? user = _userManager.FindByIdAsync(id).Result;

    if (user is null)
      return NotFound();

    IdentityResult result = _userManager.DeleteAsync(user).Result;

    if (!result.Succeeded) {
      AddErrors(result);

      return View(user);
    }

    TempData["Success"] = "Account slettet.";

    return RedirectToAction(nameof(Index));
  }

  // GET: /Account/Register
  public IActionResult Register() {
    return View(new RegisterViewModel());
  }

  // POST: /Account/Register
  [HttpPost]
  public IActionResult Register(RegisterViewModel model) {
    if (!ModelState.IsValid)
      return View(model);

    AppUser user = new() {
      UserName = model.Email,
      Email = model.Email,
      FirstName = model.FirstName,
      LastName = model.LastName
    };

    IdentityResult result = _userManager.CreateAsync(user, model.Password).Result;

    if (result.Succeeded) {
      _userManager.AddToRoleAsync(user, "User").Wait();
      _signInManager.SignInAsync(user, isPersistent: false).Wait();

      TempData["Success"] = "Du er nu registreret og logget ind.";

      return RedirectToAction(nameof(Index));
    }

    AddErrors(result);

    return View(model);
  }

  // GET: /Account/Login
  public IActionResult Login() {
    return View(new LoginViewModel());
  }

  // POST: /Account/Login
  [HttpPost]
  public IActionResult Login(LoginViewModel model) {
    if (!ModelState.IsValid)
      return View(model);

    Microsoft.AspNetCore.Identity.SignInResult result =
      _signInManager.PasswordSignInAsync(
        model.Email,
        model.Password,
        model.RememberMe,
        lockoutOnFailure: false
      ).Result;

    if (!result.Succeeded) {
      ModelState.AddModelError(string.Empty, "Ugyldig login.");

      return View(model);
    }

    return RedirectToAction("Index", "Order");
  }

  // POST: /Account/Logout
  [HttpPost]
  public IActionResult Logout() {
    _signInManager.SignOutAsync().Wait();
    
    return RedirectToAction("Index", "Home");
  }

  public IActionResult AccessDenied() {
    return View();
  }

  private void AddErrors(IdentityResult result) {
    foreach (IdentityError error in result.Errors)
      ModelState.AddModelError(string.Empty, error.Description);
  }
}
