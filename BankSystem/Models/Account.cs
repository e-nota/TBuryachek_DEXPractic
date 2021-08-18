using BankSystem.Models;
using BankSystem.Service;

namespace BankSystem.Models
{
    public class Account
    {
        public Currency currency { get; set; } 
        public double cash { get; set;  }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (!(obj is Account))
            {
                return false;
            }

            Account result = (Account)obj;
            return result.currency == currency;

        }

        public override int GetHashCode()
        {
            return currency.GetHashCode();
        }

    }
}
