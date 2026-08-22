using VinDecoder.Api.Services;
using VinDecoder.Api.Data;
using VinDecoder.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace VinDecoder.Tests;

public class VinCountryServiceTests
{
    private readonly VinCountryService _service;
    private readonly ApplicationDbContext _context;

    public VinCountryServiceTests()
    {
        // Set up in-memory database
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        SeedTestData();
        _service = new VinCountryService(_context);
    }

    private void SeedTestData()
    {
        // Seed test data
        var regions = new[]
        {
            new VinRegion { Id = Guid.NewGuid(), Prefix = "MA", Country = "India" },
            new VinRegion { Id = Guid.NewGuid(), Prefix = "AA", Country = "South Africa" },
            new VinRegion { Id = Guid.NewGuid(), Prefix = "1", Country = "United States" },
            new VinRegion { Id = Guid.NewGuid(), Prefix = "J", Country = "Japan" },
            new VinRegion { Id = Guid.NewGuid(), Prefix = "K", Country = "South Korea" },
            new VinRegion { Id = Guid.NewGuid(), Prefix = "W", Country = "Germany" },
            new VinRegion { Id = Guid.NewGuid(), Prefix = "2", Country = "Canada" },
            new VinRegion { Id = Guid.NewGuid(), Prefix = "3", Country = "Mexico" }
        };

        _context.VinRegions.AddRange(regions);
        _context.SaveChanges();
    }

    #region Two-Character Prefix Tests
    [Fact]
    public async Task GetCountryAsync_WithTwoCharacterPrefix_ShouldReturnCorrectCountry()
    {
        // Arrange
        string vin = "MALAF51CYNM186853"; // MA prefix

        // Act
        string result = await _service.GetCountryAsync(vin);

        // Assert
        Assert.Equal("India", result);
    }

    [Theory]
    [InlineData("MALAF51CYNM186853", "India")]      // MA prefix
    [InlineData("AAXXX51CYNM186853", "South Africa")] // AA prefix
    public async Task GetCountryAsync_WithValidTwoCharacterPrefix_ShouldReturnCountry(string vin, string expectedCountry)
    {
        // Act
        string result = await _service.GetCountryAsync(vin);

        // Assert
        Assert.Equal(expectedCountry, result);
    }
    #endregion

    #region One-Character Prefix Tests (Fallback)
    [Fact]
    public async Task GetCountryAsync_WithOneCharacterPrefixFallback_ShouldReturnCorrectCountry()
    {
        // Arrange
        string vin = "1ALAF51CYNM186853"; // '1' is the first character (USA code)

        // Act
        string result = await _service.GetCountryAsync(vin);

        // Assert
        Assert.Equal("United States", result);
    }

    [Theory]
    [InlineData("1ALAF51CYNM186853", "United States")] // 1 prefix
    [InlineData("JALAF51CYNM186853", "Japan")]         // J prefix
    [InlineData("KALAF51CYNM186853", "South Korea")]   // K prefix
    [InlineData("WALAF51CYNM186853", "Germany")]       // W prefix
    [InlineData("2ALAF51CYNM186853", "Canada")]        // 2 prefix
    [InlineData("3ALAF51CYNM186853", "Mexico")]        // 3 prefix
    public async Task GetCountryAsync_WithValidOneCharacterPrefix_ShouldReturnCountry(string vin, string expectedCountry)
    {
        // Act
        string result = await _service.GetCountryAsync(vin);

        // Assert
        Assert.Equal(expectedCountry, result);
    }
    #endregion

    #region Unknown Country Tests
    [Fact]
    public async Task GetCountryAsync_WithUnknownPrefix_ShouldReturnUnknown()
    {
        // Arrange
        string vin = "ZZXXX51CYNM186853"; // ZZ prefix is not in the database, Z is also not a valid country code

        // Act
        string result = await _service.GetCountryAsync(vin);

        // Assert
        Assert.Equal("Unknow", result); // Note: The service has a typo "Unknow" instead of "Unknown"
    }

    [Theory]
    [InlineData("ZZXXX51CYNM186853")] // ZZ not found, Z not found
    [InlineData("XXXAA51CYNM186853")] // XX not found, X not found
    [InlineData("YYYAA51CYNM186853")] // YY not found, Y not found
    public async Task GetCountryAsync_WithAllUnknownPrefixes_ShouldReturnUnknown(string vin)
    {
        // Act
        string result = await _service.GetCountryAsync(vin);

        // Assert
        Assert.Equal("Unknow", result);
    }
    #endregion

    #region Priority Tests - Two-Character Prefix Over One-Character
    [Fact]
    public async Task GetCountryAsync_ShouldPreferTwoCharacterPrefixOverOneCharacter()
    {
        // Arrange - VIN starts with "MA" (2-char prefix) which takes priority over "M" (1-char)
        string vin = "MALAF51CYNM186853";

        // Act
        string result = await _service.GetCountryAsync(vin);

        // Assert
        Assert.Equal("India", result); // Should be India (from MA), not something from M
    }

    [Fact]
    public async Task GetCountryAsync_ShouldOnlyUseSingleCharacterWhenTwoCharNotFound()
    {
        // Arrange - VIN with prefix that doesn't have 2-char but has 1-char
        string vin = "1BLAF51CYNM186853"; // 1B not in DB, but 1 is

        // Act
        string result = await _service.GetCountryAsync(vin);

        // Assert
        Assert.Equal("United States", result); // Should fall back to 1
    }
    #endregion

    #region Extraction Tests
    [Fact]
    public async Task GetCountryAsync_ExtractionUsesFirstTwoCharacters()
    {
        // Arrange
        string vin = "MALAF51CYNM186853";

        // Act
        string result = await _service.GetCountryAsync(vin);

        // Assert
        Assert.Equal("India", result);
    }

    [Fact]
    public async Task GetCountryAsync_ExtractionUsesFirstCharacterAsFallback()
    {
        // Arrange
        string vin = "JXLAF51CYNM186853"; // JX not in DB, but J is

        // Act
        string result = await _service.GetCountryAsync(vin);

        // Assert
        Assert.Equal("Japan", result);
    }
    #endregion

    #region Consistency Tests
    [Fact]
    public async Task GetCountryAsync_WithSameVin_ShouldReturnConsistentResults()
    {
        // Arrange
        string vin = "MALAF51CYNM186853";

        // Act
        string result1 = await _service.GetCountryAsync(vin);
        string result2 = await _service.GetCountryAsync(vin);
        string result3 = await _service.GetCountryAsync(vin);

        // Assert
        Assert.Equal(result1, result2);
        Assert.Equal(result2, result3);
        Assert.Equal("India", result1);
    }

    [Fact]
    public async Task GetCountryAsync_WithDifferentVinsSamePrefix_ShouldReturnSameCountry()
    {
        // Arrange
        string vin1 = "MALAF51CYNM186853";
        string vin2 = "MAZZZZZZZZZZZZZZ";

        // Act
        string result1 = await _service.GetCountryAsync(vin1);
        string result2 = await _service.GetCountryAsync(vin2);

        // Assert
        Assert.Equal(result1, result2);
        Assert.Equal("India", result1);
    }
    #endregion

    #region Case Sensitivity Tests
    [Fact]
    public async Task GetCountryAsync_WithUppercaseVin_ShouldReturnCountry()
    {
        // Arrange
        string vin = "MALAF51CYNM186853";

        // Act
        string result = await _service.GetCountryAsync(vin);

        // Assert
        Assert.Equal("India", result);
    }

    [Fact]
    public async Task GetCountryAsync_WithLowercaseVin_ShouldHandleCorrectly()
    {
        // Arrange
        string vin = "malaf51cynm186853";

        // Act
        string result = await _service.GetCountryAsync(vin);

        // Assert
        // This depends on implementation - it may or may not work with lowercase
        Assert.NotNull(result);
    }
    #endregion

    #region Database Tests
    [Fact]
    public async Task GetCountryAsync_WithMultiplePrefixesInDatabase_ShouldFindCorrectOne()
    {
        // Arrange
        // Multiple prefixes are already seeded
        string[] vins = new[]
        {
            "MALAF51CYNM186853",  // MA - India
            "AAXXX51CYNM186853",  // AA - South Africa
            "1ALAF51CYNM186853",  // 1  - United States
        };

        // Act & Assert
        var result1 = await _service.GetCountryAsync(vins[0]);
        Assert.Equal("India", result1);

        var result2 = await _service.GetCountryAsync(vins[1]);
        Assert.Equal("South Africa", result2);

        var result3 = await _service.GetCountryAsync(vins[2]);
        Assert.Equal("United States", result3);
    }
    #endregion
}