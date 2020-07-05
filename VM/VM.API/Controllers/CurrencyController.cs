using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VM.Core.Contracts;
using VM.Core.Services;

namespace VM.API.Controllers
{
    [Route("api/")]
    public class CurrencyController : ControllerBase
    {
        private ICurrencyManager _currencyController;
        private readonly ICurrencyManagerFactory _currencyControllerFactory;
        private readonly ILogger<CurrencyController> _logger;

        public CurrencyController(ICurrencyManagerFactory currencyControllerFactory, ILogger<CurrencyController> logger)
        {
            _currencyControllerFactory = currencyControllerFactory;
            _logger = logger;
        }

        [HttpGet]
        public ContentResult Welcome()
        {
            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = StatusCodes.Status200OK,
                Content = "<html><head><meta charset=\"UTF-8\"><title>API</title></head><body><h1>Welcome to exchange API :-)</h1><ul>"
                + "<li>GET localhost:63857/api/{currencyCode} CurrencyCode = usd/brl</li>"
                + "</ul></body></html>"
            };
        }

        [HttpGet, Route("{currencyCode}")]
        public async Task<ActionResult<GetExchangeResponse>> GetExchange(GetExchangeRequest request)
        {
            try
            {
                _currencyController = _currencyControllerFactory.CreateCurrencyController(request.CurrencyCode.ToUpper());
                return await _currencyController.GetExchange();
            }
            catch (ArgumentException argumentException)
            {
                _logger.LogError(argumentException, "Invalid currency");
                return StatusCode(StatusCodes.Status400BadRequest, argumentException.Message);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unhandled exception");
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }
    }
}