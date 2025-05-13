namespace GWWS60Beadando.Models;

using System.ComponentModel.DataAnnotations;

public class Ship
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string Cargo { get; set; }

    [Range(0, 1000000)]
    public double Weight { get; set; }

    public DateTime ArrivalDate { get; set; }
}
