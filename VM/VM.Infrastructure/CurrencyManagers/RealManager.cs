using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VM.Core.Contracts;
using VM.Core.Entities;
using VM.Core.Services;

namespace VM.Infrastructure.CurrencyManagers
{
    public class RealManager : ICurrencyManager
    {
        private readonly ICurrencyManager _dollarController;
        private readonly IExchangeRepository _repository;
        private readonly ILogger<RealManager> _logger;

        public string CurrencyCode => "BRL";
        public string CurrencySymbol => "R$";
        public string CurrencyName => "Real";
        public decimal CurrencyMaxPerMonth => 300M;

        public RealManager(ICurrencyManager dollarManager, IExchangeRepository repository, ILogger<RealManager> logger)
        {
            _dollarController = dollarManager;
            _repository = repository;
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

        public async Task<BuyCurrencyResponse> BuyCurrency(int userId, decimal amount)
        {
            var result = new BuyCurrencyResponse();
            try
            {
                var currentFactor = (await GetExchange()).SellPrice;
                var totalCurrencyToBuy = Math.Round(amount / currentFactor, 2);
                var maxPermissible = Math.Round(CurrencyMaxPerMonth - await _repository.GetCurrentTotal(userId, CurrencyCode, DateTime.Today), 2);
                if (totalCurrencyToBuy <= maxPermissible)
                {
                    await _repository.SaveTransaction(new ExchangePurchases
                    {
                        UserId = userId,
                        Currency = CurrencyCode,
                        Date = DateTime.Today,
                        Amount = totalCurrencyToBuy
                    });
                    result.CurrencyCode = CurrencyCode;
                    result.BoughtAmount = totalCurrencyToBuy;
                }
                else
                {
                    throw new Exception($"Quantity: {amount:0.##} is above monthly limit. You can use max {(maxPermissible * currentFactor):0.##}");
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unhandled exception");
                throw;
            }
            return result;
        }
    }
}