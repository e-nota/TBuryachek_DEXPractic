using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BankSystem.Exceptions;
using BankSystem.Models;


namespace BankSystem.Service

{
    public class BankService
    {
        List<Client> clients = new List<Client>();
        List<Employee> employes = new List<Employee>();
        Dictionary<Client, List<Account>> clientsaccounts = new Dictionary<Client, List<Account>>();

        List<Client> clientsFromFile = new List<Client>();
        List<Account> accountsFromFile = new List<Account>();
        List<Employee> employesFromFile = new List<Employee>();
        Dictionary<Client, List<Account>> clientsaccFromFile = new Dictionary<Client, List<Account>>();

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
                
                if ((person is Client) && (!clients.Contains((Client)(IPerson)person)))
                {
                    clients.Add((Client)(IPerson)person);

                    text =  person.Fio + "," + person.PassNum + "," + person.YearOfBirth + "|";
                    WriteTextToFile(text, "Clients.txt");

                }

                if ((person is Employee) && (!employes.Contains((Employee)(IPerson)person)))
                {
                    employes.Add((Employee)(IPerson)person);

                    text =   person.Fio + "," + person.PassNum + "," + person.YearOfBirth + "|";
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

            using (FileStream fileStream = new FileStream($"{path}\\{filename}", FileMode.Append))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(texttowrite);
                fileStream.Write(array, 0, array.Length);
            }


        }

        // ----------------
        public delegate double ExchangeMessageHandler<O, T>(double sum, O currencyfrom, T currencyto) where O : ICurrency
                                                                                      where T : ICurrency;

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
          if (clientsaccounts.ContainsKey(newclient))  // New client is exist in dictionary clients
            {
                List<Account> accountexist = clientsaccounts[newclient];

                if (accountexist.Contains(newaccount))
                {
                    Console.WriteLine($"Such a client already exists!");
                }
                else
                {
                    accountexist.Add(newaccount);
                    clientsaccounts[newclient] = accountexist;
                    Console.WriteLine("New account for exist client added");
                }
            }
            else // New client is NOT exist in dictionary clients
            {   
                List<Account> accounts = new List<Account>();
                accounts.Add(newaccount);
                clientsaccounts.Add(newclient, accounts);

                //
                string text = newclient.Fio + "," + newclient.PassNum + "," + newclient.YearOfBirth + "/";
                text += newaccount.currency + "," + newaccount.cash + "|";
                WriteTextToFile(text, "DirectoryClients.txt");
                //
                ReadClientFromFile("DirectoryClients.txt");
                //

                Console.WriteLine("New client added");
            }
        }

        public void ReadClientFromFile (string filename)
        {
            string path = Path.Combine("d:", "Courses", "TBuryachek_DEXPractic", "BankSystem", "Files");

            using (FileStream fileStream = new FileStream($"{path}\\{filename}", FileMode.Open))
            {
                byte[] array = new byte[fileStream.Length];
                fileStream.Read(array, 0, array.Length);
                string text = System.Text.Encoding.Default.GetString(array);

                int count = text.Split("|").Length-1;
                string [] strlistclients = new string [count];
                strlistclients = text.Split("|");

                Client client = new Client();
                Account account = new Account();
                string[] strdataclient = new string[3];
                string[] strdataaccount = new string[2];

                for (int i = 0; i < count; i++)
                
                {
                  //  Console.WriteLine(strlistclients[i]);

                    int count1 = strlistclients[i].Split("/").Length;
                    string[] str1 = new string[count1];
                    str1 = strlistclients[i].Split("/");

                    for (int j = 0; j < count1; j++)
                    {
                        string[] strclients = new string[count1/2];
                        string[] straccounts = new string[count1 / 2];

                        if (j == 0 || j % 2 == 0)
                        {
                            for (int k = 0; k < count1 / 2; k++)
                            {
                                strclients[k] = str1[j];
                              //  Console.WriteLine(strclients[k]);
                                
                                strdataclient = strclients[k].Split(",");
                           }
                        }
                        else
                        {
                            for (int p = 0; p < count1 / 2; p++)
                            {
                                straccounts[p] = str1[j];
                             //   Console.WriteLine(straccounts[p]);
                                
                                strdataaccount = straccounts[p].Split(",");
                            }
                        }

                        for (int q = 0; q < 3; q++)
                        {
                            switch (q)
                            {
                                case 0:
                                    client.Fio = strdataclient[q];
                                    break;
                                case 1:
                                    client.PassNum = strdataclient[q];
                                    break;
                                case 2:
                                    client.YearOfBirth = Convert.ToInt32(strdataclient[q]);
                                    break;
                                 default:
                                    break;
                            }

                            for (int t = 0; t < 2; t++)
                            {
                                switch (t)
                                {
                                    case 0:
                                        if (strdataaccount[t] == "BankSystem.Service.RUB")
                                        {
                                            account.currency = new RUB();
                                        }
                                        else if (strdataaccount[t] == "BankSystem.Service.USD")
                                        {
                                            account.currency = new USD();
                                        }
                                        else if (strdataaccount[t] == "BankSystem.Service.UAH")
                                        {
                                            account.currency = new UAH();
                                        }
                                        else if (strdataaccount[t] == "BankSystem.Service.MDL")
                                        {
                                            account.currency = new MDL();
                                        }
                                        break;
                                        
                                    case 1:
                                        account.cash = Convert.ToDouble(strdataaccount[t]);
                                        break;
                                    default:
                                             break;

                                }
                            }
                        }

                        if (clientsaccFromFile.ContainsKey(client))  
                        {
                            List<Account> accountexist = clientsaccFromFile[client];

                            if (!accountexist.Contains(account))
                            {
                                accountexist.Add(account);
                                clientsaccFromFile[client] = accountexist;
                            }
                        }
                        else 
                        {
                            List<Account> accounts = new List<Account>();
                            accounts.Add(account);
                            clientsaccFromFile.Add(client, accounts);
                        }
                    }
                }

            }

           

        }

    }
}