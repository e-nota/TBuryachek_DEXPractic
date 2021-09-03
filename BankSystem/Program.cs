using BankSystem.Models;
using BankSystem.Service;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace BankSystem
{
    class Program
    {
        static async Task Main(string[] args)
        {
            MDL lei = new MDL() { Rate = 1.1 , Name = "MDL"};
            RUB rubl = new RUB() { Rate = 0.2 , Name = "RUB"};
            UAH grivna = new UAH() { Rate = 0.65, Name = "UAH"};
            USD dol = new USD() { Rate = 16, Name = "USD"};

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
            Account account4dol = new Account() { currency = dol, cash = 70 };
            Account account5rub = new Account() { currency = rubl, cash = 1 };

            List<Account> account1 = new List<Account> { account1lei, account1rub, account1grn };
            List<Account> account2 = new List<Account> { account2grn };
            List<Account> account3 = new List<Account> { account3lei };
            List<Account> account4 = new List<Account> { account4rub, account4dol};
            List<Account> account5 = new List<Account> { account5rub };
/*
            Dictionary<Client, List<Account>> ClientsBalance = new Dictionary<Client, List<Account>>
            {
                {client1, account1 },
                {client2, account2 },
                {client3, account3 },
                {client4, account4 },
                {client5, account5 }
            };
*/
            Dictionary<string, List<Account>> ClientsBalance = new Dictionary<string, List<Account>>
            {
                {client1.PassNum, account1 },
                {client2.PassNum, account2 },
                {client3.PassNum, account3 },
                {client4.PassNum, account4 },
                {client5.PassNum, account5 }
            };

            FileExportService fileExportServ = new FileExportService();
            fileExportServ.FileExport(client1);
  
            var bankService = new BankService();
 
            Client newclienttxt1 = new Client { Fio = "Petrov1", PassNum = "Q555", YearOfBirth = 1947 };
            Client newclienttxt2 = new Client { Fio = "Komov", PassNum = "Q666", YearOfBirth = 2000 };
            //     bankService.AddClientAccount(newclienttxt1, account1rub);
            //     bankService.AddClientAccount(newclienttxt2, account4dol);

                 bankService.AddClient(newclienttxt1);
                 bankService.AddClient(newclienttxt2);
            /*    List<Client> newClients = new List<Client>();
                newClients.Add(newclienttxt1);
                newClients.Add(newclienttxt2);

                var serClients = JsonConvert.SerializeObject(newClients); // (ClientsBalance);
                bankService.WriteTextToFile(serClients, "Clients.txt");

                            //bankService.ReadClientFromFile("SerDirectoryClients.txt");
                            //

                            string path = Path.Combine("d:", "Courses", "TBuryachek_DEXPractic", "BankSystem", "Files");
                            using (FileStream fileStream = new FileStream($"{path}\\SerDirectoryClients.txt", FileMode.Open))
                            {

                                byte[] array = new byte[fileStream.Length];
                                fileStream.Read(array, 0, array.Length);
                                string text = System.Text.Encoding.Default.GetString(array);


                                Dictionary<string, List<Account>> desClients = JsonConvert.DeserializeObject<Dictionary<string, List<Account>>>(text);
                            }
                  */

            //Получение курсов валют онлайн

            // "status":200,"message":"rates","data":{ "USDRUB":"64.1824"} 
            Exchange exchange = new Exchange();
            decimal convertSum = await exchange.ConverterCurrencyOnlineAsync(10, dol, rubl);
            Console.WriteLine($"Convert 10 USD to RUB: {convertSum.ToString()} RUB");
            decimal convertSum1 = await exchange.ConverterCurrencyOnlineAsync(10, rubl, dol);
            Console.WriteLine($"Convert 10 RUB to USD: {convertSum1.ToString()} USD");

        }
    }
}
