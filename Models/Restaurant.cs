namespace AddisEats.Api.MiniProject.Capston.Models;

public class Restaurant
{
    
public int Id { get; init; }
public string Name { get; set; } = "";
public string Area { get; set; } = "";
public decimal DeliveryFeeEtb { get; set; }
public bool IsActive { get; set; } = true;
public string OwnerPhone { get; set; } = ""; // dtos use
public decimal CommissionRate { get; set; } // dtos use

}