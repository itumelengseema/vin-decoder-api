using VinDecoder.Api.Models;

namespace VinDecoder.Api.Services
{
    public class VinDecoderService
    {
        private readonly VinCheckDigitService _vinCheckDigitService;
        private readonly VinCountryService _vinCountryService;
        public VinDecoderService(VinCheckDigitService vinCheckDigitService, VinCountryService vinCountryService)
        {
            _vinCheckDigitService = vinCheckDigitService;
            _vinCountryService = vinCountryService;
        }

        public VinDecodeResult Decode(string? vin)
        {
            

            if (string.IsNullOrWhiteSpace(vin))
            {
                throw new ArgumentNullException("VIN cannot be empty.");

            }
            if (vin.Length != 17)
            {
                throw new ArgumentException($"{vin} is not a valid Vin number.VIN must contain exactly 17 characters.");
            }

            vin = vin.ToUpper();


            if (vin.Contains("O") || vin.Contains("I") || vin.Contains("Q"))
            {
                throw new ArgumentException("VIN cannot contain I, O or Q.");
            }

            if (vin.Any(c => !char.IsLetterOrDigit(c)))
            {
                throw new ArgumentException("VIN can only contain letters and numbers.");
            }

            if (!_vinCheckDigitService.IsValid(vin))
            {
                throw new ArgumentException("VIN check digit is invalid.");
            }
            string country = _vinCountryService.GetCountry(vin);
            string wmi = vin.Substring(0, 3);
            string vds = vin.Substring(3, 6);
            string vis = vin.Substring(9, 8);

            VinDecodeResult result = new VinDecodeResult
            {
                Vin = vin,
                Vds = vds,
                Wmi = wmi,
                Vis = vis,
                Country = country,
            };

            return result;
        }
    }
}