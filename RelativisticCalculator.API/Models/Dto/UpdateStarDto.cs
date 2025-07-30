using System.ComponentModel.DataAnnotations;

namespace RelativisticCalculator.API.Models.Dto;

public class UpdateStarDto
{
    [Required(ErrorMessage = "Star name is required")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Star name must be between 1 and 100 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Distance in light years is required")]
    [Range(0.0001, double.MaxValue, ErrorMessage = "Distance must be a positive number")]
    public double DistanceLy { get; set; }
}