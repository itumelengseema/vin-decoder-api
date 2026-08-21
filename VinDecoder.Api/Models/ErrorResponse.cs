namespace VinDecoder.Api.Models;

public class ErrorResponse
{
    public int Status { get; set; }
    public required string Error { get; set; }
    public required string Message { get; set; }
}