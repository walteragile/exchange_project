using System.Threading.Tasks;
using VM.Core.Services;

namespace VM.Core.Contracts
{
    public interface IExternalService
    {
        Task<GetExchangeResponse> GetExchangeAsync(string currencyCode);
    }
}