using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CalcApp.Models;

public class CalcViewModel {
  public double Operand1 { get; set; }
  public double Operand2 { get; set; }

  [Required(ErrorMessage = "Der skal vælges en operator")]
  public string? SelectedOperator { get; set; }
  public IEnumerable<SelectListItem> OperatorList { get; set; } = [];

  public double Result { get; set; }

  /*
  public double Result {
    get {
      return SelectedOperator switch {
        "+" => Operand1 + Operand2,
        "-" => Operand1 - Operand2,
        "*" => Operand1 * Operand2,
        "/" => Operand1 / Operand2,
        "%" => Operand1 % Operand2,
        _ => double.NaN,
      };
    }
  }
  */
}
