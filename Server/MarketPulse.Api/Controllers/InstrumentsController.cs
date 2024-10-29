using MarketPulse.Api.Models;
using MarketPulse.Api.Service;
using Microsoft.AspNetCore.Mvc;

namespace MarketPulse.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstrumentsController : ControllerBase
    {
        private readonly IInstrumentService _instrumentService;
        private readonly ILogger<InstrumentsController> _logger;
        public InstrumentsController(IInstrumentService instrumentService, ILogger<InstrumentsController> logger)
        {
            _instrumentService = instrumentService;
            _logger = logger;
        }
     
        // GET: api/instruments
        [HttpGet]
        public async Task<ActionResult<BaseResponse<List<InstrumentModel>>>> GetInstruments()
        {
            var model = new BaseResponse<List<InstrumentModel>>();
            try
            {
                // Retrieve the list of available instruments
                var instruments = await _instrumentService.List();
                model.Data = instruments;
                model.Success = true;

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred when getting instruments.");
                throw ex;
            }
        }
        // GET: api/instruments/{instrument}
        [HttpGet("{instrument}")]
        public async Task<ActionResult<InstrumentPriceModel>> GetInstrumentPrice(string instrument)
        {
            try
            {
                var result = await _instrumentService.GetPrice(instrument);
                if (result == null)
                {
                    return NotFound($"Instrument '{instrument}' not found.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred when getting instrument price.");
                throw ex;
            }
        }
    }
}