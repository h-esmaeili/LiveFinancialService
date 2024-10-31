using MarketPulse.Api.Models;
using MarketPulse.Api.ServiceAgent;

namespace MarketPulse.Api.Service
{
    public class InstrumentService : IInstrumentService
    {
        private readonly TiingoApiClient _apiClient;
        private readonly ILogger<InstrumentService> _logger;
        // Mocked data for demonstration
        public InstrumentService(TiingoApiClient apiClient, ILogger<InstrumentService> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }
        public async Task<List<InstrumentModel>> List()
        {
            //var result = await _apiClient.GetInstruments(instrument);
            var result = new List<InstrumentModel>
            {
                new InstrumentModel{ Name = "btcusd", Description = "Bitcoin to US Dollar", BaseCurrency = "BTC", QuoteCurrency = "USD" },
                new InstrumentModel{ Name = "ethusd", Description = "Ethereum to US Dollar", BaseCurrency = "ETH", QuoteCurrency = "USD" },
                new InstrumentModel{ Name = "bnbusd", Description = "BNB to US Dollar", BaseCurrency = "BNB", QuoteCurrency = "USD" },
                new InstrumentModel{ Name = "solusd", Description = "SOL to US Dollar", BaseCurrency = "SOL", QuoteCurrency = "USD" },
            };

            return result;
        }
        public async Task<List<InstrumentDataModel>> GetPrice(string instrument)
        {
            var result = await _apiClient.GetInstrumentPrice(instrument);

            return result;
        }
    }
}
