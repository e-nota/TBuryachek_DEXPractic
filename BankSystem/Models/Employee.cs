using System;

namespace BankSystem.Models
{
    public class Employee : IPerson
    {
        public string Fio { get; set; }

        public string PassNum { get; set; }
        public int YearOfBirth { get; set; }

        public string Position { get; set; }
    }
}