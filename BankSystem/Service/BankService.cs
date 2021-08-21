using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BankSystem.Exceptions;
using BankSystem.Models;
using Newtonsoft.Json;

namespace BankSystem.Service

{
    public class BankService
    {
        public delegate double ExchangeMessageHandler<O, T>(double sum, O currencyfrom, T currencyto) where O : ICurrency
                                                                                      where T : ICurrency;

        List<Client> clients = new List<Client>();
        List<Employee> employes = new List<Employee>();
        Dictionary<string, List<Account>> clientsaccounts = new Dictionary<string, List<Account>>();

        List<Client> clientsFromFile = new List<Client>();
        List<Account> accountsFromFile = new List<Account>();
        List<Employee> employesFromFile = new List<Employee>();
        Dictionary<string, List<Account>> clientsaccFromFile = new Dictionary<string, List<Account>>();

        
        public Employee FindEmployee(Employee person)
        { 
            return (Employee) Find(person);
        }

        public Client FindClient(Client person)
        {
            return (Client) Find(person);
        }

        private IPerson Find<T>(T person) where T : IPerson
        {
             if (person is Client)
               {
                return (IPerson) clients.Where(p => p.PassNum.Contains(person.PassNum));
               }

               if (person is Employee)
               {
                return (IPerson) employes.Where(p => p.PassNum.Contains(person.PassNum));
               }
            
            return default(T);     
        }

        public Client AddClient(Client person)
        {
            return (Client)Add(person);
        }

        public Employee AddEmployee(Employee person)
        {
            return (Employee)Add(person);
        }

       private IPerson Add<T> (T person) where T :IPerson
        {
            try
            {
                var dat = new DateTime();
                dat = DateTime.Today;
                int yearofnow = dat.Year;

                string text = "";

                if (yearofnow < person.YearOfBirth + 18)
                {
                    throw new UnderageException($"User {person.YearOfBirth} year of birth. User registration prohibited!");
                }

                ReadClientFromFile("Clients.txt");
                if ((person is Client) && (!clientsFromFile.Contains((Client)(IPerson)person)))
                {
                    clientsFromFile.Add((Client)(IPerson)person);

                    text = JsonConvert.SerializeObject(clientsFromFile);
                    WriteTextToFile(text, "Clients.txt");
                }

                ReadClientFromFile("Employee.txt");
                if ((person is Employee) && (!employesFromFile.Contains((Employee)(IPerson)person)))
                {
                    employesFromFile.Add((Employee)(IPerson)person);

                    text = JsonConvert.SerializeObject(employesFromFile);
                    WriteTextToFile(text, "Employee.txt");
                }    
             }
            catch (UnderageException E)
            {
                Console.WriteLine(E);
            }
            catch (Exception E)
            {
                Console.WriteLine(E);
            }

            return default(T);
        }

        public void WriteTextToFile(string texttowrite, string filename)
        {
            string path = Path.Combine("d:", "Courses", "TBuryachek_DEXPractic", "BankSystem", "Files");
            DirectoryInfo directoryInfo = new DirectoryInfo(path);

            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            using (FileStream fileStream = new FileStream($"{path}\\{filename}", FileMode.Create))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(texttowrite);
                fileStream.Write(array, 0, array.Length);
            }
        }


        public void MoneyTransfer(double Sum, Account accountfrom, Account accountto, ExchangeMessageHandler<ICurrency, ICurrency> transferHandler)
        {
            ICurrency cur1 = accountfrom.currency;
            ICurrency cur2 = accountto.currency;
            
            try
            {
                if (Sum < 0)
                {
                    throw new IncorrectSumException($"Sum for exchange is incorrect = {Sum}");
                }
                
                if (accountfrom.cash < Sum) 
                {
                    throw new InsufficientFundsException($"Account has Insufficient Funds: {accountfrom.currency} = {accountfrom.cash}, {accountto.currency} = {accountto.cash} ");
                }
                else
                {
                    double exchangesum = transferHandler.Invoke(Sum, cur1, cur2);

                    accountto.cash += exchangesum;
                    accountfrom.cash -= Sum;
                    Console.WriteLine($"\nTransfer made. Balance now: {accountfrom.currency} = {accountfrom.cash}, {accountto.currency} = {accountto.cash} \n");
                }

            }
            catch (InsufficientFundsException E)
            {
                Console.WriteLine(E);
            }
            catch (Exception E)
            {
                Console.WriteLine(E);
            }
        
        }

        public void FuncMoneyTransfer(double Sum, Account accountfrom, Account accountto, Func<double, ICurrency, ICurrency, double>  transferFunc)
        {
            ICurrency cur1 = accountfrom.currency;
            ICurrency cur2 = accountto.currency;

            try
            {
                if (Sum < 0)
                {
                    throw new IncorrectSumException($"Sum for exchange is incorrect = {Sum}");
                }

                if (accountfrom.cash < Sum)
                {
                    throw new InsufficientFundsException($"Account has Insufficient Funds: {accountfrom.currency} = {accountfrom.cash}, {accountto.currency} = {accountto.cash} ");
                }
                else
                {
                    double exchangesum = transferFunc(Sum, cur1, cur2);

                    accountto.cash += exchangesum;
                    accountfrom.cash -= Sum;
                    Console.WriteLine($"\nTransfer made. Balance now: {accountfrom.currency} = {accountfrom.cash}, {accountto.currency} = {accountto.cash} \n");
                }

            }
            catch (InsufficientFundsException E)
            {
                Console.WriteLine(E);
            }
            catch (Exception E)
            {
                Console.WriteLine(E);
            }

        }
        //-----------------------
        public void AddClientAccount (Client newclient, Account newaccount)
        {
          if (clientsaccounts.ContainsKey(newclient.PassNum))  // New client is exist in dictionary clients
            {
                List<Account> accountexist = clientsaccounts[newclient.PassNum];

                if (accountexist.Contains(newaccount))
                {
                    Console.WriteLine($"Such a client already exists!");
                }
                else
                {
                    accountexist.Add(newaccount);
                    clientsaccounts[newclient.PassNum] = accountexist;
                    Console.WriteLine("New account for exist client added");
                }
            }
            else // New client is NOT exist in dictionary clients
            {   
                List<Account> accounts = new List<Account>();
                accounts.Add(newaccount);
                clientsaccounts.Add(newclient.PassNum, accounts);
        

                Console.WriteLine("New client added");
            }
            var serClientAcc = JsonConvert.SerializeObject(clientsaccounts);
            WriteTextToFile(serClientAcc, "DirectoryClients.txt");
        }

        public void ReadClientFromFile(string filename)
        {
            string path = Path.Combine("d:", "Courses", "TBuryachek_DEXPractic", "BankSystem", "Files");

            string text;

            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
            FileInfo fileInfo = new FileInfo($"{path}\\{filename}");
            if (fileInfo.Exists)
            {
                using (FileStream fileStream = new FileStream($"{path}\\{filename}", FileMode.Open))
                {
                    byte[] array = new byte[fileStream.Length];
                    fileStream.Read(array, 0, array.Length);
                    text = System.Text.Encoding.Default.GetString(array);
                }
                if (filename == "DirectoryClients.txt")
                {
                    clientsaccFromFile = JsonConvert.DeserializeObject<Dictionary<string, List<Account>>>(text);
                }
                if (filename == "Clients.txt")
                {
                    clientsFromFile = JsonConvert.DeserializeObject<List<Client>>(text);
                }
                if (filename == "Employee.txt")
                {
                    employesFromFile = JsonConvert.DeserializeObject<List<Employee>>(text);
                }
            }
            else
            {
                //Console.WriteLine($"File {filename} do not exist!");
            }

        }

    }
}