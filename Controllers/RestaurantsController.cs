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
//  area, page and limit. At limit=5
    [HttpGet]
    public ActionResult BySearch(
    [FromQuery] string? area,
    [FromQuery] int page = 1, 
    [FromQuery] int limit = 5)
    {
        var result = _restaurantService.Search(area, page, limit);

        if(result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("{id}/menu")]

    public ActionResult GetById(int id, [FromQuery] string menu)
    {
        var foundRestaunrat = _restaurantService.GetById(id);

        if (foundRestaunrat == null)
        {
            return NotFound();
        }

        

        return Ok($"{foundRestaunrat.Name} and Menu is :-> {menu}");
    }
}