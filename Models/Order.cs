namespace AddisEats.Api.MiniProject.Capston.Models;

public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = "";
    public decimal TotalAmount { get; set; }
}