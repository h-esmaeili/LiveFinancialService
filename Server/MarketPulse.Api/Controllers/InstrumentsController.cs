using Microsoft.AspNetCore.Mvc;

namespace MarketPulse.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstrumentsController : ControllerBase
    {
        // Mocked data for demonstration
        private static readonly List<string> Instruments = new List<string> { "EURUSD", "USDJPY", "BTCUSD" };

        // GET: api/instruments
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetInstruments()
        {
            // Retrieve the list of available instruments
            return Ok(Instruments);
        }

        // GET: api/instruments/{symbol}
        [HttpGet("{symbol}")]
        public async Task<ActionResult<decimal>> GetInstrumentPrice(string symbol)
        {
            // For demonstration, let's mock a price.
            // You can replace this with a call to your data provider (e.g., Tiingo or Binance).
            var price = symbol switch
            {
                "EURUSD" => 1.2345m,
                "USDJPY" => 109.75m,
                "BTCUSD" => 50000.00m,
                _ => (decimal?)null
            };

            if (price == null)
            {
                return NotFound($"Instrument '{symbol}' not found.");
            }

            return Ok(price);
        }
    }
}