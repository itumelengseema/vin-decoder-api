using Microsoft.EntityFrameworkCore;
using VinDecoder.Api.Data;
using VinDecoder.Api.Models;
using VinDecoder.Api.Models.DTOs;
using VinDecoder.Api.Services;

namespace VinDecoder.Tests;

public class VinManufacturerServiceTests
{
    private readonly ApplicationDbContext _context;
    private readonly VinManufacturerService _service;

    public VinManufacturerServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);

        SeedTestData();

        _service = new VinManufacturerService(_context);
    }

    private void SeedTestData()
    {
        Manufacturer[] manufacturers =
        {
            new Manufacturer
            {
                Id = Guid.NewGuid(),
                Wmi = "1HG",
                Name = "Honda"
            },
            new Manufacturer
            {
                Id = Guid.NewGuid(),
                Wmi = "KMH",
                Name = "Hyundai"
            },
            new Manufacturer
            {
                Id = Guid.NewGuid(),
                Wmi = "WVW",
                Name = "Volkswagen"
            },
            new Manufacturer
            {
                Id = Guid.NewGuid(),
                Wmi = "MAL",
                Name = "Hyundai Motor India"
            },
            new Manufacturer
            {
                Id = Guid.NewGuid(),
                Wmi = "JT2",
                Name = "Toyota"
            }
        };

        _context.Manufacturers.AddRange(manufacturers);
        _context.SaveChanges();
    }

    // --------------------------------------------------
    // GetManufacturerAsync
    // --------------------------------------------------

    [Theory]
    [InlineData("1HGCV41JXMN109186", "Honda")]
    [InlineData("KMHEC4A46EU109186", "Hyundai")]
    [InlineData("WVWZZZ3CZ9E000001", "Volkswagen")]
    [InlineData("MALAF51CYNM186853", "Hyundai Motor India")]
    [InlineData("JT2BF28K0U0036175", "Toyota")]
    public async Task GetManufacturerAsync_WithKnownWmi_ReturnsCorrectManufacturer(
        string vin,
        string expectedManufacturer)
    {
        // Act
        string result = await _service.GetManufacturerAsync(vin);

        // Assert
        Assert.Equal(expectedManufacturer, result);
    }

    [Fact]
    public async Task GetManufacturerAsync_WithUnknownWmi_ReturnsUnknown()
    {
        // Arrange
        string vin = "ZZX12345678901234";

        // Act
        string result = await _service.GetManufacturerAsync(vin);

        // Assert
        Assert.Equal("Unknown", result);
    }

    [Fact]
    public async Task GetManufacturerAsync_UsesFirstThreeCharactersAsWmi()
    {
        // Arrange
        string vin = "1HGAAAAAAAAAAAAAA";

        // Act
        string result = await _service.GetManufacturerAsync(vin);

        // Assert
        Assert.Equal("Honda", result);
    }

    [Fact]
    public async Task GetManufacturerAsync_WithDifferentVinsAndSameWmi_ReturnsSameManufacturer()
    {
        // Arrange
        string vin1 = "1HGCV41JXMN109186";
        string vin2 = "1HGZZZZZZZZZZZZZZ";

        // Act
        string result1 = await _service.GetManufacturerAsync(vin1);
        string result2 = await _service.GetManufacturerAsync(vin2);

        // Assert
        Assert.Equal("Honda", result1);
        Assert.Equal("Honda", result2);
    }

    // --------------------------------------------------
    // GetAllManufacturersAsync
    // --------------------------------------------------

    [Fact]
    public async Task GetAllManufacturersAsync_ReturnsAllManufacturers()
    {
        // Act
        List<ManufacturerDTO> result =
            await _service.GetAllManufacturersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Count);
    }

    [Fact]
    public async Task GetAllManufacturersAsync_ReturnsExpectedManufacturerData()
    {
        // Act
        List<ManufacturerDTO> result =
            await _service.GetAllManufacturersAsync();

        // Assert
        Assert.Contains(result,
            manufacturer =>
                manufacturer.Wmi == "1HG" &&
                manufacturer.Name == "Honda");

        Assert.Contains(result,
            manufacturer =>
                manufacturer.Wmi == "KMH" &&
                manufacturer.Name == "Hyundai");

        Assert.Contains(result,
            manufacturer =>
                manufacturer.Wmi == "MAL" &&
                manufacturer.Name == "Hyundai Motor India");
    }

    [Fact]
    public async Task GetAllManufacturersAsync_ReturnsManufacturerDtos()
    {
        // Act
        List<ManufacturerDTO> result =
            await _service.GetAllManufacturersAsync();

        // Assert
        Assert.All(result, manufacturer =>
        {
            Assert.False(string.IsNullOrWhiteSpace(manufacturer.Wmi));
            Assert.False(string.IsNullOrWhiteSpace(manufacturer.Name));
        });
    }

    // --------------------------------------------------
    // CreateManufacturerAsync
    // --------------------------------------------------

    [Fact]
    public async Task CreateManufacturerAsync_WithValidRequest_CreatesManufacturer()
    {
        // Arrange
        CreateManufacturerRequest request = new()
        {
            Wmi = "ABC",
            Name = "Test Manufacturer"
        };

        // Act
        Manufacturer result =
            await _service.CreateManufacturerAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("ABC", result.Wmi);
        Assert.Equal("Test Manufacturer", result.Name);
    }

    [Fact]
    public async Task CreateManufacturerAsync_WithLowercaseWmi_ConvertsWmiToUppercase()
    {
        // Arrange
        CreateManufacturerRequest request = new()
        {
            Wmi = "xyz",
            Name = "Test Manufacturer"
        };

        // Act
        Manufacturer result =
            await _service.CreateManufacturerAsync(request);

        // Assert
        Assert.Equal("XYZ", result.Wmi);
    }

    [Fact]
    public async Task CreateManufacturerAsync_PersistsManufacturerToDatabase()
    {
        // Arrange
        CreateManufacturerRequest request = new()
        {
            Wmi = "TST",
            Name = "Test Manufacturer"
        };

        // Act
        Manufacturer created =
            await _service.CreateManufacturerAsync(request);

        // Assert
        Manufacturer? savedManufacturer =
            await _context.Manufacturers
                .FirstOrDefaultAsync(m => m.Wmi == "TST");

        Assert.NotNull(savedManufacturer);
        Assert.Equal(created.Id, savedManufacturer.Id);
        Assert.Equal("TST", savedManufacturer.Wmi);
        Assert.Equal("Test Manufacturer", savedManufacturer.Name);
    }

    [Fact]
    public async Task CreateManufacturerAsync_CreatedManufacturerCanBeDecoded()
    {
        // Arrange
        CreateManufacturerRequest request = new()
        {
            Wmi = "NEW",
            Name = "New Manufacturer"
        };

        await _service.CreateManufacturerAsync(request);

        // Act
        string result =
            await _service.GetManufacturerAsync("NEW12345678901234");

        // Assert
        Assert.Equal("New Manufacturer", result);
    }

    [Fact]
    public async Task CreateManufacturerAsync_MultipleManufacturers_GenerateDifferentIds()
    {
        // Arrange
        CreateManufacturerRequest request1 = new()
        {
            Wmi = "NW1",
            Name = "Manufacturer One"
        };

        CreateManufacturerRequest request2 = new()
        {
            Wmi = "NW2",
            Name = "Manufacturer Two"
        };

        // Act
        Manufacturer result1 =
            await _service.CreateManufacturerAsync(request1);

        Manufacturer result2 =
            await _service.CreateManufacturerAsync(request2);

        // Assert
        Assert.NotEqual(result1.Id, result2.Id);
    }
}