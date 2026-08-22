using Microsoft.EntityFrameworkCore;
using VinDecoder.Api.Data;

namespace VinDecoder.Api.Services
{
    public class VinManufacturerService
    {
        private readonly ApplicationDbContext _context;
      
        public VinManufacturerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<String> GetManufacturerAsync(string vin)
        {
            string wmi = vin.Substring(0, 3);

            var manufacturer = await _context.Manufacturers.FirstOrDefaultAsync(m => m.Wmi == wmi);

            if (manufacturer != null)
            {
                return manufacturer.Name;
            }
            return "Unknown";
        }
    }
}