namespace AddisEats.Api.MiniProject.Capston.DTOs;

public record RestaurantDTO(
    int Id,
    string Name,
    string Area,
    decimal DeliveryFeeEtb,
    bool IsActive

);