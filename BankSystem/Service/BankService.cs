using System.Collections.Generic;
using System.Linq;
using BankSystem.Models;

namespace BankSystem.BankService
{
    public class BankService
    {
        List<Client> clients = new List<Client>();
        List<Employee> employes = new List<Employee>();

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
            if ((person is Client) && (!clients.Contains((Client) (IPerson) person)))
            {
                clients.Add((Client) (IPerson) person);
            }

            if ((person is Employee) && (!employes.Contains((Employee)(IPerson) person)))
            {
                employes.Add((Employee) (IPerson) person);
            }

            return default(T);

        }

    }
}