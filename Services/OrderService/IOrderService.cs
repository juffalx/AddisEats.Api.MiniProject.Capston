using AddisEats.Api.MiniProject.Capston.DTOs;
using AddisEats.Models;

namespace AddisEats.Api.MiniProject.Capston.Services.OrderService;

public interface IOrderService
{
    MenuItem GetByID(int id);
    MenuItem Add(OrderCreateDTO item);

}