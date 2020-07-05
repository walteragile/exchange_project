namespace VM.Core.Contracts
{
    public interface ICurrencyManagerFactory
    {
        ICurrencyManager CreateCurrencyController(string currencyCode);
    }
}