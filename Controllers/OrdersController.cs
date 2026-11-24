
using AddisEats.Api.MiniProject.Capston.DTOs;
using AddisEats.Api.MiniProject.Capston.Models;
using AddisEats.Api.MiniProject.Capston.Services.OrderService;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/orders")]
public class OrdersController : ControllerBase
{
    private  readonly IOrderService _OrderService;

    public OrdersController(IOrderService orderService)
    {
        _OrderService = orderService;
    }

    [HttpGet("{id:int}", Name = "GetOrderById")]
    public IActionResult GetOrderById(int id)
    {
        var order = _OrderService.GetByID(id);

        if (order is null)
            return NotFound();

        return Ok(order);
    }

    [HttpPost]
public IActionResult CreateOrder(CreateOrderRequest req)
{
    try
    {
        var result = _OrderService.Add(req);

        return CreatedAtAction(
            nameof(GetOrderById),
            new { id = result.Id },
            result
        );
    }
    catch (ArgumentException ex)
    {
        return BadRequest(new { message = ex.Message });
    }
    catch (KeyNotFoundException ex)
    {
        return NotFound(new { message = ex.Message });
    }
}

}
