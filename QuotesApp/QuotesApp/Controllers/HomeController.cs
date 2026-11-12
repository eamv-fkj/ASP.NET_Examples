using Microsoft.AspNetCore.Mvc;
using QuotesApp.Models;
using QuotesApp.Repositories;

namespace QuotesApp.Controllers;

public class HomeController : Controller {
  private readonly IQuoteRepository _quoteRepository;

  public HomeController(IQuoteRepository quoteRepository) {
    _quoteRepository = quoteRepository;
  }

  // GET: /
  public IActionResult Index() {
    IEnumerable<Quote> quotes = _quoteRepository.GetAll();
    return View(quotes);
  }
}
