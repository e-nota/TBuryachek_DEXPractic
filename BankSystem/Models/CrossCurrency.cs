using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Models
{
    public class CrossCurrency
    {
        [JsonProperty("USDRUB")]
        public decimal rate { get; set; }
        [JsonProperty("USDUAH")]
        private decimal rate2 { set { rate = value; } }
        [JsonProperty("USDMDL")]
        private decimal rate3 { set { rate = value; } }
        [JsonProperty("RUBUSD")]
        private decimal rate4 { set { rate = value; } }
        [JsonProperty("RUBUAH")]
        private decimal rate5 { set { rate = value; } }
        [JsonProperty("RUBMDL")]
        private decimal rate6 { set { rate = value; } }
        [JsonProperty("UAHUSD")]
        private decimal rate7 { set { rate = value; } }
        [JsonProperty("UAHRUB")]
        private decimal rate8 { set { rate = value; } }
        [JsonProperty("UAHML")]
        private decimal rate9 { set { rate = value; } }
        [JsonProperty("MDLUSD")]
        private decimal rate10 { set { rate = value; } }
        [JsonProperty("MDLRUB")]
        private decimal rate11 { set { rate = value; } }
        [JsonProperty("MDLUAH")]
        private decimal rate12 { set { rate = value; } }
      

        /*
          public string USDRUB { get; set; }
          public string USDUAH { get; set; }
          public string USDMDL { get; set; }
          public string RUBUSD { get; set; }
          public string RUBUAH { get; set; }
          public string RUBMDL { get; set; }
          public string UAHUSD { get; set; }
          public string UAHRUB { get; set; }
          public string UAHMDL { get; set; }
          public string MDLUSD { get; set; }
          public string MDLRUB { get; set; }
          public string MDLUAH { get; set; }
        */
    }
}
