
using System.ComponentModel.DataAnnotations;
using AddisEats.Api.MiniProject.Capston.Validation;

namespace AddisEats.Api.MiniProject.Capston.DTOs;

public class CreateOrderRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "MenuItemId Should Possitive Number")]
    public int MenuItemId {get; set;}
    [EthiopianMobile]
    public string CustomerPhone { get; set; } = string.Empty;

    [Required(ErrorMessage = "RestaurantId is Required!")]
    [Range(1, int.MaxValue, ErrorMessage = "RestaurantId Should Possitive Number")]
    public int RestaurantId { get; set; }
    [Required(ErrorMessage = "Area is Required.")]
    [StringLength(50,ErrorMessage = "Area maximum contain 50 characters.")]
    [MinLength(2, ErrorMessage = "Area must contain at least 2 characters.")]
    public string Area { get; set; } = string.Empty;
    [Range(1, 50, ErrorMessage = "Quantity should between 1 and 50")]
    public int Qty{get; set;}

}