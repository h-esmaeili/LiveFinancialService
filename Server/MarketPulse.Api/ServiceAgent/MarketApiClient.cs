using MarketPulse.Api.Models;
using Newtonsoft.Json;

namespace MarketPulse.Api.ServiceAgent
{
    public class MarketApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string token = "f1057513a9f1f229eb129d191a489e9dd58cd357";
        public MarketApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }
        public async Task<List<InstrumentModel>> GetInstruments(string instrument)
        {
            var response = await _httpClient.GetAsync($"tiingo/crypto?token={token}");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<InstrumentModel>>(content);

            return result;
        }
        public async Task<List<InstrumentDataModel>> GetInstrumentPrice(string instrument)
        {
            var startDate = DateTime.Now.AddDays(5).Date.ToString("yyyy-MM-dd");
            var response = await _httpClient.GetAsync($"tiingo/crypto/prices?tickers={instrument}&startDate={startDate}&resampleFreq=5min&token={token}");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<InstrumentDataModel>>(content);
            
            return result;
        }
    }
}
