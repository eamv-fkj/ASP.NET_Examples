using BookApp.Models.Books;

namespace BookApp.Test.Models.Books;

public class BookTests {
  [Fact]
  public void Title_is_empty_on_new_book() {
    // Arrange
    Book book = new();

    // Act
    string title = book.Title;

    // Assert
    Assert.Equal(string.Empty, title);
  }

  [Fact]
  public void Title_returns_the_value_that_was_set() {
    // Arrange
    Book book = new();

    // Act
    book.Title = "Dune";

    // Assert
    Assert.Equal("Dune", book.Title);
  }

  [Theory]
  [InlineData(1)]
  [InlineData(256)]
  [InlineData(412)]
  public void Pages_returns_the_value_that_was_set(int pages) {
    // Arrange
    Book book = new();

    // Act
    book.Pages = pages;

    // Assert
    Assert.Equal(pages, book.Pages);
  }
}
