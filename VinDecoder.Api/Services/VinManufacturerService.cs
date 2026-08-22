using Microsoft.EntityFrameworkCore;
using VinDecoder.Api.Data;
using VinDecoder.Api.Models;
using VinDecoder.Api.Models.DTOs;

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

        public async Task<List<ManufacturerDTO>> GetAllManufacturersAsync()
        {


            return await _context.Manufacturers.Select(m => new ManufacturerDTO
            {
                Wmi = m.Wmi,
                Name = m.Name
            }).ToListAsync();
        }

        public async Task<Manufacturer> CreateManufacturerAsync(CreateManufacturerRequest request
        )
        {
            Manufacturer manufacturer = new Manufacturer()
            {
                Wmi = request.Wmi.ToUpper(),
                Name = request.Name
            };

            await _context.Manufacturers.AddAsync(manufacturer);
            await _context.SaveChangesAsync();
            return manufacturer;
        }
    }
}