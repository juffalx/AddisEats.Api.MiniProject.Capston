using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AddisEats.Api.MiniProject.Capston.Validation;

public class EthiopianMobileAttribute : ValidationAttribute
{
    private static readonly Regex Pattern =
        new(@"^(?:(?:09|07)\d{8}|\+251(?:9|7)\d{8})$");

    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (value is null)
            return ValidationResult.Success;

        if (value is not string phoneNumber)
        {
            return new ValidationResult("Phone number must be a string.");
        }

        if (Pattern.IsMatch(phoneNumber))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(
            "Enter a valid Ethiopian mobile number starting with 09, 07, +2519 or +2517.");
    }
}