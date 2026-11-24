using System.ComponentModel.DataAnnotations;
namespace AddisEats.Api.MiniProject.Capston.Validation;

public class EtbAmountAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(
        object? value, 
        ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }
        
        if (value is not decimal amount)
        {
            return new ValidationResult("Please Enter Valid Decimal ETB Birr");
        }

        if (amount <= 0)
        {
            return new ValidationResult("Amount must be greater than zero.");
        }

        if (decimal.Round(amount,2) == amount)
        {
            return  ValidationResult.Success;
        }
    return new ValidationResult("Plese Enter valid 2 decimal Etb Number");
    }
}