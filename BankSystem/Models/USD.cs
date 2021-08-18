using BankSystem.Models;
using BankSystem.Service;

namespace BankSystem.Service
{
    public class USD : Currency
    {
        public double Rate { get; set; }
    }
}
