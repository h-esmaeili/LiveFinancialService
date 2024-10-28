using MarketPulse.Api.Models;
using MarketPulse.Api.ServiceAgent;

namespace MarketPulse.Api.Service
{
    public class InstrumentService : IInstrumentService
    {
        private readonly MarketApiClient _apiClient;
        public InstrumentService(MarketApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<List<InstrumentDataModel>> GetPrice(string instrument)
        {
            var result = await _apiClient.GetInstrumentPrice(instrument);

            return result;
        }

        public async Task<List<InstrumentModel>> List(string instrument)
        {
            var result = await _apiClient.GetInstruments(instrument);

            return result;
        }
    }
}
