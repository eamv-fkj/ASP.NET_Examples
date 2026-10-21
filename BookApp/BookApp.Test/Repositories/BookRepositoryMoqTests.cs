using BookApp.Models.Books;
using BookApp.Repositories;
using Moq;

namespace BookApp.Test.Repositories;

public class BookRepositoryMoqTests {

  [Fact]
  public void Setup_can_return_a_different_value_per_id() {
    // Arrange
    Mock<IBookRepository> mock = new();
    mock.Setup(r => r.GetById(0)).Returns(new Book { Id = 0, Title = "The Sibley Guide to Birds" });
    mock.Setup(r => r.GetById(1)).Returns(new Book { Id = 1, Title = "The Genius of Birds" });
    mock.Setup(r => r.GetById(2)).Returns(new Book { Id = 2, Title = "What It's Like to Be a Bird" });
    IBookRepository repository = mock.Object;

    // Act
    string? title0 = repository.GetById(0)?.Title;
    string? title1 = repository.GetById(1)?.Title;
    string? title2 = repository.GetById(2)?.Title;

    // Assert
    Assert.Equal("The Sibley Guide to Birds", title0);
    Assert.Equal("The Genius of Birds", title1);
    Assert.Equal("What It's Like to Be a Bird", title2);
  }

  [Fact]
  public void Setup_can_match_the_argument_with_It_Is() {
    // Arrange
    Mock<IBookRepository> mock = new();
    mock.Setup(r => r.GetById(It.Is<int>(id => id < 3))).Returns(new Book { Title = "Ukendt bog" });
    IBookRepository repository = mock.Object;

    // Act
    string? title1 = repository.GetById(1)?.Title;

    // Assert
    Assert.Equal("Ukendt bog", title1);
    Assert.Null(repository.GetById(7));
  }

  [Fact]
  public void GetAll_can_return_a_list_the_mock_provides() {
    // Arrange
    Mock<IBookRepository> mock = new();
    mock.Setup(r => r.GetAll()).Returns(new List<Book> {
      new() { Id = 0, Title = "The Sibley Guide to Birds" },
      new() { Id = 1, Title = "The Genius of Birds" },
      new() { Id = 2, Title = "What It's Like to Be a Bird" },
    });
    IBookRepository repository = mock.Object;

    // Act
    List<Book> books = repository.GetAll().ToList();

    // Assert
    Assert.Equal(3, books.Count);
    Assert.Equal("The Sibley Guide to Birds", books[0].Title);
    Assert.Equal("The Genius of Birds", books[1].Title);
    Assert.Equal("What It's Like to Be a Bird", books[2].Title);
  }

  [Fact]
  public void Verify_proves_that_Add_was_called_with_the_right_book() {
    // Arrange
    Mock<IBookRepository> mock = new();
    IBookRepository repository = mock.Object;

    // Act
    repository.Add(new Book { Title = "The Sibley Guide to Birds" });

    // Assert
    mock.Verify(r => r.Add(It.Is<Book>(b => b.Title == "The Sibley Guide to Birds")), Times.Once);
  }

  [Fact]
  public void Verify_can_require_that_a_call_never_happened() {
    // Arrange
    Mock<IBookRepository> mock = new();
    IBookRepository repository = mock.Object;

    // Act: kun GetAll bliver kaldt
    repository.GetAll();

    // Assert
    mock.Verify(r => r.Delete(It.IsAny<int>()), Times.Never);
  }

  [Fact]
  public void Callback_can_capture_the_book_that_was_added() {
    // Arrange
    Mock<IBookRepository> mock = new();
    Book? added = null;

    mock.Setup(r => r.Add(It.IsAny<Book>()))
      .Callback<Book>(book => added = book)
      .Returns(new Book { Id = 7 });

    IBookRepository repository = mock.Object;

    // Act
    Book result = repository.Add(new Book { Title = "The Sibley Guide to Birds" });

    // Assert
    Assert.NotNull(added);
    Assert.Equal("The Sibley Guide to Birds", added.Title);
    Assert.Equal(7, result.Id);
  }

  [Fact]
  public void VerifyNoOtherCalls_proves_there_were_no_further_calls() {
    // Arrange
    Mock<IBookRepository> mock = new();
    IBookRepository repository = mock.Object;

    // Act
    repository.GetById(1);

    // Assert
    mock.Verify(r => r.GetById(1), Times.Once);
    mock.VerifyNoOtherCalls();
  }

  [Fact]
  public void Returns_with_a_lambda_can_return_the_argument_itself() {
    // Arrange
    Mock<IBookRepository> mock = new();
    mock.Setup(r => r.Add(It.IsAny<Book>())).Returns<Book>(book => book);
    IBookRepository repository = mock.Object;

    // Act
    Book added = repository.Add(new Book { Id = 3, Title = "The Genius of Birds" });

    // Assert
    Assert.Equal("The Genius of Birds", added.Title);
    Assert.Equal(3, added.Id);
  }

  [Fact]
  public void A_mock_can_be_told_to_throw_an_exception() {
    // Arrange
    Mock<IBookRepository> mock = new();
    mock.Setup(r => r.GetAll())
      .Throws(() => new InvalidOperationException("Repository er ikke tilgængeligt"));
    IBookRepository repository = mock.Object;

    // Act
    InvalidOperationException exception = Assert.Throws<InvalidOperationException>(
      () => repository.GetAll());

    // Assert
    Assert.Equal("Repository er ikke tilgængeligt", exception.Message);
  }
}