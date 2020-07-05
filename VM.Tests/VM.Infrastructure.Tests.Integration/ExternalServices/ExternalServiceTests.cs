using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using VM.Core.Contracts;
using VM.Infrastructure.ExternalServices;

namespace VM.Infrastructure.Tests.Integration.ExternalServices
{
    [TestClass]
    public class ExternalServiceTests
    {
        IExternalService _target;

        string _currencyCode;

        [TestInitialize]
        public void TestInitialize()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                { "ExternalURL", "http://www.bancoprovincia.com.ar/Principal" }
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            _target = new ExternalService(configuration, new Logger<ExternalService>(new LoggerFactory()));
        }

        [TestMethod]
        public async Task GetExchangeAsync_OnDollar_Success()
        {
            // Arrange
            _currencyCode = "dolar";

            // Act
            var result = await _target.GetExchangeAsync(_currencyCode);

            // Assert
            Assert.IsNotNull(result);
        }
    }
}