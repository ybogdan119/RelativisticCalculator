using RelativisticCalculator.API.Models.Dto;

namespace RelativisticCalculator.API.Services.Interfaces;

/// <summary>
/// Provides core logic for relativistic travel calculations
/// based on classical physics and special relativity.
/// </summary>
public interface IRelCalcService
{
    /// <summary>
    /// Calculates relativistic travel parameters for a given acceleration,
    /// distance in light-years, and deceleration flag.
    /// </summary>
    /// <param name="accelerationG">The constant acceleration in units of Earth's gravity (g).</param>
    /// <param name="distanceLightYears">The total travel distance in light-years.</param>
    /// <param name="decelerateAtTarget">
    /// Whether the spacecraft should decelerate upon reaching the destination (i.e., symmetric acceleration/deceleration).
    /// </param>
    /// <returns>
    /// A <see cref="CalculationResultDto"/> containing:
    /// - Proper time on the ship (subjective duration),
    /// - Coordinate time on Earth (objective duration),
    /// - Max speed as a fraction of the speed of light.
    /// </returns>
    CalculationResultDto Calculate(
        double accelerationG,
        double distanceLightYears,
        bool decelerateAtTarget);
}