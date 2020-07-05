using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using VM.Core.Contracts;
using VM.Core.Services;

namespace VM.Infrastructure.ExternalServices
{
    public class ExternalService : IExternalService
    {
        private readonly HttpClient _client;
        private readonly string _baseURL;
        private readonly ILogger<ExternalService> _logger;

        public ExternalService(IConfiguration configuration, ILogger<ExternalService> logger)
        {
            _client = new HttpClient();
            _baseURL = configuration.GetValue<string>("ExternalUrl");
            _logger = logger;
        }

        public async Task<GetExchangeResponse> GetExchangeAsync(string currency)
        {
            GetExchangeResponse result = null;
            try
            {
                var response = await _client.GetAsync($"{_baseURL}/{currency}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var jsonExchange = JArray.Parse(content);
                    return new GetExchangeResponse()
                    {
                        BuyPrice = (decimal)jsonExchange[0],
                        SellPrice = (decimal)jsonExchange[1]
                    };
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unhandled exception");
            }
            return result;
        }
    }
}