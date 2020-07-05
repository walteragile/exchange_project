using System;
using System.Threading.Tasks;
using VM.Core.Entities;

namespace VM.Core.Contracts
{
    public interface IExchangeRepository
    {
        Task<decimal> GetCurrentTotal(int userId, string currencyCode, DateTime date);
        Task<ExchangePurchases> SaveTransaction(ExchangePurchases exchangePurchase);
    }
}