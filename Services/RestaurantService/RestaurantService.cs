using AddisEats.Api.MiniProject.Capston.Datas;
using AddisEats.Api.MiniProject.Capston.Models;

namespace AddisEats.Api.MiniProject.Capston.Services.RestaurantService;

public class RestaurantService : IRestaurantService
{
    private readonly List<Restaurant> _allDish = Seed.Restaurants; 
       public List<Restaurant> Search(RestaurantQuery r)
    {
        var result = _allDish
        .Where(item => r.Area == null || item.Area.Trim().ToLowerInvariant() == r.Area.Trim().ToLowerInvariant())
        .Skip((r.Page - 1) * r.Limit)
        .Take(r.Limit)
        .ToList();

        return result;
    }
    public Restaurant GetById(int id)
    {
        Restaurant? result = _allDish.Where(r => r.Id == id).FirstOrDefault();

        return result;
    }
}