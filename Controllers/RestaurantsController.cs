using AddisEats.Api.MiniProject.Capston.Services.RestaurantService;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/[Controller]")]

public class RestaurantsController : ControllerBase
{
    private readonly IRestaurantService _restaurantService;

    public RestaurantsController(IRestaurantService restaurantService)
    {
        _restaurantService = restaurantService;
    }

    [HttpGet]
    public ActionResult BySearch()
    {
        var result = _restaurantService.Search(null);

        if(result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}