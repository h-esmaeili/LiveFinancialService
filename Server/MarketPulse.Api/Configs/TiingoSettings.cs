namespace MarketPulse.Api.Configs
{
    public class TiingoSettings
    {
        public TiingoRest REST { get; set; }
        public TiingoWebSockets WebSockets { get; set; }
        public string ApiKey { get; set; }
    }
    public class TiingoRest
    {
        public string ApiUrl { get; set; }
    }
    public class TiingoWebSockets
    {
        public string Uri { get; set; }
        public string Ticker { get; set; }
    }
}
