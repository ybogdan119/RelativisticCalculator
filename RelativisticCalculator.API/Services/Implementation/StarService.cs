using Microsoft.EntityFrameworkCore;
using RelativisticCalculator.API.Data;
using RelativisticCalculator.API.Exceptions;
using RelativisticCalculator.API.Models.Dto;
using RelativisticCalculator.API.Models.Entities;
using RelativisticCalculator.API.Services.Interfaces;

namespace RelativisticCalculator.API.Services.Implementation;

public class StarService : IStarService
{
    private readonly AppDbContext _db;

    /// <summary>
    /// Initializes a new instance of the <see cref="StarService"/> class.
    /// </summary>
    /// <param name="db">The database context.</param>
    public StarService(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Creates a new star entity in the database.
    /// Throws <see cref="StarConflictException"/> if a star with the same name already exists.
    /// </summary>
    /// <param name="star">The star data to create.</param>
    /// <returns>The created star data transfer object.</returns>
    /// <exception cref="StarConflictException">Thrown when a star with the given name already exists.</exception>
    public async Task<StarDto> CreateAsync(CreateStarDto star)
    {
        var exists = await _db.Stars.AnyAsync(s => s.Name == star.Name);
        if (exists)
        {
            throw new StarConflictException(star.Name);
        }

        var entityEntry = await _db.Stars.AddAsync(new Star
        {
            Name = star.Name,
            DistanceLy = star.DistanceLy,
        });

        await _db.SaveChangesAsync();

        return new StarDto
        {
            Id = entityEntry.Entity.Id,
            DistanceLy = entityEntry.Entity.DistanceLy,
            Name = entityEntry.Entity.Name
        };
    }

    /// <summary>
    /// Creates multiple stars in a batch.
    /// Can optionally allow partial insert when some stars conflict by name.
    /// </summary>
    /// <param name="stars">The collection of stars to create.</param>
    /// <param name="allowPartialInsert">If <c>true</c>, inserts only non-conflicting stars and returns conflicts; otherwise throws on any conflict.</param>
    /// <returns>
    /// A <see cref="PartialInsertResultDto"/> containing inserted stars, conflicts, and status code.
    /// </returns>
    /// <exception cref="StarConflictException">Thrown if <paramref name="allowPartialInsert"/> is <c>false</c> and conflicts exist.</exception>
    public async Task<PartialInsertResultDto> CreateAllAsync(IEnumerable<CreateStarDto> stars, bool allowPartialInsert = false)
    {
        // Materialize the input collection only once
        var starList = stars.ToList();

        // Extract names to be inserted
        var namesToInsert = starList.Select(s => s.Name).ToList();

        // Find names that already exist in the database
        var existingNames = await _db.Stars
            .Where(s => namesToInsert.Contains(s.Name))
            .Select(s => s.Name)
            .ToListAsync();

        // Names that cause conflicts
        var conflicts = existingNames.Intersect(namesToInsert).ToList();

        // If partial insert is not allowed — throw exception
        if (!allowPartialInsert && conflicts.Any())
        {
            throw new StarConflictException(conflicts);
        }

        // Keep only stars that don't conflict
        var insertableStars = starList
            .Where(s => !conflicts.Contains(s.Name))
            .Select(s => new Star
            {
                Name = s.Name,
                DistanceLy = s.DistanceLy
            })
            .ToList();

        // Insert into the database
        if (insertableStars.Any())
        {
            await _db.Stars.AddRangeAsync(insertableStars);
            await _db.SaveChangesAsync();
        }

        // Return insert result
        return new PartialInsertResultDto
        {
            Message = conflicts.Any()
                ? "Some stars were inserted, some already existed."
                : "All stars inserted successfully.",
            StatusCode = conflicts.Any() ? 207 : 200,
            Inserted = insertableStars.Select(s => new StarDto
            {
                Id = s.Id,
                Name = s.Name,
                DistanceLy = s.DistanceLy
            }).ToList(),
            Conflicts = conflicts
        };
    }

    /// <summary>
    /// Reads a star by its ID.
    /// </summary>
    /// <param name="id">The ID of the star to read.</param>
    /// <returns>The star DTO if found; otherwise, <c>null</c>.</returns>
    public async Task<StarDto?> ReadAsync(int id)
    {
        var star = await _db.Stars
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);

        if (star == null)
        {
            return null;
        }

        return new StarDto
        {
            Id = star.Id,
            Name = star.Name,
            DistanceLy = star.DistanceLy
        };
    }

    /// <summary>
    /// Reads a star by its name.
    /// </summary>
    /// <param name="name">The name of the star to read.</param>
    /// <returns>The star DTO if found; otherwise, <c>null</c>.</returns>
    public async Task<StarDto?> ReadAsync(string name)
    {
        var star = await _db.Stars
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Name == name);

        if (star == null)
        {
            return null;
        }

        return new StarDto
        {
            Id = star.Id,
            Name = star.Name,
            DistanceLy = star.DistanceLy
        };
    }

    /// <summary>
    /// Reads all stars from the database.
    /// </summary>
    /// <returns>A collection of all star DTOs.</returns>
    public async Task<IEnumerable<StarDto?>> ReadAllAsync()
    {
        return await _db.Stars.Select(s => new StarDto
        {
            Id = s.Id,
            Name = s.Name,
            DistanceLy = s.DistanceLy
        }).ToListAsync();
    }

    /// <summary>
    /// Updates an existing star.
    /// Throws <see cref="KeyNotFoundException"/> if the star is not found.
    /// Throws <see cref="StarConflictException"/> if the new name conflicts with another star.
    /// </summary>
    /// <param name="id">The ID of the star to update.</param>
    /// <param name="star">The updated star data.</param>
    /// <returns>The updated star DTO.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if star with <paramref name="id"/> does not exist.</exception>
    /// <exception cref="StarConflictException">Thrown if another star with the same name exists.</exception>
    public async Task<StarDto> UpdateAsync(int id, UpdateStarDto star)
    {
        var starToUpdate = await _db.Stars.FindAsync(id);
        if (starToUpdate is null)
        {
            throw new KeyNotFoundException($"Star with ID {id} not found.");
        }

        var nameExists = await _db.Stars.AnyAsync(s => s.Name == star.Name && s.Id != starToUpdate.Id);
        if (nameExists)
        {
            throw new StarConflictException(star.Name);
        }

        starToUpdate.Name = star.Name;
        starToUpdate.DistanceLy = star.DistanceLy;

        await _db.SaveChangesAsync();

        return new StarDto
        {
            Id = starToUpdate.Id,
            Name = starToUpdate.Name,
            DistanceLy = starToUpdate.DistanceLy
        };
    }

    /// <summary>
    /// Deletes a star by its ID.
    /// </summary>
    /// <param name="id">The ID of the star to delete.</param>
    /// <returns><c>true</c> if deletion was successful; <c>false</c> if star was not found.</returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var star = await _db.Stars.FindAsync(id);
        if (star is null)
        {
            return false;
        }

        _db.Stars.Remove(star);
        await _db.SaveChangesAsync();
        return true;
    }
}
