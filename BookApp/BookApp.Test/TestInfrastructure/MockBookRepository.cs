using BookApp.Models.Books;
using BookApp.Repositories;
using Moq;

namespace BookApp.Test.TestInfrastructure;

// En Mock<IBookRepository> fungerer som et rigtige BookRepository:
//   Id tildeles fra 0 og opefter, GetAll() returnerer kopi, og Add, Update og Delete
//   ændrer den underliggende liste. Dermed kan tests både læse og skrive.
public static class MockBookRepository {

  public static Mock<IBookRepository> Create(params Book[] books) {
    List<Book> stored = [];
    int nextId = 0;

    foreach (Book book in books) {
      book.Id = nextId++;
      stored.Add(book);
    }

    Mock<IBookRepository> mock = new();

    mock.Setup(r => r.GetAll())
      .Returns(() => stored.ToList());

    mock.Setup(r => r.GetById(It.IsAny<int>()))
      .Returns((int id) => stored.FirstOrDefault(b => b.Id == id));

    mock.Setup(r => r.Exists(It.IsAny<int>()))
      .Returns((int id) => stored.Any(b => b.Id == id));

    mock.Setup(r => r.Add(It.IsAny<Book>()))
      .Returns<Book>(book => {
        book.Id = nextId++;
        stored.Add(book);
        return book;
      });

    mock.Setup(r => r.Update(It.IsAny<Book>()))
      .Returns<Book>(book => {
        Book? existing = stored.FirstOrDefault(b => b.Id == book.Id);

        if (existing is null)
          return false;

        existing.Copy(book);
        return true;
      });

    mock.Setup(r => r.Delete(It.IsAny<int>()))
      .Returns<int>(id => {
        Book? book = stored.FirstOrDefault(b => b.Id == id);
        return book is not null && stored.Remove(book);
      });

    return mock;
  }
}