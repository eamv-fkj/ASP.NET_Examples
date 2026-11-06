using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BookApp.Test.TestInfrastructure;

// TempData kræver en HttpContext med en service provider.
// Provideren må kun bruges af én controller ad gangen.
public sealed class InMemoryTempDataProvider : ITempDataProvider {
  private readonly Dictionary<string, object> _values = [];

  public IDictionary<string, object> LoadTempData(HttpContext context) => _values;

  public void SaveTempData(HttpContext context, IDictionary<string, object> values) {
    _values.Clear();

    foreach (KeyValuePair<string, object> entry in values)
      _values[entry.Key] = entry.Value;
  }
}