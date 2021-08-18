using BankSystem.Service;

namespace BankSystem.Models
{
    public class Currency : ICurrency
    {
        public double Rate { get; set; }
    }
}
