namespace MarketPulse.Api.Models
{
    public class BaseResponse<T> : BaseResponse
    {
        public T Data { get; set; }
    }
    public class BaseResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}