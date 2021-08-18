using System;

namespace BankSystem.Models
{
    public class Client : Humans
    {
        public override int GetHashCode()
        {
            return Convert.ToInt32(PassNum);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is Client))
            {
                return false;
            }

            Client result = (Client)obj;
            return result.Fio == Fio && result.PassNum == PassNum;
        }
    }
}