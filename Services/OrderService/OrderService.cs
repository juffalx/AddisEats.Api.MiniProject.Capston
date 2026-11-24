using AddisEats.Api.MiniProject.Capston.DTOs;
using AddisEats.Api.MiniProject.Capston.Datas;
using AddisEats.Api.MiniProject.Capston.Results;
using System.Linq;
namespace AddisEats.Api.MiniProject.Capston.Services.OrderService;
public class OrderService : IOrderService
{
    // private readonly int _DefaultPageLimit = 5;

   private static readonly List<Order> _Orders = new();

    private static int NextId = 1;
    public Result<Order> Add(CreateOrderRequest req)
{
    if (req.Qty <= 0)
        return Result<Order>.Failure( new Error{
            Code = "Invalid_Quantity", 
            Message  = "Quantity must be greater than zero."
        });

    var restaurant = Seed.Restaurants
        .FirstOrDefault(r => r.Id == req.RestaurantId && r.IsActive);

    if (restaurant is null)
        return Result<Order>.Failure( new Error{
            Code = "Restaurant_Not_Found",
            Message = "Restaurant not found or inactive."
        });

    var menuItem = Seed._MenuItems
        .FirstOrDefault(m => m.Id == req.MenuItemId);

    if (menuItem is null)
        return Result<Order>.Failure( new Error{
            Code = "Menu_Item_Not_Found",
            Message = "Menu item not found."
        });

    decimal total = menuItem.Price * req.Qty;

    var result = new Order()
    {
        Id = NextId++,
        CustomerPhone = req.CustomerPhone,
        RestaurantId = restaurant.Id,
        MenuItemId = menuItem.Id,
        Area = req.Area,
        Qty = req.Qty,
        TotalAmount = total // should be calculated in order service, not in controller or DTO.
    };

    _Orders.Add(result);

    return Result<Order>.Success(result);
}

   
    public Order? GetByID(int id)
{
    return _Orders.FirstOrDefault(o => o.Id == id);
}
        
}

