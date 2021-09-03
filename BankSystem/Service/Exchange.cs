using System;
using System.Threading.Tasks;
using BankSystem.Models;
using BankSystem.Service;

namespace BankSystem.Service
{
    public class Exchange :IExchange
    {
        public double ConverterCurrency<O, T>(double sum, O originalcurrency, T targetcurrency) where O : ICurrency
                                                                                      where T : ICurrency
        {
           // var N = originalcurrency.Rate;
           
            return (Convert.ToDouble(sum) * originalcurrency.Rate / targetcurrency.Rate);
        }

        public async Task<decimal> ConverterCurrencyOnlineAsync (decimal sum, Currency originalcurrency, Currency targetcurrency) 
        {
            string apikey = "9b67dd001042de35716280ce3f5f55a1";

            ExchangeRatesService exchangeRatesService = new ExchangeRatesService(apikey);
            var rates = await exchangeRatesService.GetRates(originalcurrency, targetcurrency);
            //CrossCurrency ccur = rates.data;
            decimal crossrate = Convert.ToDecimal(rates.data.rate);

            return (sum * crossrate);
        }
    }
}