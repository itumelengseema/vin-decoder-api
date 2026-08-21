using VinDecoder.Api.Services;

namespace VinDecoder.Tests;

public class VinManufacturerServiceTests
{
    [Fact]
    public void GetManufacturer_ShouldReturnHonda_whenWmiIs1HG()
    {
        VinManufacturerService manufacturerService = new VinManufacturerService();
        string vin = "MAKGM2520L4001234";

        string results = manufacturerService.GetManufacturer(vin);
        
        Assert.Equal("Honda", results);
    }
}