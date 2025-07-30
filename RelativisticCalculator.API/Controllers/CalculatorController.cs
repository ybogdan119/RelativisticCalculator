using Microsoft.AspNetCore.Mvc;
using RelativisticCalculator.API.Filters;
using RelativisticCalculator.API.Models.Dto;
using RelativisticCalculator.API.Services.Interfaces;

namespace RelativisticCalculator.API.Controllers;

/// <summary>
/// Provides endpoints for performing relativistic flight calculations
/// based on either star name or custom distance values.
/// </summary>
[ApiController]
[Route("calculate")]
[ValidateModel]
public class CalculatorController : ControllerBase
{
    private readonly IRelService _relService;

    public CalculatorController(IRelService relService)
    {
        _relService = relService;
    }

    /// <summary>
    /// Calculates relativistic travel time based on the distance to a known star.
    /// </summary>
    /// <param name="request">Request containing the name of the star, acceleration in G, and deceleration flag.</param>
    /// <returns>The result of the relativistic calculation including time experienced by the ship and Earth.</returns>
    /// <response code="200">Calculation completed successfully.</response>
    /// <response code="400">Invalid input data (e.g. acceleration must be > 0).</response>
    /// <response code="404">The specified star was not found in the database.</response>
    [HttpPost("by-name")]
    [ProducesResponseType(typeof(CalculationResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CalculationResultDto>> CalculateByName([FromBody] CalculationByNameRequestDto request)
    {
        var result = await _relService.CalculateByName(request);
        return Ok(result);
    }

    /// <summary>
    /// Calculates relativistic travel time based on a custom distance value.
    /// </summary>
    /// <param name="request">Request containing distance in light-years, acceleration in G, and deceleration flag.</param>
    /// <returns>The result of the relativistic calculation including time experienced by the ship and Earth.</returns>
    /// <response code="200">Calculation completed successfully.</response>
    /// <response code="400">Invalid input data (e.g. negative distance or acceleration).</response>
    [HttpPost("by-value")]
    [ProducesResponseType(typeof(CalculationResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public ActionResult<CalculationResultDto> CalculateByValue([FromBody] CalculationByValueRequestDto request)
    {
        var result = _relService.CalculateByValue(request);
        return Ok(result);
    }
}
