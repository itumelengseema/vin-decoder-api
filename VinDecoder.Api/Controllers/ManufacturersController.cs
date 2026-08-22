using Microsoft.AspNetCore.Mvc;
using VinDecoder.Api.Models;
using VinDecoder.Api.Models.DTOs;
using VinDecoder.Api.Services;

namespace VinDecoder.Api.Controllers;
[ApiController]
[Route("api/manufacturers")]
public class ManufacturersController: ControllerBase
{
    private readonly VinManufacturerService _vinManufacturerService;

    public ManufacturersController(VinManufacturerService vinManufacturerService)
    {
        _vinManufacturerService = vinManufacturerService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ManufacturerDTO>>> GetManufacturersAsync()
    {
        
        try
        {
            List<ManufacturerDTO> manufacturers = await _vinManufacturerService.GetAllManufacturersAsync();
            return Ok(manufacturers);
        }
        catch (Exception e)
        {
            ErrorResponse errorResponse = new ErrorResponse()
            {
                Status = 404,
                Error = "Not Found",
                Message = e.Message
            };
            return BadRequest(errorResponse);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Manufacturer>> CreateManufacturerAsync(CreateManufacturerRequest request)
    {
        try
        {
            
            var createdManufacturer =
                await _vinManufacturerService.CreateManufacturerAsync(request);

            return Created("", createdManufacturer);
        }
        catch (Exception e)
        {
            ErrorResponse errorResponse = new ErrorResponse()
            {
                Status = 400,
                Error = "Something went wrong",
                Message = e.Message
            };
            return BadRequest(errorResponse);
        }
    }
}