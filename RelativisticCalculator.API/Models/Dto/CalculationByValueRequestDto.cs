using System.ComponentModel.DataAnnotations;

namespace RelativisticCalculator.API.Models.Dto;

public class CalculationByValueRequestDto : BaseCalculationRequestDto
{
    [Required(ErrorMessage = "Distance in light years is required")]
    [Range(0.0001, double.MaxValue, ErrorMessage = "Distance must be a positive number")]
    public double DistanceLightYears { get; set; }
}