using System;
using System.Linq;
using System.Threading.Tasks;
using VM.Core.Contracts;
using VM.Core.Entities;

namespace VM.Infrastructure.DataAccess
{
    public class ExchangeRepository : IExchangeRepository
    {
        private readonly IRepository<ExchangePurchases> _repository;

        public ExchangeRepository(IRepository<ExchangePurchases> repository)
        {
            _repository = repository;
        }

        public async Task<decimal> GetCurrentTotal(int userId, string currencyCode, DateTime date)
        {
            var transactionsInMonth = await _repository.Get(e =>
                e.UserId == userId && e.Currency == currencyCode &&
                e.Date.Year == date.Year && e.Date.Month == date.Month);
            return transactionsInMonth == null ? 0M : transactionsInMonth.Sum(t => t.Amount);
        }

        public async Task<ExchangePurchases> SaveTransaction(ExchangePurchases exchangePurchase)
        {
            var result = await _repository.Insert(exchangePurchase);
            await _repository.SaveAsync();
            return result;
        }
    }
}