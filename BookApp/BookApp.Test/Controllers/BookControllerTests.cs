using BookApp.Controllers;
using BookApp.Models.Books;
using BookApp.Repositories;
using BookApp.Test.TestInfrastructure;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BookApp.Test.Controllers;

public class BookControllerTests {
  
  [Fact] // GET /Book/Index
  public void Index_returns_a_view_with_all_books() {
    // Arrange
    Mock<IBookRepository> mock = MockBookRepository.Create(
      new Book { Title = "The Genius of Birds" },
      new Book { Title = "The Sibley Guide to Birds" }
    );
    BookController controller = TestController.WithContext(new BookController(mock.Object));

    // Act
    IActionResult result = controller.Index();

    // Assert
    ViewResult view = Assert.IsType<ViewResult>(result);
    List<Book> books = Assert.IsAssignableFrom<IEnumerable<Book>>(view.Model).ToList();

    Assert.Equal(2, books.Count);
    Assert.Equal("The Genius of Birds", books[0].Title);
    Assert.Equal("The Sibley Guide to Birds", books[1].Title);
  }

  [Theory] // GET /Book/Details/id, id = 0, 1, 2
  [InlineData(0, "The Genius of Birds")]
  [InlineData(1, "The Sibley Guide to Birds")]
  [InlineData(2, "What It's Like to Be a Bird")]
  public void Details_returns_a_view_with_the_book_that_has_the_id(int id, string expectedTitle) {
    // Arrange
    Mock<IBookRepository> mock = MockBookRepository.Create(
      new Book { Title = "The Genius of Birds" },
      new Book { Title = "The Sibley Guide to Birds" },
      new Book { Title = "What It's Like to Be a Bird" }
    );
    BookController controller = TestController.WithContext(new BookController(mock.Object));

    // Act
    IActionResult result = controller.Details(id);

    // Assert
    ViewResult view = Assert.IsType<ViewResult>(result);
    Book book = Assert.IsType<Book>(view.Model);

    Assert.Equal(id, book.Id);
    Assert.Equal(expectedTitle, book.Title);
  }

  [Fact] // GET /Book/Details/5
  public void Details_returns_not_found_when_no_book_has_the_id() {
    // Arrange
    Mock<IBookRepository> mock = MockBookRepository.Create(new Book { Title = "The Genius of Birds" });
    BookController controller = TestController.WithContext(new BookController(mock.Object));

    // Act
    IActionResult result = controller.Details(5);

    // Assert
    Assert.IsType<NotFoundResult>(result);
  }

  [Fact] // GET /Book/Details/0
  public void Details_throws_an_exception_from_the_repository() {
    // Arrange
    Mock<IBookRepository> mock = MockBookRepository.Create(new Book { Title = "Dune" });
    mock.Setup(r => r.GetById(It.IsAny<int>()))
      .Throws(() => new InvalidOperationException("Repository er ikke tilgængeligt"));
    BookController controller = TestController.WithContext(new BookController(mock.Object));

    // Act
    InvalidOperationException exception = Assert.Throws<InvalidOperationException>(
      () => controller.Details(0));

    // Assert
    Assert.Equal("Repository er ikke tilgængeligt", exception.Message);
  }

  [Fact] // GET /Book/Create
  public void Create_returns_a_view_with_an_empty_form() {
    // Arrange
    Mock<IBookRepository> mock = MockBookRepository.Create();
    BookController controller = TestController.WithContext(new BookController(mock.Object));

    // Act
    IActionResult result = controller.Create();

    // Assert
    ViewResult view = Assert.IsType<ViewResult>(result);
    BookFormViewModel model = Assert.IsType<BookFormViewModel>(view.Model);

    Assert.Equal(string.Empty, model.Title);
    Assert.Equal(string.Empty, model.Author);
  }

  [Fact] // POST /Book/Create
  public void Create_adds_the_book_and_redirects_to_index_when_input_is_valid() {
    // Arrange
    Mock<IBookRepository> mock = MockBookRepository.Create();
    BookController controller = TestController.WithContext(new BookController(mock.Object));

    BookFormViewModel viewModel = new() {
      Title = "Dune",
      Author = "Frank Herbert",
      Pages = 412,
      Price = 149.95m,
      Published = 1965
    };

    // Act
    IActionResult result = controller.Create(viewModel);

    // Assert
    RedirectToActionResult redirect = Assert.IsType<RedirectToActionResult>(result);
    Assert.Equal(nameof(BookController.Index), redirect.ActionName);

    mock.Verify(
      r => r.Add(It.Is<Book>(b =>
        b.Title == "Dune" &&
        b.Author == "Frank Herbert" &&
        b.Pages == 412 &&
        b.Price == 149.95m &&
        b.Published == 1965)),
      Times.Once
    );

    Assert.Equal("Bogen \"Dune\" er oprettet.", controller.TempData["Success"]);
  }

  [Fact] // POST /Books/Create
  public void Create_returns_the_model_again_and_adds_nothing_when_the_input_is_invalid() {
    // Arrange
    Mock<IBookRepository> mock = MockBookRepository.Create();
    BookController controller = TestController.WithContext(new BookController(mock.Object));

    controller.ModelState.AddModelError(
      nameof(BookFormViewModel.Title), "Titel er påkrævet.");

    BookFormViewModel viewModel = new() { Title = string.Empty };

    // Act
    IActionResult result = controller.Create(viewModel);

    // Assert
    ViewResult view = Assert.IsType<ViewResult>(result);

    Assert.Same(viewModel, view.Model);
    mock.Verify(r => r.Add(It.IsAny<Book>()), Times.Never);
  }

  [Fact] // GET /Book/Edit/0
  public void Edit_returns_a_view_with_the_book_that_has_the_id() {
    // Arrange
    Book dune = new() {
      Title = "Dune",
      Author = "Frank Herbert",
      Pages = 412,
      Price = 149.95m,
      Published = 1965
    };

    Mock<IBookRepository> mock = MockBookRepository.Create(dune);
    BookController controller = TestController.WithContext(new BookController(mock.Object));

    // Act
    IActionResult result = controller.Edit(0);

    // Assert
    ViewResult view = Assert.IsType<ViewResult>(result);
    BookFormViewModel model = Assert.IsType<BookFormViewModel>(view.Model);

    Assert.Equal("Dune", model.Title);
    Assert.Equal("Frank Herbert", model.Author);
    Assert.Equal(412, model.Pages);
    Assert.Equal(149.95m, model.Price);
    Assert.Equal(1965, model.Published);
  }

  [Fact] // GET /Book/Edit/5
  public void Edit_returns_not_found_when_no_book_has_the_id() {
    // Arrange
    Mock<IBookRepository> mock = MockBookRepository.Create(new Book { Title = "Dune" });
    BookController controller = TestController.WithContext(new BookController(mock.Object));

    // Act
    IActionResult result = controller.Edit(5);

    // Assert
    Assert.IsType<NotFoundResult>(result);
    mock.Verify(r => r.Update(It.IsAny<Book>()), Times.Never);
  }

  [Fact] // POST /Book/Edit
  public void Edit_updates_the_book_and_redirects_to_index_when_the_input_is_valid() {
    // Arrange
    Book dune = new() {
      Title = "Dune",
      Author = "Frank Herbert",
      Pages = 412,
      Price = 149.95m,
      Published = 1965
    };

    Mock<IBookRepository> mock = MockBookRepository.Create(dune);
    BookController controller = TestController.WithContext(new BookController(mock.Object));

    BookFormViewModel viewModel = new() {
      Title = "Children of Dune",
      Author = "Frank Herbert",
      Pages = 430,
      Price = 159.95m,
      Published = 1976
    };

    // Act
    IActionResult result = controller.Edit(0, viewModel);

    // Assert
    RedirectToActionResult redirect = Assert.IsType<RedirectToActionResult>(result);
    Assert.Equal(nameof(BookController.Index), redirect.ActionName);

    mock.Verify(r => r.Update(It.Is<Book>(b => b.Id == 0 && b.Title == "Children of Dune")), Times.Once);

    Assert.Equal("Children of Dune", dune.Title);
    Assert.Equal(430, dune.Pages);
    Assert.Equal("Bogen \"Children of Dune\" er opdateret.", controller.TempData["Success"]);
  }

  [Fact] // POST /Book/Edit
  public void Edit_does_not_update_anything_when_the_input_is_invalid() {
    // Arrange
    Mock<IBookRepository> mock = MockBookRepository.Create(new Book { Title = "Dune" });
    BookController controller = TestController.WithContext(new BookController(mock.Object));

    controller.ModelState.AddModelError(
      nameof(BookFormViewModel.Title), "Titel er påkrævet.");

    BookFormViewModel viewModel = new() { Title = string.Empty };

    // Act
    IActionResult result = controller.Edit(0, viewModel);

    // Assert
    ViewResult view = Assert.IsType<ViewResult>(result);

    Assert.Same(viewModel, view.Model);
    mock.Verify(r => r.Update(It.IsAny<Book>()), Times.Never);
  }

  [Fact] // GET /Books/Delete/0
  public void Delete_returns_a_view_with_the_book_that_has_the_id() {
    // Arrange
    Mock<IBookRepository> mock = MockBookRepository.Create(new Book { Title = "Dune" });
    BookController controller = TestController.WithContext(new BookController(mock.Object));

    // Act
    IActionResult result = controller.Delete(0);

    // Assert
    ViewResult view = Assert.IsType<ViewResult>(result);
    Book book = Assert.IsType<Book>(view.Model);

    Assert.Equal("Dune", book.Title);
  }

  [Fact] // GET /Books/Delete/5
  public void Delete_returns_not_found_when_no_book_has_the_id() {
    // Arrange
    Mock<IBookRepository> mock = MockBookRepository.Create(new Book { Title = "Dune" });
    BookController controller = TestController.WithContext(new BookController(mock.Object));

    // Act
    IActionResult result = controller.Delete(5);

    // Assert
    Assert.IsType<NotFoundResult>(result);
  }

  [Fact] // POST /Books/Delete/0
  public void DeleteConfirmed_deletes_the_book_and_redirects_to_index() {
    // Arrange
    Mock<IBookRepository> mock = MockBookRepository.Create(new Book { Title = "Dune" });
    BookController controller = TestController.WithContext(new BookController(mock.Object));

    // Act
    IActionResult result = controller.DeleteConfirmed(0);

    // Assert
    RedirectToActionResult redirect = Assert.IsType<RedirectToActionResult>(result);
    Assert.Equal(nameof(BookController.Index), redirect.ActionName);

    mock.Verify(r => r.Delete(0), Times.Once);
    Assert.Equal("Bogen er slettet.", controller.TempData["Success"]);
  }

  [Fact] // POST /Books/Delete/5
  public void DeleteConfirmed_sets_no_success_message_when_the_book_could_not_be_deleted() {
    // Arrange
    Mock<IBookRepository> mock = MockBookRepository.Create(new Book { Title = "Dune" });
    BookController controller = TestController.WithContext(new BookController(mock.Object));

    // Act
    IActionResult result = controller.DeleteConfirmed(5);

    // Assert
    RedirectToActionResult redirect = Assert.IsType<RedirectToActionResult>(result);
    Assert.Equal(nameof(BookController.Index), redirect.ActionName);

    mock.Verify(r => r.Delete(5), Times.Once);
    Assert.False(controller.TempData.ContainsKey("Success"));
  }
}