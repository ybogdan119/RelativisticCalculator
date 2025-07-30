namespace RelativisticCalculator.API.Configuration
{
    /// <summary>
    /// Represents a set of immutable physical constants used in relativistic calculations.
    /// </summary>
    public class PhysicalConstants
    {
        /// <summary>
        /// The standard acceleration due to gravity (in meters per second squared).
        /// Commonly used as the unit for spacecraft acceleration (1 g ≈ 9.81 m/s²).
        /// </summary>
        public double G { get; init; }

        /// <summary>
        /// The speed of light in vacuum (in meters per second).
        /// Typically defined as exactly 299,792,458 m/s.
        /// </summary>
        public double C { get; init; }

        /// <summary>
        /// The length of one light-year expressed in meters.
        /// Used for converting astronomical distances into SI units.
        /// </summary>
        public double LightYearInMeters { get; init; }
    }
}