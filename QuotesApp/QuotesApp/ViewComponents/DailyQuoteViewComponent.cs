using Microsoft.AspNetCore.Mvc;
using QuotesApp.Models;
using QuotesApp.Repositories;

namespace QuotesApp.ViewComponents;

public class DailyQuoteViewComponent : ViewComponent {
  private readonly IQuoteRepository _quoteRepository;

  public DailyQuoteViewComponent(IQuoteRepository quoteRepository) {
    _quoteRepository = quoteRepository;
  }

  public IViewComponentResult Invoke() {
    Quote quote = _quoteRepository.GetRandom();
    return View(quote);
  }
}
