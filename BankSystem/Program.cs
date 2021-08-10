using BankSystem.Models;
using BankSystem.Service;
using System;
using System.Collections.Generic;

namespace BankSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            MDL lei = new MDL() { Rate = 1.1 };
            RUB rubl = new RUB() { Rate = 0.2 };
            UAH grivna = new UAH() { Rate = 0.65 };
            USD dol = new USD() { Rate = 16 };

            Client client1 = new Client() { Fio = "Ivanov", PassNum = "Q123", YearOfBirth = 1991 };
            Client client2 = new Client() { Fio = "Petrov", PassNum = "Q124", YearOfBirth = 1998 };
            Client client3 = new Client() { Fio = "Zaicev", PassNum = "Q125", YearOfBirth = 1974 };
            Client client4 = new Client() { Fio = "Volkova", PassNum = "Q126", YearOfBirth = 1981 };
            Client client5 = new Client() { Fio = "Ivanova", PassNum = "Q127", YearOfBirth = 2003 };
            
            // --- Few accounts for one client ---
            Account account1lei = new Account() { currency = lei, cash = 100 };
            Account account1rub = new Account() { currency = rubl, cash = 50 };
            Account account1grn = new Account() { currency = grivna, cash = 200 };
            Account account2grn = new Account() { currency = grivna, cash = 10.5 };
            Account account3lei = new Account() { currency = lei, cash = 278 };
            Account account4rub = new Account() { currency = rubl, cash = 1000 };
            Account account4dol = new Account() { currency = dol, cash = 50 };
            Account account5rub = new Account() { currency = rubl, cash = 1 };

            List<Account> account1 = new List<Account> { account1lei, account1rub, account1grn };
            List<Account> account2 = new List<Account> { account2grn };
            List<Account> account3 = new List<Account> { account3lei };
            List<Account> account4 = new List<Account> { account4rub, account4dol};
            List<Account> account5 = new List<Account> { account5rub };

            Dictionary<Client, List<Account>> ClientBalance = new Dictionary<Client, List<Account>>
            {
                {client1, account1 },
                {client2, account2 },
                {client3, account3 },
                {client4, account4 },
                {client5, account5 }
            };

            var bankService = new BankService();
            var exch = new Exchange();
            var exchangeHandler = new BankService.ExchangeMessageHandler<ICurrency, ICurrency>(exch.ConverterCurrency);

            bankService.MoneyTransfer(1, account1lei, account1rub, exchangeHandler);
            bankService.MoneyTransfer(20, account1lei, account1rub, exchangeHandler); // Exception
            bankService.MoneyTransfer(1, account1lei, account1rub, exchangeHandler);
            bankService.MoneyTransfer(10, account1lei, account1rub, exchangeHandler);

            // ----------------
            Console.WriteLine("----------------------------------");
            Client newclient1 = new Client { Fio = "Tabakov", PassNum = "Q333", YearOfBirth = 1947 };
            Account newaccount1 = new Account { currency = rubl, cash = 200 };
            bankService.AddClientAccount(newclient1, newaccount1);

            Client newclient2 = client3;
            Account newaccount2 = account3lei;
            bankService.AddClientAccount(newclient2, newaccount2);

            bankService.AddClientAccount(newclient2, newaccount2);
            
            bankService.AddClientAccount(newclient2, newaccount1);


            //--------------
            /* double sum = 100;
             Exchange ex = new Exchange();
             double convertsum = ex.ConverterCurrency<USD, RUB>(sum, dol, rubl);
             Console.WriteLine($"Convert sum (sum = {sum}, rate usd = {dol.Rate}, rub = {rubl.Rate}) = {convertsum}");
            */
            //----------------
        }

    }
}
