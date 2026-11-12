using Microsoft.AspNetCore.Mvc;
using WizardApp.Models;

namespace WizardApp.Controllers;

public class HomeController : Controller {
  private const string _nameKey = "Wizard.Name";
  private const string _emailKey = "Wizard.Email";
  private const string _educationKey = "Wizard.Education";
  private const string _semesterKey = "Wizard.Semester";

  public IActionResult Index() {
    PersonalInfoViewModel model = new() {
      Name = HttpContext.Session.GetString(_nameKey) ?? string.Empty,
      Email = HttpContext.Session.GetString(_emailKey) ?? string.Empty
    };

    return View(model);
  }

  [HttpPost]
  public IActionResult Index(PersonalInfoViewModel model) {
    if (!ModelState.IsValid)
      return View(model);

    HttpContext.Session.SetString(_nameKey, model.Name);
    HttpContext.Session.SetString(_emailKey, model.Email);

    return RedirectToAction(nameof(Step2));
  }

  public IActionResult Step2() {
    EducationViewModel model = new() {
      Education = HttpContext.Session.GetString(_educationKey) ?? string.Empty,
      Semester = HttpContext.Session.GetInt32(_semesterKey) ?? 1
    };

    return View(model);
  }

  [HttpPost]
  public IActionResult Step2(EducationViewModel model) {
    if (!ModelState.IsValid)
      return View(model);

    HttpContext.Session.SetString(_educationKey, model.Education);
    HttpContext.Session.SetInt32(_semesterKey, model.Semester);

    return RedirectToAction(nameof(Summary));
  }

  public IActionResult Summary() {
    WizardViewModel model = new() {
      Name = HttpContext.Session.GetString(_nameKey) ?? string.Empty,
      Email = HttpContext.Session.GetString(_emailKey) ?? string.Empty,
      Education = HttpContext.Session.GetString(_educationKey) ?? string.Empty,
      Semester = HttpContext.Session.GetInt32(_semesterKey) ?? 1
    };

    return View(model);
  }

  [HttpPost]
  public IActionResult Reset() {
    HttpContext.Session.Clear();

    return RedirectToAction(nameof(Index));
  }
}
