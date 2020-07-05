using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VM.Core.Contracts;
using VM.Core.Services;

namespace VM.Infrastructure.CurrencyManagers
{
    public class DollarManager : ICurrencyManager
    {
        private readonly IExternalService _externalService;
        private readonly ILogger<DollarManager> _logger;

        public string CurrencyCode => "USD";
        public string CurrencySymbol => "$";
        public string CurrencyName => "Dolar";

        public DollarManager(IExternalService externalService, ILogger<DollarManager> logger)
        {
            _externalService = externalService;
            _logger = logger;
        }

        public async Task<GetExchangeResponse> GetExchange()
        {
            GetExchangeResponse result = null;
            try
            {
                result = await _externalService.GetExchangeAsync(CurrencyName);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unhandled exception");
            }
            return result;
        }
    }
}