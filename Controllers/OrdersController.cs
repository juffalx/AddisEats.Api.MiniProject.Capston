
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
    public IActionResult CreateOrder(OrderCreateDTO order)
    {
        var result = _OrderService.Add(order);

        return CreatedAtAction(
            nameof(CreateOrder),
            new {id = result.Id},
            result
            
        );
    }
}
