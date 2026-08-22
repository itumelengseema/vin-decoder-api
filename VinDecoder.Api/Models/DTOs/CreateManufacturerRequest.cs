namespace VinDecoder.Api.Models.DTOs;

public class CreateManufacturerRequest
{
    public required string Wmi { get; set; }
    public required string Name { get; set; }
}