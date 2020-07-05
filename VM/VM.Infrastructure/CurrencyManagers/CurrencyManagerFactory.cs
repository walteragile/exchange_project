using Microsoft.Extensions.Logging;
using System;
using VM.Core.Contracts;

namespace VM.Infrastructure.CurrencyManagers
{
    public class CurrencyManagerFactory : ICurrencyManagerFactory
    {
        private readonly IExternalService _externalService;
        private readonly ICurrencyManager _dollarManager;

        public CurrencyManagerFactory(IExternalService externalService, ICurrencyManager dollarManager)
        {
            _externalService = externalService;
            _dollarManager = dollarManager;
        }

        public ICurrencyManager CreateCurrencyController(string currencyCode)
        {
            if (currencyCode == "USD")
            {
                return new DollarManager(_externalService, new Logger<DollarManager>(new LoggerFactory()));
            }
            else if (currencyCode == "BRL")
            {
                return new RealManager(_dollarManager, new Logger<RealManager>(new LoggerFactory()));
            }
            else
            {
                throw new ArgumentException("Currency not currently supported");
            }
        }
    }
}