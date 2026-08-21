using VinDecoder.Api.Services;

namespace VinDecoder.Tests;

public class VinModelYearServiceTests
{
    [Fact]
    public void GetYearModel_shouldReturn2022_WhenYearCodeIsN()
    {
        VinModelYearService service = new VinModelYearService();
        string vin = "MALAF51CYNM186853";

        int result = service.GetYearModel(vin);

        Assert.Equal(2022, result);
    }
}