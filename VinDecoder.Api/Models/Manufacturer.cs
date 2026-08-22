namespace VinDecoder.Api.Models;

public class Manufacturer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Wmi { get; set; }
    public required string Name { get; set; }
    
}