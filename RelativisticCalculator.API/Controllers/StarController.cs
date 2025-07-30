using Microsoft.AspNetCore.Mvc;
using RelativisticCalculator.API.Filters;
using RelativisticCalculator.API.Models.Dto;
using RelativisticCalculator.API.Services.Interfaces;

namespace RelativisticCalculator.API.Controllers;

/// <summary>
/// Manages CRUD operations for stars used in relativistic calculations.
/// </summary>
[ApiController]
[Route("[controller]")]
[ValidateModel]
public class StarController : ControllerBase
{
    private readonly IStarService _starService;

    public StarController(IStarService starService)
    {
        _starService = starService;
    }

    /// <summary>
    /// Creates a new star.
    /// </summary>
    /// <param name="starDto">Star data to be created.</param>
    /// <returns>The created star.</returns>
    /// <response code="200">Star created successfully.</response>
    /// <response code="400">Validation failed.</response>
    /// <response code="409">A star with the same name already exists.</response>
    [HttpPost]
    [ProducesResponseType(typeof(StarDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateStarDto starDto)
    {
        var created = await _starService.CreateAsync(starDto);
        return Ok(created);
    }

    /// <summary>
    /// Creates multiple stars in a single request.
    /// </summary>
    /// <param name="stars">A collection of stars to be created.</param>
    /// <param name="allowPartialInsert">If true, inserts only non-conflicting stars.</param>
    /// <returns>Insertion result with success and conflict details.</returns>
    /// <response code="200">All stars inserted successfully.</response>
    /// <response code="207">Some stars were inserted, some had conflicts.</response>
    /// <response code="400">Invalid input or duplicates not allowed.</response>
    [HttpPost("batch")]
    [ProducesResponseType(typeof(PartialInsertResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PartialInsertResultDto), StatusCodes.Status207MultiStatus)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAll(
        [FromBody] IEnumerable<CreateStarDto> stars,
        [FromQuery] bool allowPartialInsert = false)
    {
        var result = await _starService.CreateAllAsync(stars, allowPartialInsert);

        return result.StatusCode switch
        {
            200 => Ok(result),
            207 => StatusCode(207, result),
            _ => throw new InvalidOperationException("Unexpected insert result")
        };
    }

    /// <summary>
    /// Retrieves a star by its ID.
    /// </summary>
    /// <param name="id">The ID of the star.</param>
    /// <returns>The requested star if found.</returns>
    /// <response code="200">Star found.</response>
    /// <response code="404">Star not found.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(StarDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReadById(int id)
    {
        var star = await _starService.ReadAsync(id);
        return star is null ? NotFound() : Ok(star);
    }

    /// <summary>
    /// Retrieves a star by its name.
    /// </summary>
    /// <param name="name">The name of the star.</param>
    /// <returns>The requested star if found.</returns>
    /// <response code="200">Star found.</response>
    /// <response code="404">Star not found.</response>
    [HttpGet("{name}")]
    [ProducesResponseType(typeof(StarDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReadByName(string name)
    {
        var star = await _starService.ReadAsync(name);
        return star is null ? NotFound() : Ok(star);
    }

    /// <summary>
    /// Retrieves all stored stars.
    /// </summary>
    /// <returns>A list of all stars.</returns>
    /// <response code="200">Stars retrieved successfully.</response>
    [HttpGet]
    [Route("all")]
    [ProducesResponseType(typeof(IEnumerable<StarDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ReadAll()
    {
        var stars = await _starService.ReadAllAsync();
        return Ok(stars);
    }

    /// <summary>
    /// Updates an existing star by ID.
    /// </summary>
    /// <param name="id">The ID of the star to update.</param>
    /// <param name="starDto">The updated star data.</param>
    /// <returns>The updated star.</returns>
    /// <response code="200">Star updated successfully.</response>
    /// <response code="400">Invalid update data.</response>
    /// <response code="404">Star not found.</response>
    [HttpPut("update/{id}")]
    [ProducesResponseType(typeof(StarDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateStarDto starDto)
    {
        var updated = await _starService.UpdateAsync(id, starDto);
        return Ok(updated);
    }

    /// <summary>
    /// Deletes a star by ID.
    /// </summary>
    /// <param name="id">The ID of the star to delete.</param>
    /// <returns>No content if deleted; Not found if the star doesn't exist.</returns>
    /// <response code="204">Star deleted successfully.</response>
    /// <response code="404">Star not found.</response>
    [HttpDelete("delete/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _starService.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }
}
