using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanEntity
{
    public class HomeLoan : Loan
    {
        public string PropertyAddress { get; set; }
        public decimal PropertyValue { get; set; }

        public HomeLoan(int loanId, Customer customer, decimal principalAmount, decimal interestRate, int loanTerm, string loanType, string propertyAddress, decimal propertyValue)
            : base(loanId, customer, principalAmount, interestRate, loanTerm, loanType)
        {
            PropertyAddress = propertyAddress;
            PropertyValue = propertyValue;
        }

        public override string ToString()
        {
            return base.ToString() + $", Property Address: {PropertyAddress}, Property Value: {PropertyValue}";
        }
    }
}
