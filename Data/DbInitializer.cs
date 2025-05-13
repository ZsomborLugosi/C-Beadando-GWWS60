using GWWS60Beadando.Models;
using Microsoft.EntityFrameworkCore;

namespace GWWS60Beadando.Data;

public static class DbInitializer
{
    public static void Initialize(DockyardContext context)
    {
        // Ensure database is created and migrations are applied
        context.Database.Migrate();
        
        // Check if database already has data
        if (context.Ships.Any())
        {
            return; // Database already seeded
        }
        
        // Add sample ships
        var ships = new Ship[]
        {
            new Ship
            {
                Name = "Northern Star",
                Cargo = "Electronics",
                Weight = 45000,
                ArrivalDate = DateTime.Now.AddDays(-2)
            },
            new Ship
            {
                Name = "Ocean Voyager",
                Cargo = "Grain",
                Weight = 72000,
                ArrivalDate = DateTime.Now.AddDays(-5)
            },
            new Ship
            {
                Name = "Pacific Explorer",
                Cargo = "Vehicles",
                Weight = 38500,
                ArrivalDate = DateTime.Now.AddDays(-1)
            },
            new Ship
            {
                Name = "Atlantic Carrier",
                Cargo = "Container Goods",
                Weight = 65000,
                ArrivalDate = DateTime.Now
            }
        };
        
        context.Ships.AddRange(ships);
        context.SaveChanges();
    }
}