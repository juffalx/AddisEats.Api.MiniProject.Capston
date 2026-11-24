using AddisEats.Api.MiniProject.Capston.DTOs;
using AddisEats.Api.MiniProject.Capston.Models;
using AddisEats.Api.MiniProject.Capston.Results;

namespace AddisEats.Api.MiniProject.Capston.Services.OrderService;

public interface IOrderService
{
    Order? GetByID(int id);
    Result<Order> Add(CreateOrderRequest item);
}