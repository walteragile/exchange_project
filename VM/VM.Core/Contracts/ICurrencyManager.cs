using System.Threading.Tasks;
using VM.Core.Services;

namespace VM.Core.Contracts
{
    public interface ICurrencyManager
    {
        string CurrencyCode { get; }
        string CurrencySymbol { get; }
        string CurrencyName { get; }

        Task<GetExchangeResponse> GetExchange();
    }
}