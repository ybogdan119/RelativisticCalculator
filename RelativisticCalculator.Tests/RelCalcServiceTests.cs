using Microsoft.Extensions.Options;
using RelativisticCalculator.API.Configuration;
using RelativisticCalculator.API.Services.Implementation;

namespace RelativisticCalculator.Tests;

public class RelCalcServiceTests
{
    private readonly RelCalcService _service;

    public RelCalcServiceTests()
    {
        // Настройка физических констант
        var physicalConstants = new PhysicalConstants
        {
            G = 9.80665, // м/с²
            C = 299792458, // м/с
            LightYearInMeters = 9.4607304725808e15 // метров в световом году
        };

        var options = Options.Create(physicalConstants);
        _service = new RelCalcService(options);
    }
    
    
    [Theory]
    [InlineData(1.0, 4.2, true, 3.527, 5.824, 0.9488673114)]
    [InlineData(1.0, 4.2, false, 2.285, 5.08, 0.9822799798)]
    [InlineData(0.3, 550, true, 33.255, 556.4, 0.9999326509)]
    [InlineData(0.3, 550, false, 18.85, 553.22, 0.999982966)]
    [InlineData(10, 863, true, 1.762, 863.2, 0.9999999748)]
    [InlineData(10, 863, false, 0.948, 863, 0.9999999937)]
    public void Calculate_ShouldMatchExpectedPhysicsResults(
        double accelerationG,
        double distanceLy,
        bool decelerate,
        double expectedShipYears,
        double expectedEarthYears,
        double expectedMaxSpeed)
    {
        var result = _service.Calculate(accelerationG, distanceLy, decelerate);

        Assert.InRange(result.YearsPassedShip, expectedShipYears - 0.1, expectedShipYears + 0.1);
        Assert.InRange(result.YearsPassedEarth, expectedEarthYears - 0.1, expectedEarthYears + 0.1);
        Assert.InRange(result.MaxVelocityFractionOfC, expectedMaxSpeed - 0.000001, expectedMaxSpeed + 0.000001);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Calculate_ShouldThrow_WhenAccelerationIsNonPositive(double accelerationG)
    {
        // Arrange
        double distanceLy = 4.2;

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            _service.Calculate(accelerationG, distanceLy, true));
    }

    [Fact]
    public void Calculate_ShouldThrow_WhenAccelerationIsTooLarge()
    {
        // Arrange
        double accelerationG = 1001;
        double distanceLy = 1.0;

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            _service.Calculate(accelerationG, distanceLy, false));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Calculate_ShouldThrow_WhenDistanceIsNonPositive(double distanceLy)
    {
        // Arrange
        double accelerationG = 1.0;

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            _service.Calculate(accelerationG, distanceLy, false));
    }

    [Fact]
    public void Calculate_ShouldThrow_WhenPhysicalParametersInvalid()
    {
        // Arrange
        double accelerationG = 1e-15; // Очень маленькое ускорение
        double distanceLy = 1e-15;    // Очень малая дистанция

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            _service.Calculate(accelerationG, distanceLy, true));
    }
    
    [Fact]
    public void Calculate_ShouldThrow_PeakSpeedBiggerThenC()
    {
        // Arrange
        double accelerationG = 1000; // Очень маленькое ускорение
        double distanceLy = 100000;    // Очень малая дистанция
        bool decelerate = false;

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            _service.Calculate(accelerationG, distanceLy, decelerate));
    }
}