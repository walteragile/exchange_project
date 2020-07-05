using System.Threading.Tasks;
using VM.Core.Services;

namespace VM.Core.Contracts
{
    public interface ICurrencyManager
    {
        string CurrencyCode { get; }
        string CurrencySymbol { get; }
        string CurrencyName { get; }
        decimal CurrencyMaxPerMonth { get; }

        Task<GetExchangeResponse> GetExchange();
        Task<BuyCurrencyResponse> BuyCurrency(int userId, decimal amount);
    }
}