namespace AddisEats.Api.MiniProject.Capston.DTOs;

public record OrderCreateDTO
(
    string Name,
    string? Category,
    decimal Price
    );