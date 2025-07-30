using Microsoft.Extensions.Options;
using RelativisticCalculator.API.Configuration;
using RelativisticCalculator.API.Models.Dto;
using RelativisticCalculator.API.Services.Interfaces;

namespace RelativisticCalculator.API.Services.Implementation;

public class RelCalcService : IRelCalcService
{
    private readonly PhysicalConstants _constants;

    /// <summary>
    /// Initializes a new instance of the <see cref="RelCalcService"/> class with physical constants.
    /// </summary>
    /// <param name="options">The physical constants options.</param>
    public RelCalcService(IOptions<PhysicalConstants> options)
    {
        _constants = options.Value;
    }

    /// <summary>
    /// Calculates relativistic travel parameters based on acceleration, distance, and deceleration settings.
    /// </summary>
    /// <param name="accelerationG">Acceleration in units of g (Earth gravity).</param>
    /// <param name="distanceLightYears">Distance to travel in light years.</param>
    /// <param name="decelerateAtTarget">Whether to decelerate halfway to the target.</param>
    /// <returns>A <see cref="CalculationResultDto"/> containing ship time, Earth time, and max velocity fraction.</returns>
    /// <exception cref="ArgumentException">Thrown when input parameters are invalid (e.g., negative acceleration or distance).</exception>
    /// <exception cref="InvalidOperationException">Thrown when physical parameters result in impossible or overflow calculations.</exception>
    public CalculationResultDto Calculate(double accelerationG, double distanceLightYears, bool decelerateAtTarget)
    {
        if (accelerationG <= 0)
            throw new ArgumentException("Acceleration must be positive", nameof(accelerationG));

        if (accelerationG > 1000)
            throw new ArgumentException("Acceleration must be less than 1000g", nameof(accelerationG));

        if (distanceLightYears <= 0)
            throw new ArgumentException("Distance must be positive", nameof(distanceLightYears));

        // Physical constants (more precise values)
        double g = _constants.G; // 9.80665 m/s²
        double c = _constants.C; // 299,792,458 m/s
        double lightYearInMeters = _constants.LightYearInMeters; // 9.4607304725808e15 m
        double secondsInYear = 365.25 * 24.0 * 3600.0; // Number of seconds in a year (31557600)

        // Calculate acceleration in m/s² and distance in meters
        double a = accelerationG * g;
        double d = distanceLightYears * lightYearInMeters;

        // Distance to accelerate before deceleration (if applicable)
        double halfDistance = decelerateAtTarget ? d * 0.5 : d;

        // Check physical feasibility
        double arg = 1.0 + (a * halfDistance) / (c * c);
        if (arg <= 1.0)
            throw new InvalidOperationException("Invalid physical parameters");

        // Proper time calculation: τ = (c/a) * acosh(1 + ad/(c²))
        double properTimeOneWay = (c / a) * Math.Log(arg + Math.Sqrt(arg * arg - 1.0));

        // Overflow checks
        if (double.IsInfinity(properTimeOneWay) || double.IsNaN(properTimeOneWay))
            throw new InvalidOperationException("Calculation resulted in overflow - distance or acceleration too large");

        double totalProperTime = decelerateAtTarget ? 2 * properTimeOneWay : properTimeOneWay;

        // Coordinate time calculation: t_coord = (c/a) * sinh(a*τ/c)
        double sinhValue = Math.Sinh(a * properTimeOneWay / c);
        if (double.IsInfinity(sinhValue))
            throw new InvalidOperationException("Calculation resulted in overflow - parameters too extreme");

        double coordinateTimeOneWay = (c / a) * sinhValue;
        double totalCoordinateTime = decelerateAtTarget ? 2 * coordinateTimeOneWay : coordinateTimeOneWay;

        // Maximum velocity
        double tanhValue = Math.Tanh(a * properTimeOneWay / c);
        double maxVelocity = c * tanhValue;
        double maxVelocityFraction = maxVelocity / c;

        // Additional sanity checks
        if (totalCoordinateTime < totalProperTime)
            throw new InvalidOperationException("Coordinate time cannot be less than proper time");

        if (maxVelocityFraction >= 1)
            throw new InvalidOperationException("Maximum velocity fraction exceeded lightspeed");

        // Convert times to years, rounded to three decimals
        double yearsPassedShip = Math.Round(totalProperTime / secondsInYear, 3);
        double yearsPassedEarth = Math.Round(totalCoordinateTime / secondsInYear, 3);

        return new CalculationResultDto
        {
            YearsPassedShip = yearsPassedShip,
            YearsPassedEarth = yearsPassedEarth,
            MaxVelocityFractionOfC = maxVelocityFraction
        };
    }
}
