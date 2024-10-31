using MarketPulse.Api.Configs;
using MarketPulse.Api.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace MarketPulse.Api.ServiceAgent
{
    public class TiingoApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly TiingoSettings _apiConfig;
        public TiingoApiClient(HttpClient httpClient, IOptions<TiingoSettings> settings)
        {
            _httpClient = httpClient;
            _apiConfig = settings.Value;
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }
        public async Task<List<InstrumentModel>> GetInstruments(string instrument)
        {
            var response = await _httpClient.GetAsync($"tiingo/crypto?token={_apiConfig.ApiKey}");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<InstrumentModel>>(content);

            return result;
        }
        public async Task<List<InstrumentDataModel>> GetInstrumentPrice(string instrument)
        {
            var startDate = DateTime.Now.AddDays(-2).Date.ToString("yyyy-MM-dd");
            var response = await _httpClient.GetAsync($"tiingo/crypto/prices?tickers={instrument}&startDate={startDate}&resampleFreq=5min&token={_apiConfig.ApiKey}");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<InstrumentDataModel>>(content);
            
            return result;
        }
    }
}