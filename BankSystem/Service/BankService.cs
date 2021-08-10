using System;
using System.Collections.Generic;
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

        private IPerson Add<T> (T person) where T :IPerson
        {
            try
            {
                var dat = new DateTime();
                int yearofnow = dat.Year;
                if (yearofnow < person.YearOfBirth + 18)
                {
                    throw new UnderageException($"User {person.YearOfBirth} year of birth. User registration prohibited!");
                }

                if ((person is Client) && (!clients.Contains((Client)(IPerson)person)))
                {
                    clients.Add((Client)(IPerson)person);
                }

                if ((person is Employee) && (!employes.Contains((Employee)(IPerson)person)))
                {
                    employes.Add((Employee)(IPerson)person);
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

        // ----------------
        public delegate double ExchangeMessageHandler<O, T>(double sum, O currencyfrom, T currencyto) where O : ICurrency
                                                                                      where T : ICurrency;

        public void MoneyTransfer(double Sum, Account accountfrom, Account accountto, ExchangeMessageHandler<ICurrency, ICurrency> transferHandler)
        {
            ICurrency cur1 = accountfrom.currency;
            ICurrency cur2 = accountto.currency;
            double exchangesum = transferHandler.Invoke(Sum, cur1, cur2);
            try
            {
                  if (accountfrom.cash < exchangesum)
                {
                    throw new InsufficientFundsException($"Account has Insufficient Funds: {accountfrom.currency} = {accountfrom.cash}, {accountto.currency} = {accountto.cash} ");
                }
                else
                {
                    accountto.cash += exchangesum;
                    accountfrom.cash -= exchangesum;
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
            List<Account> accounts = new List<Account>();
            accounts.Add(newaccount);

            if (clientsaccounts.ContainsKey(newclient))
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
            else
            {
                clientsaccounts.Add(newclient, accounts);
                Console.WriteLine("New client added");
            }
        }

    }
}