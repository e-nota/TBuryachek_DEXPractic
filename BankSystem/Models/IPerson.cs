using System;

namespace BankSystem.Models
{
    public interface IPerson
    {
        public string Fio { get; set; }
        public string PassNum { get; set; }
        public int YearOfBirth { get; set; }
           
    }
}