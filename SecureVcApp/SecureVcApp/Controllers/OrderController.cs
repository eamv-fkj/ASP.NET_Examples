using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SecureVcApp.Models;
using SecureVcApp.Repositories;
using SecureVcApp.ViewModels.Order;

namespace SecureVcApp.Controllers;

[Authorize]
public class OrderController : Controller {
  private readonly IOrderRepository _orderRepository;
  private readonly UserManager<AppUser> _userManager;

  public OrderController(
    IOrderRepository orderRepository,
    UserManager<AppUser> userManager)
  {
    _orderRepository = orderRepository;
    _userManager = userManager;
  }

  // GET: /Order/Index
  public IActionResult Index() {
    IEnumerable<Order> orders = _orderRepository.GetByUserId(CurrentUserId);

    return View(orders);
  }

  // GET: /Order/Create
  public IActionResult Create() {
    return View(new OrderCreateViewModel());
  }

  // POST: /Order/Create
  [HttpPost]
  public IActionResult Create(OrderCreateViewModel model) {
    if (!ModelState.IsValid)
      return View(model);

    Order order = new() {
      ProductName = model.ProductName,
      Quantity = model.Quantity,
      OrderDate = DateTime.Now,
      UserId = CurrentUserId
    };

    _orderRepository.Add(order);
    _orderRepository.Save();

    TempData["Success"] = "Ordre oprettet.";

    return RedirectToAction(nameof(Index));
  }

  // GET: /Order/Details/5
  public IActionResult Details(int id) {
    Order? order = _orderRepository.GetByIdAndUserId(id, CurrentUserId);

    if (order is null)
      return NotFound();

    return View(order);
  }

  // GET: /Order/Edit/5
  public IActionResult Edit(int id) {
    Order? order = _orderRepository.GetByIdAndUserId(id, CurrentUserId);

    if (order is null)
      return NotFound();

    OrderEditViewModel model = new() {
      Id = order.Id,
      ProductName = order.ProductName,
      Quantity = order.Quantity
    };

    return View(model);
  }

  // POST: /Order/Edit/5
  [HttpPost]
  public IActionResult Edit(OrderEditViewModel model) {
    if (!ModelState.IsValid)
      return View(model);

    Order? order = _orderRepository.GetByIdAndUserId(model.Id, CurrentUserId);

    if (order is null)
      return NotFound();

    order.ProductName = model.ProductName;
    order.Quantity = model.Quantity;

    _orderRepository.Update(order);
    _orderRepository.Save();

    TempData["Success"] = "Ordre opdateret.";

    return RedirectToAction(nameof(Index));
  }

  // GET: /Order/Delete/5
  public IActionResult Delete(int id) {
    Order? order = _orderRepository.GetByIdAndUserId(id, CurrentUserId);

    if (order is null)
      return NotFound();

    return View(order);
  }

  // POST: /Order/Delete/5
  [HttpPost, ActionName("Delete")]
  public IActionResult DeleteConfirmed([FromForm] int id) {
    Order? order = _orderRepository.GetByIdAndUserId(id, CurrentUserId);

    if (order is null)
      return NotFound();

    _orderRepository.Delete(order);
    _orderRepository.Save();

    TempData["Success"] = "Ordre slettet.";

    return RedirectToAction(nameof(Index));
  }

  private string CurrentUserId => _userManager.GetUserId(User)!;
}
