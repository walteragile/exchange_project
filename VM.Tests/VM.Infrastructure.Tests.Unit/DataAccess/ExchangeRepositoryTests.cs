using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VM.Core.Contracts;
using VM.Core.Entities;
using VM.Infrastructure.DataAccess;

namespace VM.Infrastructure.Tests.Unit.DataAccess
{
    [TestClass]
    public class ExchangeRepositoryTests
    {
        IRepository<ExchangePurchases> _mockRepository;

        IExchangeRepository _target;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = Substitute.For<IRepository<ExchangePurchases>>();

            _target = new ExchangeRepository(_mockRepository);
        }

        [TestMethod]
        public async Task GetCurrentTotal_OnData_Success()
        {
            // Arrange
            IEnumerable<ExchangePurchases> fakePurchaseSet = new List<ExchangePurchases>()
            {
                new ExchangePurchases { Amount = 1.5M }
            };
            _mockRepository.Get(Arg.Any<Expression<Func<ExchangePurchases, bool>>>())
                .Returns(Task.FromResult(fakePurchaseSet));

            // Act
            var result = await _target.GetCurrentTotal(1, "USD", DateTime.Today);

            // Assert
            Assert.AreEqual(1.5M, result);
        }

        [TestMethod]
        public async Task GetCurrentTotal_OnNoData_Returns0()
        {
            // Arrange            
            _mockRepository.Get(Arg.Any<Expression<Func<ExchangePurchases, bool>>>())
                .Returns(Task.FromResult((IEnumerable<ExchangePurchases>)null));

            // Act
            var result = await _target.GetCurrentTotal(1, "USD", DateTime.Today);

            // Assert
            Assert.AreEqual(0M, result);
        }
    }
}