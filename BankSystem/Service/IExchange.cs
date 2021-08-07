using BankSystem.Service;

namespace BankSystem.BankService
{
    public interface IExchange
    {
        public double ConverterCurrency<O, T>(double sum, O originalcurrency, T targetcurrency) where O : ICurrency
                                                                                      where T : ICurrency;
  
    }
}