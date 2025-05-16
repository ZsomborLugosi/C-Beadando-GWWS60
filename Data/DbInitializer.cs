using GWWS60Beadando.Models;
using Microsoft.EntityFrameworkCore;

namespace GWWS60Beadando.Data;

public static class DbInitializer
{
    public static void Initialize(DockyardContext context)
    {
        context.Database.Migrate();
        
        if (context.Ships.Any())
        {
            return; 
        }
        
        
        var ships = new Ship[]
        {
            new Ship
            {
                Name = "Titanic",
                Cargo = "Passengers",
                Weight = 45000,
                ArrivalDate = DateTime.Now.AddDays(-2)
            },
            new Ship
            {
                Name = "Nautilus",
                Cargo = "Divers",
                Weight = 72000,
                ArrivalDate = DateTime.Now.AddDays(-5)
            },
            new Ship
            {
                Name = "Black Pearl",
                Cargo = "Treasure",
                Weight = 38500,
                ArrivalDate = DateTime.Now.AddDays(-1)
            },
            new Ship
            {
                Name = "Big Guy",
                Cargo = "Container Cargo",
                Weight = 65000,
                ArrivalDate = DateTime.Now
            }
        };
        
        context.Ships.AddRange(ships);
        context.SaveChanges();
    }
}