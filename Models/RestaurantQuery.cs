namespace AddisEats.Api.MiniProject.Capston.Models;

// Create a RestaurantQuery class with Area, MaxFeeEtb, Page and Limit, bind it with
// [FromQuery], and pass it to the service unchanged.

public class RestaurantQuery
{
    public string  Area{get; set;} = "";
    public decimal MaxFeeEtb{get; set;}
    public int Page{get; set;}
    public int Limit{get; set;}
}