using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanEntity
{
    public class CarLoan : Loan
    {
        public string CarModel { get; set; }
        public decimal CarValue { get; set; }

        public CarLoan(int loanId, Customer customer, decimal principalAmount, decimal interestRate, int loanTerm, string loanType, string carModel, decimal carValue)
            : base(loanId, customer, principalAmount, interestRate, loanTerm, loanType)
        {
            CarModel = carModel;
            CarValue = carValue;
        }

        public override string ToString()
        {
            return base.ToString() + $", Car Model: {CarModel}, Car Value: {CarValue}";
        }
    }
}
