
namespace AddisEats.Api.MiniProject.Capston.DTOs;

public class CreateOrderRequest
{
    public int MenuItemId {get; set;}
    public int RestaurantId { get; set; }

    public string Area { get; set; } = string.Empty;

    public int Qty{get; set;}
}