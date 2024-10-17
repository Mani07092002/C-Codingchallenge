using LoanException;
using LoanEntity;
using LoanDao;
using Loanutil;
class Program
    {
        static void Main(string[] args)
        {
            ILoanRepository loanRepository = new LoanRepositoryImpl();
            int choice;

            do
            {
                Console.WriteLine("Loan Management System");
                Console.WriteLine("1. Apply Loan");
                Console.WriteLine("2. Get All Loans");
                Console.WriteLine("3. Get Loan by ID");
                Console.WriteLine("4. Loan Repayment");
                Console.WriteLine("5. Exit");
                Console.Write("Enter your choice: ");
                choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        
                        Console.WriteLine("Enter Customer ID:");
                        int customerId = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter Principal Amount:");
                        decimal principalAmount = decimal.Parse(Console.ReadLine());
                        Console.WriteLine("Enter Interest Rate:");
                        decimal interestRate = decimal.Parse(Console.ReadLine());
                        Console.WriteLine("Enter Loan Term:");
                        int loanTerm = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter Loan Type (HomeLoan/CarLoan):");
                        string loanType = Console.ReadLine();
                        Console.WriteLine("loan Applied Sucessfully");


                    Loan newLoan = new Loan(2,new Customer { CustomerId = customerId }, principalAmount, interestRate, loanTerm, loanType);
                        loanRepository.ApplyLoan(newLoan);
                        break;

                    case 2:
                        
                        List<Loan> loans = loanRepository.GetAllLoans();
                        foreach (var loan in loans)
                        {
                            Console.WriteLine($"Loan ID: {loan.LoanId}, Status: {loan.LoanStatus}");
                        }
                        break;

                    case 3:
                      
                        Console.WriteLine("Enter Loan ID:");
                        int loanId = int.Parse(Console.ReadLine());
                        Loan loanById = loanRepository.GetLoanById(loanId);
                        if (loanById != null)
                        {
                            Console.WriteLine($"Loan ID: {loanById.LoanId}, Customer ID: {loanById.Customer.CustomerId}, Status: {loanById.LoanStatus}");
                        }
                        break;

                    case 4:
                        
                        Console.WriteLine("Enter Loan ID:");
                        int repaymentLoanId = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter Amount to Repay:");
                        decimal amount = decimal.Parse(Console.ReadLine());
                        loanRepository.LoanRepayment(repaymentLoanId, amount);
                        break;

                    case 5:
                        Console.WriteLine("Exiting...");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            } while (choice != 5);
        }
    }
