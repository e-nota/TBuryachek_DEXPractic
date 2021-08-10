using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Exceptions
{
    public class UnderageException :Exception
    {
        public UnderageException(string message) : base(message)
        {

        }
    }
}
