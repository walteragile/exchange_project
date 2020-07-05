using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Threading.Tasks;
using VM.Core.Contracts;
using VM.Core.Services;
using VM.Infrastructure.CurrencyManagers;

namespace VM.Infrastructure.Tests.Unit.Controllers
{
    [TestClass]
    public class DollarManagerTests
    {
        IExternalService _mockExternalService;
        ILogger<DollarManager> _mockLogger;

        ICurrencyManager _target;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockExternalService = Substitute.For<IExternalService>();            
            _mockLogger = Substitute.For<ILogger<DollarManager>>();

            _target = new DollarManager(_mockExternalService, _mockLogger);
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
    }
}