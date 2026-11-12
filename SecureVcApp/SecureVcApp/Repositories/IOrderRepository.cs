using SecureVcApp.Models;

namespace SecureVcApp.Repositories;

public interface IOrderRepository {
  IEnumerable<Order> GetAll();
  IEnumerable<Order> GetByUserId(string userId);
  Order? GetById(int id);
  Order? GetByIdAndUserId(int id, string userId);
  void Add(Order order);
  void Update(Order order);
  void Delete(Order order);
  void Save();
}
