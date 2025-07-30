using RelativisticCalculator.API.Models.Dto;
using RelativisticCalculator.API.Services.Interfaces;

namespace RelativisticCalculator.API.Services.Implementation;

public class RelService : IRelService
{
    private readonly IStarService _starService;
    private readonly IRelCalcService _calc;

    /// <summary>
    /// Initializes a new instance of the <see cref="RelService"/> class.
    /// </summary>
    /// <param name="starService">The star service for retrieving star data.</param>
    /// <param name="calc">The relativistic calculation service.</param>
    public RelService(IStarService starService, IRelCalcService calc)
    {
        _starService = starService;
        _calc = calc;
    }

    /// <summary>
    /// Calculates the relativistic travel parameters based on a star's name.
    /// </summary>
    /// <param name="request">The request containing star name, acceleration, and deceleration options.</param>
    /// <returns>The calculated relativistic travel results.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when a star with the specified name is not found.</exception>
    public async Task<CalculationResultDto> CalculateByName(CalculationByNameRequestDto request)
    {
        var star = await _starService.ReadAsync(request.Name);
        if (star is null)
        {
            throw new KeyNotFoundException("Star not found");
        }
        
        return _calc.Calculate(request.AccelerationG,
            star.DistanceLy,
            request.DecelerateAtTarget);
    }

    /// <summary>
    /// Calculates the relativistic travel parameters based on a given distance.
    /// </summary>
    /// <param name="request">The request containing acceleration, distance, and deceleration options.</param>
    /// <returns>The calculated relativistic travel results.</returns>
    public CalculationResultDto CalculateByValue(CalculationByValueRequestDto request)
    {
        return _calc.Calculate(
            request.AccelerationG,
            request.DistanceLightYears,
            request.DecelerateAtTarget);
    }
}
