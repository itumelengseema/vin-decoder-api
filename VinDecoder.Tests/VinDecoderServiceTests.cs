
using VinDecoder.Api.Services;

namespace VinDecoder.Tests;

public class VinDecoderServiceTests
{
    private readonly VinDecoderService _service;
    
    private const string ValidVinNo = "MALAF51CYNM186853";
    private const string SpecialCharacterVinNo = "MALAF51@YNM186853";
    private const string SixteenCharacterVinNo = "MALAF51CYNM18685";
    
    public VinDecoderServiceTests()
    {
        VinCheckDigitService checkDigitService = new();
        VinCountryService countryService = new();
        VinManufacturerService manufacturerService = new();
        VinModelYearService modelYearService = new();

        _service = new VinDecoderService(
            checkDigitService,
            countryService,
            manufacturerService,
            modelYearService
        );
    }
    
    [Fact]
    public void DecodeVin_WithValidVin_ShouldReturnVinResultObject()
    {
        var result = _service.Decode(ValidVinNo);

        Assert.NotNull(result);
        Assert.Equal(ValidVinNo, result.Vin);
        Assert.Equal("MAL", result.Wmi);
        Assert.Equal("AF51CY", result.Vds);
        Assert.Equal("NM186853", result.Vis);
    }

    [Fact]
    public void DecodeVin_WithSpecialCharacter_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(
            () => _service.Decode(SpecialCharacterVinNo)
        );
    }

    [Fact]
    public void DecodeVin_WithSixteenCharacters_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(
            () => _service.Decode(SixteenCharacterVinNo)
        );
    }
}