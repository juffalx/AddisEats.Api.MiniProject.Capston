using System.ComponentModel.DataAnnotations;
namespace AddisEats.Api.MiniProject.Capston.DTOs;

public class OrderValidationDemo : IValidatableObject
{
    public int Qty { get; set; }
    public decimal UnitPrice { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Qty > 10 && UnitPrice < 100)
        {
            yield return new ValidationResult(
                "Unit price must be at least 100 ETB when quantity exceeds 10.",
                new[] { nameof(UnitPrice) }
            );
        }
    }
}


// var order = new OrderValidationDemo
// {
//     Qty = 15,
//     UnitPrice = 80
// };
