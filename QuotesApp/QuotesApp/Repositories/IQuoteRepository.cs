using QuotesApp.Models;

namespace QuotesApp.Repositories;

public interface IQuoteRepository {
  IEnumerable<Quote> GetAll();
  Quote GetRandom();
}
