using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace BookApp.Test.TestInfrastructure;

public static class TestController {

  // Giver en controller den ControllerContext den kræver for
  // f.eks. at bruge TempData og RedirectToAction
  public static TController WithContext<TController>(TController controller) where TController : Controller {
    ServiceCollection services = new();

    // AddMvcCore registrerer de afhængigheder MVC selv har brug for,
    // fx IUrlHelperFactory som RedirectToAction kalder.
    services.AddMvcCore();

    // Transient, fordi hver controller skal have sin egen TempData-butik.
    // xUnit kører tests parallelt, så en delt instance ville blande testene sammen.
    services.AddTransient<ITempDataProvider, InMemoryTempDataProvider>();
    services.AddTransient<ITempDataDictionaryFactory, TempDataDictionaryFactory>();

    controller.ControllerContext = new ControllerContext {
      HttpContext = new DefaultHttpContext {
        RequestServices = services.BuildServiceProvider()
      },
      RouteData = new RouteData(),
      ActionDescriptor = new ControllerActionDescriptor()
    };

    return controller;
  }
}