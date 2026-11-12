using EFApp.Data;
using EFApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EFApp.Repositories;

public class PersonRepository : IPersonRepository {

  private readonly AppDbContext _db;

  public PersonRepository(AppDbContext db) {
    _db = db;
  }

  public IEnumerable<Person> GetAll() =>
    _db.Persons.AsNoTracking().OrderBy(p => p.Name).ToList();

  public Person? GetById(int id) =>
    _db.Persons.AsNoTracking().FirstOrDefault(p => p.Id == id);

  public void Add(Person person) =>
    _db.Persons.Add(person);

  public void Update(Person person) =>
    _db.Persons.Update(person);

  public void Delete(Person person) =>
    _db.Persons.Remove(person);

  public void Save() =>
    _db.SaveChanges();
}
