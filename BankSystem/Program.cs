using BankSystem.BankService;
using BankSystem.Models;
using BankSystem.Service;
using System;


namespace BankSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Client client1 = new Client() { Fio = "Ivanov", PassNum = "Q123" };
            Client client2 = new Client() { Fio = "Petrov", PassNum = "Q124" };
            Client client3 = new Client() { Fio = "Zaicev", PassNum = "Q125" };
            Client client4 = new Client() { Fio = "Volkova", PassNum = "Q126" };
            Client client5 = new Client() { Fio = "Ivanova", PassNum = "Q127" };
            */
            MDL lei = new MDL() { Rate = 1.1 };
            RUB rubl = new RUB() { Rate = 0.2 };
            UAH grivna = new UAH() { Rate = 0.65 };
            USD dol = new USD() { Rate = 16 };

            double sum = 100;
            Exchange ex = new Exchange();
            double convertsum = ex.ConverterCurrency<USD, RUB>(sum, dol, rubl);
            Console.WriteLine($"Convert sum (sum = {sum}, rate usd = {dol.Rate}, rub = {rubl.Rate}) = {convertsum}");
                
        }


    }
}
