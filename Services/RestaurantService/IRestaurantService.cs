using AddisEats.Api.MiniProject.Capston.Models;
namespace AddisEats.Api.MiniProject.Capston.Services.RestaurantService;
//  Search and GetById, register it scoped,
// and inject it.
public interface IRestaurantService
{
    List<Restaurant> Search(string category);
    Restaurant GetById(int id);
}