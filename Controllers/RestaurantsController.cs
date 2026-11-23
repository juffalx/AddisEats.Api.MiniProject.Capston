using AddisEats.Api.MiniProject.Capston.DTOs;
using AddisEats.Api.MiniProject.Capston.Mappers;
using AddisEats.Api.MiniProject.Capston.Models;
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

    // Get one restaurant
    [HttpGet("{id}")]

    public ActionResult<RestaurantDTO> GetOne(int id)
    {
        return Ok(_restaurantService.GetById(id).ToDTO());
    }

//  area, page and limit. At limit=5
    [HttpGet]
    public ActionResult BySearch(
    [FromQuery] RestaurantQuery r)
    {
        var result = _restaurantService.Search(r);

        if(result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("{id:int}/menu")]

    public ActionResult GetById(int id, [FromQuery] string menu)
    {
        var foundRestaunrat = _restaurantService.GetById(id);

        if (foundRestaunrat == null)
        {
            return NotFound();
        }

        

        return Ok($"{foundRestaunrat.Name} and Menu is :-> {menu}");
    }

    // for test abugitiy
    // [HttpGet("{ids:int}")]
    // public IActionResult Test1(int ids)
    // {
    //     return Ok($"---> {ids} <---");
    // }
    // [HttpGet("{slug:alpha}")]
    // public IActionResult Test1(string slug)
    // {
    //     return Ok($"---> {slug} <---");
    // }
}