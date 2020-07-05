using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Threading.Tasks;
using VM.Core.Contracts;
using VM.Core.Entities;
using VM.Core.Services;
using VM.Infrastructure.CurrencyManagers;

namespace VM.Infrastructure.Tests.Unit.Controllers
{
    [TestClass]
    public class DollarManagerTests
    {
        IExternalService _mockExternalService;
        IExchangeRepository _mockExchangeRepository;
        ILogger<DollarManager> _mockLogger;

        ICurrencyManager _target;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockExternalService = Substitute.For<IExternalService>();
            _mockExchangeRepository = Substitute.For<IExchangeRepository>();
            _mockLogger = Substitute.For<ILogger<DollarManager>>();

            _target = new DollarManager(_mockExternalService, _mockExchangeRepository, _mockLogger);
        }

        [TestMethod]
        public async Task GetExchange_OnCall_Success()
        {
            // Arrange
            var exchange = new GetExchangeResponse { BuyPrice = 30.0M, SellPrice = 40.0M };
            _mockExternalService.GetExchangeAsync(_target.CurrencyName)
                .Returns(Task.FromResult(exchange));

            // Act
            var result = await _target.GetExchange();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task BuyCurrency_OnValidAmount_Success()
        {
            // Arrange
            var exchange = new GetExchangeResponse { BuyPrice = 30.0M, SellPrice = 40.0M };
            _mockExternalService.GetExchangeAsync(_target.CurrencyName)
                .Returns(Task.FromResult(exchange));
            _mockExchangeRepository.GetCurrentTotal(Arg.Any<int>(), "USD", Arg.Any<DateTime>())
                .Returns(Task.FromResult(120M));
            var amountInPesos = 2000;

            // Act
            var result = await _target.BuyCurrency(1, amountInPesos);

            // Assert
            Assert.IsTrue(result.BoughtAmount > 0.00M);
        }

        [TestMethod]
        public async Task BuyCurrency_OnIValidAmount_ThrowsException()
        {
            // Arrange
            var exchange = new GetExchangeResponse { BuyPrice = 30.0M, SellPrice = 40.0M };
            _mockExternalService.GetExchangeAsync(_target.CurrencyName)
                .Returns(Task.FromResult(exchange));
            _mockExchangeRepository.GetCurrentTotal(Arg.Any<int>(), "USD", Arg.Any<DateTime>())
                .Returns(Task.FromResult(151M));
            var amountInPesos = 2000;

            // Act && Assert
            await Assert.ThrowsExceptionAsync<Exception>(async () => await _target.BuyCurrency(1, amountInPesos));
            await _mockExchangeRepository.DidNotReceive().SaveTransaction(Arg.Any<ExchangePurchases>());
        }
    }
}