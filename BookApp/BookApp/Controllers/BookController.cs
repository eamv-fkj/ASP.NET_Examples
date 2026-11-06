using BookApp.Models.Books;
using BookApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BookApp.Controllers;

public class BookController : Controller {
  private readonly IBookRepository _bookRepository;

  public BookController(IBookRepository bookRepository) {
    _bookRepository = bookRepository;
  }

  // GET: /Book
  public IActionResult Index() {
    IEnumerable<Book> books = _bookRepository.GetAll();
    return View(books);
  }

  // GET: /Book/Details/5
  public IActionResult Details(int id) {
    Book? book = _bookRepository.GetById(id);

    if (book is null)
      return NotFound();

    return View(book);
  }

  // GET: /Book/Create
  public IActionResult Create() {
    return View(new BookFormViewModel());
  }

  // POST: /Book/Create
  [HttpPost]
  public IActionResult Create(BookFormViewModel model) {
    if (!ModelState.IsValid)
      return View(model);

    Book book = new();
    book.Copy(model);
    _bookRepository.Add(book);

    TempData["Success"] = $"Bogen \"{book.Title}\" er oprettet.";
    return RedirectToAction(nameof(Index));
  }

  // GET: /Book/Edit/5
  public IActionResult Edit(int id) {
    Book? book = _bookRepository.GetById(id);

    if (book is null)
      return NotFound();

    BookFormViewModel model = new();
    model.Copy(book);
    return View(model);
  }

  // POST: /Book/Edit/5
  [HttpPost]
  public IActionResult Edit(int id, BookFormViewModel model) {
    if (!ModelState.IsValid)
      return View(model);

    Book? existing = _bookRepository.GetById(id);

    if (existing is null)
      return NotFound();

    existing.Copy(model);
    _bookRepository.Update(existing);

    TempData["Success"] = $"Bogen \"{existing.Title}\" er opdateret.";
    return RedirectToAction(nameof(Index));
  }

  // GET: /Book/Delete/5
  public IActionResult Delete(int id) {
    Book? book = _bookRepository.GetById(id);

    if (book is null)
      return NotFound();

    return View(book);
  }

  // POST: /Book/Delete/5
  [HttpPost, ActionName("Delete")]
  public IActionResult DeleteConfirmed(int id) {
    if (_bookRepository.Delete(id))
      TempData["Success"] = "Bogen er slettet.";

    return RedirectToAction(nameof(Index));
  }
}