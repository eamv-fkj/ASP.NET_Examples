using BookApp.Models.Books;
using Moq;

namespace BookApp.Test.Models.Books;

public class BookBaseMoqTests {

  [Fact]
  public void SetupGet_can_mock_several_properties_at_once() {
    // Arrange
    Mock<BookBase> mock = new();
    mock.SetupGet(b => b.Title).Returns("The Genius of Birds");
    mock.SetupGet(b => b.Author).Returns("Jennifer Ackerman");
    mock.SetupGet(b => b.Pages).Returns(336);
    mock.SetupGet(b => b.Price).Returns(349.95m);
    mock.SetupGet(b => b.Published).Returns(2024);

    // Act
    BookBase book = mock.Object;

    // Assert
    Assert.Equal("The Genius of Birds", book.Title);
    Assert.Equal("Jennifer Ackerman", book.Author);
    Assert.Equal(336, book.Pages);
    Assert.Equal(349.95m, book.Price);
    Assert.Equal(2024, book.Published);
  }

  [Fact]
  public void A_mocked_property_can_return_a_value_that_depends_on_a_counter() {
    // Arrange
    int reads = 1;
    Mock<BookBase> mock = new();
    mock.SetupGet(b => b.Title).Returns(() => $"Title #{reads++}");
    BookBase book = mock.Object;

    // Act + Assert
    Assert.Equal("Title #1", book.Title);
    Assert.Equal("Title #2", book.Title);
    Assert.Equal("Title #3", book.Title);
  }

  [Fact]
  public void SetupProperty_lets_the_value_be_set_and_then_read_back() {
    // Arrange
    Mock<BookBase> mock = new();
    mock.SetupProperty(b => b.Title, "The Genius of Birds");
    BookBase book = mock.Object;

    // Act
    book.Title = "What It's Like to Be a Bird";

    // Assert
    Assert.Equal("What It's Like to Be a Bird", book.Title);
  }

  [Fact]
  public void CallBase_makes_the_property_use_the_real_behaviour_instead() {
    // Arrange
    Mock<BookBase> mock = new() { CallBase = true };
    BookBase book = mock.Object;

    // Act
    book.Title = "The Sibley Guide to Birds";

    // Assert: den rigtige property er brugt, så værdien er gemt
    Assert.Equal("The Sibley Guide to Birds", book.Title);
  }
}