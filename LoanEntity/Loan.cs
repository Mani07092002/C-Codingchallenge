using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanEntity
{
    public class Loan
    {
        public int LoanId { get; set; }
        public Customer Customer { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestRate { get; set; }
        public int LoanTerm { get; set; }
        public string LoanType { get; set; }
        public string LoanStatus { get; set; }

        public Loan() { }

        public Loan(int loanId, Customer customer, decimal principalAmount, decimal interestRate, int loanTerm, string loanType)
        {
            LoanId = loanId;
            Customer = customer;
            PrincipalAmount = principalAmount;
            InterestRate = interestRate;
            LoanTerm = loanTerm;
            LoanType = loanType;
            LoanStatus = "Pending"; // Default status
        }

        public override string ToString()
        {
            return $"Loan ID: {LoanId}, Customer: {Customer.Name}, Amount: {PrincipalAmount}, Interest Rate: {InterestRate}, Term: {LoanTerm}, Type: {LoanType}, Status: {LoanStatus}";
        }
    }
}
