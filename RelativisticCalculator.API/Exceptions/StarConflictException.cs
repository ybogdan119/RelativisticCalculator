namespace RelativisticCalculator.API.Exceptions;

/// <summary>
/// Represents an exception that is thrown when attempting to insert
/// one or more stars with names that already exist in the database.
/// </summary>
public class StarConflictException : Exception
{
    /// <summary>
    /// Gets the collection of star names that caused the conflict.
    /// </summary>
    public IEnumerable<string> ConflictingNames { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StarConflictException"/> class
    /// with a list of conflicting star names.
    /// </summary>
    /// <param name="conflictingNames">The names of the stars that already exist.</param>
    public StarConflictException(IEnumerable<string> conflictingNames)
        : base("One or more stars already exist.")
    {
        ConflictingNames = conflictingNames;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StarConflictException"/> class
    /// for a single conflicting star name.
    /// </summary>
    /// <param name="singleConflictName">The name of the star that already exists.</param>
    public StarConflictException(string singleConflictName)
        : this(new[] { singleConflictName }) { }
}