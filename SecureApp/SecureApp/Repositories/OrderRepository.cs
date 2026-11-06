using SecureApp.Data;
using SecureApp.Models;
using Microsoft.EntityFrameworkCore;

namespace SecureApp.Repositories;

public class OrderRepository : IOrderRepository {

  private readonly AppDbContext _db;

  public OrderRepository(AppDbContext db) {
    _db = db;
  }

  public IEnumerable<Order> GetAll() =>
    _db.Orders.AsNoTracking().OrderBy(o => o.OrderDate).ToList();

  public IEnumerable<Order> GetByUserId(string userId) =>
    _db.Orders.AsNoTracking()
      .Where(o => o.UserId == userId)
      .OrderByDescending(o => o.OrderDate)
      .ToList();

  public Order? GetById(int id) =>
    _db.Orders.AsNoTracking().FirstOrDefault(o => o.Id == id);

  public Order? GetByIdAndUserId(int id, string userId) =>
    _db.Orders.AsNoTracking().FirstOrDefault(o => o.Id == id && o.UserId == userId);

  public void Add(Order order) =>
    _db.Orders.Add(order);

  public void Update(Order order) =>
    _db.Orders.Update(order);

  public void Delete(Order order) =>
    _db.Orders.Remove(order);

  public void Save() =>
    _db.SaveChanges();
}
