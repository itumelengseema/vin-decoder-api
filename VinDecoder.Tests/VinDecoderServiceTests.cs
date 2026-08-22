using Microsoft.EntityFrameworkCore;
using VinDecoder.Api.Data;
using VinDecoder.Api.Models;
using VinDecoder.Api.Services;

namespace VinDecoder.Tests;

public class VinDecoderServiceTests
{
    private readonly VinDecoderService _service;

    private const string ValidVinNo = "MALAF51CYNM186853";
    private const string SpecialCharacterVinNo = "MALAF51@YNM186853";
    private const string SixteenCharacterVinNo = "MALAF51CYNM18685";
    private const string EighteenCharacterVinNo = "MALAF51CYNM1868531";
    private const string VinWithICharacter = "MALAF51CIYNM186853";
    private const string VinWithOCharacter = "MALAF51COYNM186853";
    private const string VinWithQCharacter = "MALAF51CQYNM186853";
    private const string? NullVinNo = null;
    private const string EmptyVinNo = "";
    private const string WhitespaceVinNo = "   ";

    public VinDecoderServiceTests()
    {
        // Arrange a separate in-memory database for each test
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        ApplicationDbContext context = new(options);

        // Manufacturer required by the valid test VIN
        context.Manufacturers.Add(
            new Manufacturer
            {
                Id = Guid.NewGuid(),
                Wmi = "MAL",
                Name = "Hyundai Motor India"
            }
        );

        // Region required by the valid test VIN
        context.VinRegions.Add(
            new VinRegion
            {
                Id = Guid.NewGuid(),
                Prefix = "MA",
                Country = "India"
            }
        );

        context.SaveChanges();

        VinCheckDigitService checkDigitService = new();
        VinCountryService countryService = new(context);
        VinManufacturerService manufacturerService = new(context);
        VinModelYearService modelYearService = new();

        _service = new VinDecoderService(
            checkDigitService,
            countryService,
            manufacturerService,
            modelYearService
        );
    }

    [Fact]
    public async Task DecodeVin_WithValidVin_ShouldReturnVinResultObject()
    {
        // Arrange
        string expectedWmi = "MAL";
        string expectedVds = "AF51CY";
        string expectedVis = "NM186853";

        // Act
        VinDecodeResult result = await _service.Decode(ValidVinNo);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ValidVinNo, result.Vin);
        Assert.Equal(expectedWmi, result.Wmi);
        Assert.Equal(expectedVds, result.Vds);
        Assert.Equal(expectedVis, result.Vis);

        Assert.Equal("India", result.Country);
        Assert.Equal("Hyundai Motor India", result.Manufacturer);

        Assert.NotEqual(0, result.YearModel);
    }

    [Fact]
    public async Task DecodeVin_WithSpecialCharacter_ShouldThrowArgumentException()
    {
        // Act & Assert
        ArgumentException exception =
            await Assert.ThrowsAsync<ArgumentException>(
                () => _service.Decode(SpecialCharacterVinNo)
            );

        Assert.Contains(
            "can only contain letters and numbers",
            exception.Message
        );
    }

    [Fact]
    public async Task DecodeVin_WithSixteenCharacters_ShouldThrowArgumentException()
    {
        // Act & Assert
        ArgumentException exception =
            await Assert.ThrowsAsync<ArgumentException>(
                () => _service.Decode(SixteenCharacterVinNo)
            );

        Assert.Contains(
            "exactly 17 characters",
            exception.Message
        );
    }

    [Fact]
    public async Task DecodeVin_WithEighteenCharacters_ShouldThrowArgumentException()
    {
        // Act & Assert
        ArgumentException exception =
            await Assert.ThrowsAsync<ArgumentException>(
                () => _service.Decode(EighteenCharacterVinNo)
            );

        Assert.Contains(
            "exactly 17 characters",
            exception.Message
        );
    }

    [Fact]
    public async Task DecodeVin_WithICharacter_ShouldThrowArgumentException()
    {
        // Act & Assert
        ArgumentException exception =
            await Assert.ThrowsAsync<ArgumentException>(
                () => _service.Decode(VinWithICharacter)
            );

        Assert.Contains(
            "cannot contain I, O or Q",
            exception.Message
        );
    }

    [Fact]
    public async Task DecodeVin_WithOCharacter_ShouldThrowArgumentException()
    {
        // Act & Assert
        ArgumentException exception =
            await Assert.ThrowsAsync<ArgumentException>(
                () => _service.Decode(VinWithOCharacter)
            );

        Assert.Contains(
            "cannot contain I, O or Q",
            exception.Message
        );
    }

    [Fact]
    public async Task DecodeVin_WithQCharacter_ShouldThrowArgumentException()
    {
        // Act & Assert
        ArgumentException exception =
            await Assert.ThrowsAsync<ArgumentException>(
                () => _service.Decode(VinWithQCharacter)
            );

        Assert.Contains(
            "cannot contain I, O or Q",
            exception.Message
        );
    }

    [Fact]
    public async Task DecodeVin_WithNullVin_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _service.Decode(NullVinNo)
        );
    }

    [Fact]
    public async Task DecodeVin_WithEmptyVin_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _service.Decode(EmptyVinNo)
        );
    }

    [Fact]
    public async Task DecodeVin_WithWhitespaceVin_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _service.Decode(WhitespaceVinNo)
        );
    }

    [Fact]
    public async Task DecodeVin_ShouldConvertToUpperCase()
    {
        // Arrange
        string lowerCaseVin = ValidVinNo.ToLower();

        // Act
        VinDecodeResult result =
            await _service.Decode(lowerCaseVin);

        // Assert
        Assert.Equal(
            ValidVinNo.ToUpper(),
            result.Vin
        );
    }

    [Fact]
    public async Task DecodeVin_ShouldReturnCorrectCountryFromDatabase()
    {
        // Act
        VinDecodeResult result =
            await _service.Decode(ValidVinNo);

        // Assert
        Assert.Equal("India", result.Country);
    }

    [Fact]
    public async Task DecodeVin_ShouldReturnCorrectManufacturerFromDatabase()
    {
        // Act
        VinDecodeResult result =
            await _service.Decode(ValidVinNo);

        // Assert
        Assert.Equal(
            "Hyundai Motor India",
            result.Manufacturer
        );
    }

    [Fact]
    public async Task DecodeVin_ShouldExtractCorrectWmi()
    {
        // Act
        VinDecodeResult result =
            await _service.Decode(ValidVinNo);

        // Assert
        Assert.Equal("MAL", result.Wmi);
    }

    [Fact]
    public async Task DecodeVin_ShouldExtractCorrectVds()
    {
        // Act
        VinDecodeResult result =
            await _service.Decode(ValidVinNo);

        // Assert
        Assert.Equal("AF51CY", result.Vds);
    }

    [Fact]
    public async Task DecodeVin_ShouldExtractCorrectVis()
    {
        // Act
        VinDecodeResult result =
            await _service.Decode(ValidVinNo);

        // Assert
        Assert.Equal("NM186853", result.Vis);
    }

    [Fact]
    public async Task DecodeVin_WithValidVin_ShouldReturnModelYear()
    {
        // Act
        VinDecodeResult result =
            await _service.Decode(ValidVinNo);

        // Assert
        Assert.True(result.YearModel > 0);
    }
}