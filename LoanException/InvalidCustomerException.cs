using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanException
{
    public class InvalidCustomerException : Exception
    {

        public InvalidCustomerException(string message) : base(message) { }

    }
}
