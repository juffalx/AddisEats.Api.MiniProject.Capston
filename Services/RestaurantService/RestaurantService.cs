using AddisEats.Api.MiniProject.Capston.Datas;
using AddisEats.Api.MiniProject.Capston.Models;

namespace AddisEats.Api.MiniProject.Capston.Services.RestaurantService;

public class RestaurantService : IRestaurantService
{
    private readonly List<Restaurant> _allDish = Seed.Restaurants; 
       public List<Restaurant> Search(string? category)
    {
        var result = _allDish
        .Where(r => category == null || r.Area.Trim().ToLowerInvariant() == category.Trim().ToLowerInvariant())
        .ToList();

        return result;
    }
    public Restaurant GetById(int id)
    {
        Restaurant? result = _allDish.Where(r => r.Id == id).FirstOrDefault();

        return result;
    }
}