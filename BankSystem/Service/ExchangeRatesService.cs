using BankSystem.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Service
{
    public class ExchangeRatesService
    {
        private readonly string _token;
        public ExchangeRatesService(string token)
        {
            _token = token ?? throw new ArgumentNullException(nameof(token));
        }

        public async Task<RatesResponse> GetRates(Currency originalcurrency, Currency targetcurrency)
        {
            HttpResponseMessage responseMessage;
            RatesResponse ratesResponse = new RatesResponse();

                using (var client = new HttpClient())
            {
              //  client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(_token);

                string url = "https://currate.ru/api/?get=rates&pairs=";
                url += originalcurrency.Name + targetcurrency.Name;
                url += "&key="+ _token;

                responseMessage = await client.GetAsync(url);
                responseMessage.EnsureSuccessStatusCode();
                string serializeMessage = await responseMessage.Content.ReadAsStringAsync();
                ratesResponse = JsonConvert.DeserializeObject<RatesResponse>(serializeMessage);
            }

            return ratesResponse;
        }

    }
}
