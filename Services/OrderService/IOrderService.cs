using AddisEats.Api.MiniProject.Capston.DTOs;
using AddisEats.Api.MiniProject.Capston.Models;

namespace AddisEats.Api.MiniProject.Capston.Services.OrderService;

public interface IOrderService
{
    Order? GetByID(int id);
    Order Add(CreateOrderRequest item);
}