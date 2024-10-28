namespace MarketPulse.Api.Models
{
    public class InstrumentModel
    {
        public string Ticker { get; set; }
        public string Name { get; set; }
        public string BaseCurrency { get; set; }
        public string QuoteCurrency { get; set; }
        public string Description { get; set; }
    }
}
