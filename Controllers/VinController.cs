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

        /// <summary>
        /// Decodes and validates a Vehicle Identification Number (VIN).
        /// </summary>
        /// <param name="vin">A 17-character Vehicle Identification Number.</param>
        /// <returns>Decoded VIN information including WMI, VDS, VIS, country, manufacturer and model year.</returns>
        [HttpGet("{vin}")]
        [ProducesResponseType(typeof(VinDecodeResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]  
        public ActionResult<VinDecodeResult> Decode(string vin)
        {
            try
            {
                VinDecodeResult vinResult = _vinDecoderService.Decode(vin);


                return vinResult;
            }
            catch (ArgumentException ex)
            {
                ErrorResponse errorResponse = new ErrorResponse()
                {
                    Status = 400,
                    Error = "Invalid Vin",
                    Message = ex.Message
                };
                return BadRequest(errorResponse);
            }
        }

    }
}
