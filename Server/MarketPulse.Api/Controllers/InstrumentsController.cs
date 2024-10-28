using MarketPulse.Api.Service;
using Microsoft.AspNetCore.Mvc;

namespace MarketPulse.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstrumentsController : ControllerBase
    {
        private readonly IInstrumentService _instrumentService;
        public InstrumentsController(IInstrumentService instrumentService)
        {
            _instrumentService = instrumentService;
        }
        // Mocked data for demonstration
        private static readonly List<string> Instruments = new List<string> { "EURUSD", "USDJPY", "BTCUSD" };

        // GET: api/instruments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetInstruments(string instrument)
        {
            // Retrieve the list of available instruments
            var Instruments = await _instrumentService.List(instrument);

            return Ok(Instruments);
        }

        // GET: api/instruments/{symbol}
        [HttpGet("{symbol}")]
        public async Task<ActionResult<Models.InstrumentPriceModel>> GetInstrumentPrice(string symbol)
        {
            // For demonstration, let's mock a price.
            // You can replace this with a call to your data provider (e.g., Tiingo or Binance).
            var price = await _instrumentService.GetPrice(symbol);
            if (price == null)
            {
                return NotFound($"Instrument '{symbol}' not found.");
            }

            return Ok(price);
        }
    }
}