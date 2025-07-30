using System.ComponentModel.DataAnnotations;

namespace RelativisticCalculator.API.Models.Dto;

public class CalculationByNameRequestDto : BaseCalculationRequestDto
{
    [Required(ErrorMessage = "Star name is required")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Star name must be between 1 and 100 characters")]
    public required string Name { get; set; }
}