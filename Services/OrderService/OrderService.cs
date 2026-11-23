using AddisEats.Api.MiniProject.Capston.DTOs;
using  AddisEats.Api.MiniProject.Capston.Models;
using AddisEats.Models;
namespace AddisEats.Api.MiniProject.Capston.Services.OrderService;
public class OrderService : IOrderService
{
    private readonly int _DefaultPageLimit = 5;

    private static List<MenuItem> _OrderItems = new List<MenuItem>()
    {
        new MenuItem(){Id=1,Name="FIRFIR", Category="Main", Price=150},
        new MenuItem(){Id=2,Name="Chechebsa", Category="Fast", Price=180},
        new MenuItem(){Id=3,Name="Genfo", Category="Slow", Price=120},
        new MenuItem(){Id=4,Name="Dulet", Category="Main", Price=250},
        new MenuItem(){Id=5,Name="Gomen", Category="Fast", Price=160},
        new MenuItem(){Id=6,Name="Misir", Category="Slow", Price=140},
        new MenuItem(){Id=7,Name="Beyaynet", Category="Main", Price=220},
        new MenuItem(){Id=8,Name="Enkulal", Category="Fast", Price=130},
        new MenuItem(){Id=9,Name="Ful", Category="Slow", Price=170},
        new MenuItem(){Id=10,Name="Kikil", Category="Main", Price=280},
        new MenuItem(){Id=11,Name="Azifa", Category="Fast", Price=110},
        new MenuItem(){Id=12,Name="Shinbra", Category="Slow", Price=190},
        new MenuItem(){Id=13,Name="Quanta Firfir", Category="Main", Price=320},
        new MenuItem(){Id=14,Name="Bula", Category="Fast", Price=200},
        new MenuItem(){Id=15,Name="Kurt", Category="Slow", Price=350},


    };

    
    private int NextId = _OrderItems.Count + 1;
    public MenuItem Add(OrderCreateDTO item)
    {
        
        if (item.Price <= 0)
        {
            throw new ArgumentException("Price must be greater than zero.");
        }

        var result = new MenuItem(){
            Id = NextId++,
            Name = item.Name,
            Category = item.Category,
            Price = item.Price
        };


        _OrderItems.Add(result);
        return result;
        }

   
    public MenuItem GetByID(int id)
    {
       MenuItem? result = _OrderItems.Where(item=> item.Id == id).FirstOrDefault();
        return result;
    }
        
}

