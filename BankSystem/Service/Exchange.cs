using System;

namespace BankSystem.Service
{
    public class Exchange :IExchange
    {
        public double ConverterCurrency<O, T>(double sum, O originalcurrency, T targetcurrency) where O : ICurrency
                                                                                      where T : ICurrency
        {
            var N = originalcurrency.Rate;
           
            return (Convert.ToDouble(sum) * originalcurrency.Rate / targetcurrency.Rate);
        }
    }
}