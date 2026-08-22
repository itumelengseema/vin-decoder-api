namespace VinDecoder.Api.Models;

public class VinRegion
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Prefix { get; set; }
    public required string Country { get; set; }
}