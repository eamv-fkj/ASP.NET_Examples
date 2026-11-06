using CalcApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CalcApp.Controllers;

public class HomeController : Controller {

  public IActionResult Index() {
    CalcViewModel model = new() {
      OperatorList = GetOperators()
    };

    return View(model);
  }

  [HttpPost]
  public IActionResult Index(CalcViewModel model) {
    model.OperatorList = GetOperators();

    if (!ModelState.IsValid)
      // user must try again
      return View(model);

    double op1 = model.Operand1;
    double op2 = model.Operand2;
    string? opr = model.SelectedOperator;

    if (op2 == 0 && (opr == "/" || opr == "%")) {
      ModelState.AddModelError(nameof(model.Operand2),
          "Nævner må ikke være 0 ved division eller modulus.");
      return View(model);
    }

    model.Result  = opr switch {
      "+" => op1 + op2,
      "-" => op1 - op2,
      "*" => op1 * op2,
      "/" => op1 / op2,
      "%" => op1 % op2,
      _ => double.NaN,
    };

    //ModelState.Remove(nameof(model.Result));

    return View(model);
  }

  private static IEnumerable<SelectListItem> GetOperators() {
    return [
      new() { Value = "+", Text = "+" },
      new() { Value = "-", Text = "-" },
      new() { Value = "*", Text = "*" },
      new() { Value = "/", Text = "/" },
      new() { Value = "%", Text = "%" }
    ];
  }
}
