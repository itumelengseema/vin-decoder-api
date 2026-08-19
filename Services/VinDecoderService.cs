using VinDecoder.Api.Models;

namespace VinDecoder.Api.Services
{
    public class VinDecoderService
    {
        
       public VinDecodeResult Decode(string vin)
        {
            string wmi = vin.Substring(0,3);
            string vds = vin.Substring(3,6);
            string vis = vin.Substring(9,8);

            VinDecodeResult result = new VinDecodeResult
            {
                Vin = vin,
                Vds = vds,
                Wmi = wmi,
                Vis = vis
            };

            return result;
        }
    }
}