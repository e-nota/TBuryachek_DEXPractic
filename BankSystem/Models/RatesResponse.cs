using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Models
{
    public class RatesResponse
    {
        public int status { get; set; }
        public string message { get; set; }
        public CrossCurrency data { get; set; }

    }
}
