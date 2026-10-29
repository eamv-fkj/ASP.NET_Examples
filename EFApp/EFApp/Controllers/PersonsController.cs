using EFApp.Models;
using EFApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EFApp.Controllers;

public class PersonsController : Controller {

  private readonly IPersonRepository _repository;

  public PersonsController(IPersonRepository repository) {
    _repository = repository;
  }

  public IActionResult Index() {
    var persons = _repository.GetAll();

    return View(persons);
  }

  public IActionResult Details(int id) {
    var person = _repository.GetById(id);

    if (person is null)
      return NotFound();

    return View(person);
  }

  public IActionResult Create() {
    return View(new Person());
  }

  [HttpPost]
  public IActionResult Create(Person person) {
    if (!ModelState.IsValid)
      return View(person);

    _repository.Add(person);
    _repository.Save();

    return RedirectToAction(nameof(Index));
  }

  public IActionResult Edit(int id) {
    var person = _repository.GetById(id);

    if (person is null)
      return NotFound();

    return View(person);
  }

  [HttpPost]
  public IActionResult Edit(int id, Person person) {
    if (id != person.Id)
      return BadRequest();

    if (!ModelState.IsValid)
      return View(person);

    _repository.Update(person);
    _repository.Save();

    return RedirectToAction(nameof(Index));
  }

  public IActionResult Delete(int id) {
    var person = _repository.GetById(id);

    if (person is null)
      return NotFound();

    return View(person);
  }

  [HttpPost, ActionName("Delete")]
  public IActionResult DeleteConfirmed(int id) {
    var person = _repository.GetById(id);

    if (person is null)
      return NotFound();

    _repository.Delete(person);
    _repository.Save();

    return RedirectToAction(nameof(Index));
  }
}
