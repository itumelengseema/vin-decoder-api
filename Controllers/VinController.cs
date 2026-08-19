using Microsoft.AspNetCore.Mvc;
using VinDecoder.Api.Models;
using VinDecoder.Api.Services;

namespace VinDecoder.Api.Controllers
{
    [ApiController]
    [Route("api/vin")]
    public class VinController : ControllerBase
    {
        private readonly VinDecoderService _vinDecoderService;

        public VinController(VinDecoderService vinDecoderService)
        {
            _vinDecoderService = vinDecoderService;
        }

        [HttpGet("{vin}")]
        public VinDecodeResult Decode(string vin)
        {
            VinDecodeResult vinResult = _vinDecoderService.Decode(vin);


            return vinResult;
        }

    }
}
