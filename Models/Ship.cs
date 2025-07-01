using System.ComponentModel.DataAnnotations;

namespace GWWS60Beadando.Models;

public class Ship
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Ship name is required")]
    [Display(Name = "Ship Name")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Cargo information is required")]
    [Display(Name = "Cargo Type")]
    [StringLength(200, ErrorMessage = "Cargo description cannot exceed 200 characters")]
    public string Cargo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Weight is required")]
    [Display(Name = "Weight (tons)")]
    [Range(0, 1000000, ErrorMessage = "Weight must be between 0.01 and 1,000,000 tons")]
    public double Weight { get; set; }

    [Required(ErrorMessage = "Arrival date is required")]
    [Display(Name = "Arrival Date")]
    [DataType(DataType.Date)]
    public DateTime ArrivalDate { get; set; } = DateTime.Now;
}