using Microsoft.EntityFrameworkCore;
using VinDecoder.Api.Data;

namespace VinDecoder.Api.Services
{
    public class VinCountryService
    {
        private readonly ApplicationDbContext _context;

        public VinCountryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetCountryAsync(string vin)
        {
            string twoCharacterPrefix = vin.Substring(0, 2);
            var region = await _context.VinRegions.FirstOrDefaultAsync(r => r.Prefix == twoCharacterPrefix);

            if (region != null)
            {
                return region.Country;
            }

            string oneCharacterPrefix = vin.Substring(0, 1);

            region = await _context.VinRegions.FirstOrDefaultAsync(r => r.Prefix == oneCharacterPrefix);

            if (region != null)
            {
                return region.Country;
            }
            return "Unknow";
        }
    }
}