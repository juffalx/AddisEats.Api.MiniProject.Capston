using AddisEats.Api.MiniProject.Capston.Models;
namespace AddisEats.Api.MiniProject.Capston.Services.RestaurantService;
//  Search and GetById, register it scoped,
// and inject it.
public interface IRestaurantService
{
    List<Restaurant> Search(RestaurantQuery r);
    Restaurant GetById(int id);
}