namespace VinDecoder.Api.Services
{
    public class VinCountryService
    {
        public string GetCountry(string vin)
        {
            char firstCharacter = vin[0];
            string countryCode = vin.Substring(0, 2);

            // South African WMI range
            if (countryCode == "AA" ||
                countryCode == "AB" ||
                countryCode == "AC" ||
                countryCode == "AD" ||
                countryCode == "AE" ||
                countryCode == "AF" ||
                countryCode == "AG" ||
                countryCode == "AH")
            {
                return "South Africa";
            }

            switch (firstCharacter)
            {
                case '1':
                case '4':
                case '5':
                    return "United States";

                case '2':
                    return "Canada";

                case '3':
                    return "Mexico";

                case 'J':
                    return "Japan";

                case 'K':
                    return "South Korea";

                case 'W':
                    return "Germany";

                default:
                    return "Unknown";
            }
        }
    }
}