using RelativisticCalculator.API.Models.Dto;

namespace RelativisticCalculator.API.Services.Interfaces;

/// <summary>
/// Defines methods for performing relativistic travel calculations.
/// </summary>
public interface IRelService
{
    /// <summary>
    /// Calculates relativistic travel parameters based on a known star name.
    /// Looks up the distance to the star in the database and performs the calculation.
    /// </summary>
    /// <param name="request">The calculation request including acceleration and deceleration flags.</param>
    /// <returns>The result of the calculation including proper time and coordinate time.</returns>
    Task<CalculationResultDto> CalculateByName(CalculationByNameRequestDto request);

    /// <summary>
    /// Calculates relativistic travel parameters based on a manually entered distance.
    /// </summary>
    /// <param name="request">The calculation request including acceleration, distance, and deceleration flag.</param>
    /// <returns>The result of the calculation including proper time and coordinate time.</returns>
    CalculationResultDto CalculateByValue(CalculationByValueRequestDto request);
}