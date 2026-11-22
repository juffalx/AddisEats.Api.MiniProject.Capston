using AddisEats.Api.MiniProject.Capston.Models;

namespace AddisEats.Api.MiniProject.Capston.Datas;

public static class Seed
{
    
    public static List<Restaurant> Restaurants = new List<Restaurant>
{
    // Bole — 5 restaurants
    new()
    {
        Id = 1,
        Name = "Kategna Restaurant",
        Area = "Bole",
        DeliveryFeeEtb = 150m,
        IsActive = true,
        OwnerPhone = "0911001001",
        CommissionRate = 0.10m
    },
    new()
    {
        Id = 2,
        Name = "Golden Plate Restaurant",
        Area = "Bole",
        DeliveryFeeEtb = 180m,
        IsActive = true,
        OwnerPhone = "0911001002",
        CommissionRate = 0.12m
    },
    new()
    {
        Id = 3,
        Name = "Go Fresh Meals",
        Area = "Bole",
        DeliveryFeeEtb = 200m,
        IsActive = true,
        OwnerPhone = "0911001003",
        CommissionRate = 0.10m
    },
    new()
    {
        Id = 4,
        Name = "HOTTO",
        Area = "Bole",
        DeliveryFeeEtb = 220m,
        IsActive = true,
        OwnerPhone = "0911001004",
        CommissionRate = 0.15m
    },
    new()
    {
        Id = 5,
        Name = "2000 Habesha Cultural Restaurant",
        Area = "Bole",
        DeliveryFeeEtb = 170m,
        IsActive = true,
        OwnerPhone = "0911001005",
        CommissionRate = 0.12m
    },

    // Kazanchis — 3 restaurants
    new()
    {
        Id = 6,
        Name = "Henom Restaurant",
        Area = "Kazanchis",
        DeliveryFeeEtb = 250m,
        IsActive = true,
        OwnerPhone = "0911001006",
        CommissionRate = 0.10m
    },
    new()
    {
        Id = 7,
        Name = "Sichuan Chinese Restaurant",
        Area = "Kazanchis",
        DeliveryFeeEtb = 230m,
        IsActive = true,
        OwnerPhone = "0911001007",
        CommissionRate = 0.15m
    },
    new()
    {
        Id = 8,
        Name = "Belvedere Restaurant",
        Area = "Kazanchis",
        DeliveryFeeEtb = 210m,
        IsActive = true,
        OwnerPhone = "0911001008",
        CommissionRate = 0.12m
    },

    // Piassa — 2 restaurants
    new()
    {
        Id = 9,
        Name = "Keteme Restaurant",
        Area = "Piassa",
        DeliveryFeeEtb = 280m,
        IsActive = true,
        OwnerPhone = "0911001009",
        CommissionRate = 0.10m
    },
    new()
    {
        Id = 10,
        Name = "Tea Room Pastry and Restaurant",
        Area = "Piassa",
        DeliveryFeeEtb = 250m,
        IsActive = true,
        OwnerPhone = "0911001010",
        CommissionRate = 0.12m
    },

    // Sarbet — 2 restaurants
    new()
    {
        Id = 11,
        Name = "Temsalet Kitchen",
        Area = "Sarbet",
        DeliveryFeeEtb = 300m,
        IsActive = true,
        OwnerPhone = "0911001011",
        CommissionRate = 0.15m
    },
    new()
    {
        Id = 12,
        Name = "Delmela Sarbet",
        Area = "Sarbet",
        DeliveryFeeEtb = 270m,
        IsActive = true,
        OwnerPhone = "0911001012",
        CommissionRate = 0.10m
    }
};

}