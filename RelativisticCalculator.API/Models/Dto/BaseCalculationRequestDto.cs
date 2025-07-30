using System.ComponentModel.DataAnnotations;

namespace RelativisticCalculator.API.Models.Dto;

public abstract class BaseCalculationRequestDto
{
    [Required(ErrorMessage = "Acceleration is required")]
    [Range(0.0001, 1000, ErrorMessage = "Acceleration must be >0.0001 and <1000")]
    public double AccelerationG { get; set; }
    
    [Required(ErrorMessage = "Deceleration flag years is required")]
    public bool DecelerateAtTarget { get; set; }
}