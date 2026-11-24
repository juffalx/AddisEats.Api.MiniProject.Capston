using AddisEats.Api.MiniProject.Capston.DTOs;
using AddisEats.Api.MiniProject.Capston.Datas;
using System.Linq;
namespace AddisEats.Api.MiniProject.Capston.Services.OrderService;
public class OrderService : IOrderService
{
    private readonly int _DefaultPageLimit = 5;

   private static readonly List<Order> _Orders = new();

    private static int NextId = 1;
    public Order Add(CreateOrderRequest req)
{
    if (req.Qty <= 0)
        throw new ArgumentException("Quantity must be greater than zero.");

    var restaurant = Seed.Restaurants
        .FirstOrDefault(r => r.Id == req.RestaurantId && r.IsActive);

    if (restaurant is null)
        throw new KeyNotFoundException("Restaurant not found or inactive.");

    var menuItem = Seed._MenuItems
        .FirstOrDefault(m => m.Id == req.MenuItemId);

    if (menuItem is null)
        throw new KeyNotFoundException("Menu item not found.");

    decimal total = menuItem.Price * req.Qty;

    var result = new Order()
    {
        Id = NextId++,
        RestaurantId = restaurant.Id,
        MenuItemId = menuItem.Id,
        Area = req.Area,
        Qty = req.Qty,
        TotalAmount = total
    };

    _Orders.Add(result);

    return result;
}

   
    public Order? GetByID(int id)
{
    return _Orders.FirstOrDefault(o => o.Id == id);
}
        
}

