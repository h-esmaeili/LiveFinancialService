
namespace MarketPulse.Api.Models
{
    public class InstrumentDataModel
    {
        public string Ticker { get; set; }
        public string BaseCurrency { get; set; }
        public string QuoteCurrency { get; set; }
        public List<InstrumentPriceModel> PriceData { get; set; }
    }
    public class InstrumentPriceModel
    {
        public string Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal TradesDone { get; set; }
        public decimal Volume { get; set; }
        public decimal VolumeNotional { get; set; }
    }
}