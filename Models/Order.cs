public class Order
{
    public int Id { get; set; }
    public string CustomerPhone{ get; set;} = string.Empty;

    public int RestaurantId { get; set; }
    public int MenuItemId { get; set; }
    public string Area { get; set; } = string.Empty;
    public int Qty { get; set; }
    public decimal TotalAmount { get; set; }
}