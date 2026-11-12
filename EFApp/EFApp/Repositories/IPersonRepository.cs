using EFApp.Models;

namespace EFApp.Repositories;

public interface IPersonRepository {

  IEnumerable<Person> GetAll();
  Person? GetById(int id);
  void Add(Person person);
  void Update(Person person);
  void Delete(Person person);
  void Save();
}
