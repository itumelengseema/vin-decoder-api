using VinDecoder.Api.Services;

namespace VinDecoder.Tests;

public class VinModelYearServiceTests
{
    private readonly VinModelYearService _service;

    public VinModelYearServiceTests()
    {
        _service = new VinModelYearService();
    }

    [Theory]
    [InlineData("MALAF51CYNM186853", 2022)]
    [InlineData("MALAF51CY1M186853", 2001)]
    [InlineData("MALAF51CY2M186853", 2002)]
    [InlineData("MALAF51CYAM186853", 2010)]
    [InlineData("MALAF51CYBM186853", 2011)]
    [InlineData("MALAF51CYLM186853", 2020)]
    [InlineData("MALAF51CYPM186853", 2023)]
    [InlineData("MALAF51CYRM186853", 2024)]
    public void GetYearModel_WithValidYearCode_ShouldReturnCorrectYear(
        string vin,
        int expectedYear)
    {
        // Act
        int result = _service.GetYearModel(vin);

        // Assert
        Assert.Equal(expectedYear, result);
    }

    [Fact]
    public void GetYearModel_WithInvalidYearCode_ShouldReturnZero()
    {
        // Arrange
        string vin = "MALAF51CYZM186853";

        // Act
        int result = _service.GetYearModel(vin);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetYearModel_ShouldUseTenthCharacterOfVin()
    {
        // Arrange
        string vin = "MALAF51CYNM186853";

        // The 10th VIN character is N.
        const int expectedYear = 2022;

        // Act
        int result = _service.GetYearModel(vin);

        // Assert
        Assert.Equal(expectedYear, result);
    }
}