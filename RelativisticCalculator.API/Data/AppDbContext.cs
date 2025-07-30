using Microsoft.EntityFrameworkCore;
using RelativisticCalculator.API.Models.Entities;

namespace RelativisticCalculator.API.Data;

/// <summary>
/// Entity Framework Core database context for the Relativistic Calculator application.
/// Manages access to the database and entity configurations.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Represents the collection of stars stored in the database.
    /// </summary>
    public DbSet<Star> Stars { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class
    /// with the specified options.
    /// </summary>
    /// <param name="options">The options to configure the database context.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Configures the entity mappings and constraints for the database model.
    /// </summary>
    /// <param name="modelBuilder">The model builder used to configure entities.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Ensure the Name field in the Star entity is unique.
        modelBuilder.Entity<Star>()
            .HasIndex(s => s.Name)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}