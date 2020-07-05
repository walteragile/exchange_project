using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VM.Core.Contracts;
using VM.Core.Entities;
using VM.Core.Services;

namespace VM.Infrastructure.CurrencyManagers
{
    public class DollarManager : ICurrencyManager
    {
        private readonly IExternalService _externalService;
        private readonly IExchangeRepository _repository;
        private readonly ILogger<DollarManager> _logger;

        public string CurrencyCode => "USD";
        public string CurrencySymbol => "$";
        public string CurrencyName => "Dolar";
        public decimal CurrencyMaxPerMonth => 200M;

        public DollarManager(IExternalService externalService, IExchangeRepository repository, ILogger<DollarManager> logger)
        {
            _externalService = externalService;
            _repository = repository;
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