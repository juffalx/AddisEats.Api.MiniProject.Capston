namespace AddisEats.Api.MiniProject.Capston.Mappers;
using AddisEats.Api.MiniProject.Capston.DTOs;
using AddisEats.Api.MiniProject.Capston.Models;

public static class RestaurantMapper
{
    public static RestaurantDTO ToDTO(this Restaurant r)
    {
        return new RestaurantDTO
        (
            r.OwnerPhone,
            r.CommissionRate
        );
    }
}