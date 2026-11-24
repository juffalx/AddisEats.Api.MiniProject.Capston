using System.ComponentModel.DataAnnotations;
namespace AddisEats.Api.MiniProject.Capston.DTOs;

public record OrderLineRequest
(
    [Range(1, int.MaxValue)]
    int MenuItemId,

    [Range(1, 50)]
    int Quantity
);