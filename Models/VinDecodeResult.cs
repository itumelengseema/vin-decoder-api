namespace VinDecoder.Api.Models
{
    public class VinDecodeResult
    {
        public required string Vin { get; set; } 
        public required string Wmi { get; set; } 
        public required string Vds { get; set; }
        public required string Vis { get; set; }
    }
}
