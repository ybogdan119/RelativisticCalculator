using RelativisticCalculator.API.Models.Dto;

namespace RelativisticCalculator.API.Services.Interfaces;

/// <summary>
/// Defines operations for managing stars used in relativistic distance calculations.
/// </summary>
public interface IStarService
{
    /// <summary>
    /// Creates a new star.
    /// </summary>
    /// <param name="star">The star data to create.</param>
    /// <returns>The created star.</returns>
    Task<StarDto> CreateAsync(CreateStarDto star);

    /// <summary>
    /// Creates multiple stars at once.
    /// Supports optional partial insertion when some names already exist.
    /// </summary>
    /// <param name="stars">The collection of stars to insert.</param>
    /// <param name="allowPartialInsert">Whether to insert only non-conflicting entries.</param>
    /// <returns>The result of the insertion including any conflicts.</returns>
    Task<PartialInsertResultDto> CreateAllAsync(IEnumerable<CreateStarDto> stars, bool allowPartialInsert);

    /// <summary>
    /// Retrieves a star by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the star.</param>
    /// <returns>The star, or null if not found.</returns>
    Task<StarDto?> ReadAsync(int id);

    /// <summary>
    /// Retrieves a star by its unique name.
    /// </summary>
    /// <param name="name">The name of the star.</param>
    /// <returns>The star, or null if not found.</returns>
    Task<StarDto?> ReadAsync(string name);

    /// <summary>
    /// Retrieves all stars stored in the database.
    /// </summary>
    /// <returns>A collection of all stars.</returns>
    Task<IEnumerable<StarDto?>> ReadAllAsync();

    /// <summary>
    /// Updates an existing star by its ID.
    /// </summary>
    /// <param name="id">The ID of the star to update.</param>
    /// <param name="star">The updated star data.</param>
    /// <returns>The updated star.</returns>
    Task<StarDto> UpdateAsync(int id, UpdateStarDto star);

    /// <summary>
    /// Deletes a star by its ID.
    /// </summary>
    /// <param name="id">The ID of the star to delete.</param>
    /// <returns>True if the star was deleted; false if not found.</returns>
    Task<bool> DeleteAsync(int id);
}
