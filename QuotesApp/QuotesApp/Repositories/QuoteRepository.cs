using QuotesApp.Models;

namespace QuotesApp.Repositories;

public class QuoteRepository : IQuoteRepository {

    public IEnumerable<Quote> GetAll() {
    return _quotes.ToList();
  }

  public Quote GetRandom() {
    return _quotes[Random.Shared.Next(_quotes.Count)];
  }

  private static readonly List<Quote> _quotes = [
    new() {
      Text = "Premature optimization is the root of all evil.",
      Author = "Donald Knuth",
      Category = "Tech"
    },
    new() {
      Text = "The unexamined life is not worth living.",
      Author = "Socrates",
      Category = "Life"
    },
    new() {
      Text = "Simplicity is prerequisite for reliability.",
      Author = "Edsger W. Dijkstra",
      Category = "Tech"
    },
    new() {
      Text = "All the world's a stage, and all the men and women merely players.",
      Author = "William Shakespeare",
      Category = "Life"
    },
    new() {
      Text = "First, solve the problem. Then, write the code.",
      Author = "John Johnson",
      Category = "Tech"
    },
    new() {
      Text = "The only thing we have to fear is fear itself.",
      Author = "Franklin D. Roosevelt",
      Category = "Life"
    },
    new() {
      Text = "Make it work, make it right, make it fast.",
      Author = "Johnson & Kernighan",
      Category = "Tech"
    },
    new() {
      Text = "Talk is cheap. Show me the code.",
      Author = "Linus Torvalds",
      Category = "Tech"
    }
  ];
}
