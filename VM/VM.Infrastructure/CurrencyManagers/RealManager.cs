using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VM.Core.Contracts;
using VM.Core.Services;

namespace VM.Infrastructure.CurrencyManagers
{
    public class RealManager : ICurrencyManager
    {
        private readonly ICurrencyManager _dollarController;
        private readonly ILogger<RealManager> _logger;

        public string CurrencyCode => "BRL";
        public string CurrencySymbol => "R$";
        public string CurrencyName => "Real";

        public RealManager(ICurrencyManager dollarManager, ILogger<RealManager> logger)
        {
            _dollarController = dollarManager;
            _logger = logger;
        }

        public async Task<GetExchangeResponse> GetExchange()
        {
            GetExchangeResponse result = null;
            try
            {
                var dollarExchange = await _dollarController.GetExchange();
                result = new GetExchangeResponse
                {
                    BuyPrice = dollarExchange.BuyPrice / 4,
                    SellPrice = dollarExchange.SellPrice / 4
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unhandled exception");
            }
            return result;
        }
    }
}