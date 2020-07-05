namespace VM.Core.Services
{
    public class BuyCurrencyRequest
    {
        public string CurrencyCode { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
    }
}