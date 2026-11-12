using BookApp.Models.Books;
using BookApp.Repositories;

namespace BookApp.Test.Repositories;

public class BookRepositoryTests {
  [Theory]
  [InlineData(0, "Dune")]
  [InlineData(1, "The Hobbit")]
  [InlineData(2, "Foundation")]
  public void GetById_returns_the_seeded_book(int id, string expectedTitle) {
    // Arrange
    IBookRepository repository = new BookRepository();

    // Act
    Book? book = repository.GetById(id);

    // Assert
    Assert.NotNull(book);
    Assert.Equal(expectedTitle, book.Title);
  }
}