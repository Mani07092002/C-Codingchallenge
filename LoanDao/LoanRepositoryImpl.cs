using LoanEntity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanException;
using Loanutil;

namespace LoanDao
{
    public class LoanRepositoryImpl : ILoanRepository
    {
        string connectionString = DBUtil.ReturnCn("dbcn");

        public void ApplyLoan(Loan loan)
        {
            Console.WriteLine("Do you want to apply for this loan? (Yes/No)");
            string response = Console.ReadLine();
            if (response.ToLower() != "yes") return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Loan (CustomerId, PrincipalAmount, InterestRate, LoanTerm, LoanType, LoanStatus) OUTPUT INSERTED.LoanId VALUES (@CustomerId, @PrincipalAmount, @InterestRate, @LoanTerm, @LoanType, 'Pending')";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CustomerId", loan.Customer.CustomerId);
                cmd.Parameters.AddWithValue("@PrincipalAmount", loan.PrincipalAmount);
                cmd.Parameters.AddWithValue("@InterestRate", loan.InterestRate);
                cmd.Parameters.AddWithValue("@LoanTerm", loan.LoanTerm);
                cmd.Parameters.AddWithValue("@LoanType", loan.LoanType);
                loan.LoanId = (int)cmd.ExecuteScalar();
                Console.WriteLine("Loan application submitted successfully!");
            }
        }
        public decimal CalculateInterest(int loanId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT PrincipalAmount, InterestRate, LoanTerm FROM Loan WHERE LoanId = @LoanId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@LoanId", loanId);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    decimal principalAmount = reader.GetDecimal(0);
                    decimal interestRate = reader.GetDecimal(1);
                    int loanTerm = reader.GetInt32(2);
                    return (principalAmount * interestRate * loanTerm) / 12;
                }

                throw new InvalidOperationException("Loan not found.");
            }
        }

        public void LoanStatus(int loanId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT CreditScore FROM Customer WHERE CustomerId = (SELECT CustomerId FROM Loan WHERE LoanId = @LoanId)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@LoanId", loanId);
                int creditScore = (int)cmd.ExecuteScalar();

                string status = creditScore > 650 ? "Approved" : "Rejected";
                UpdateLoanStatus(loanId, status);
                Console.WriteLine($"Loan Status: {status}");
            }
        }

        private void UpdateLoanStatus(int loanId, string status)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Loan SET LoanStatus = @Status WHERE LoanId = @LoanId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@LoanId", loanId);
                cmd.ExecuteNonQuery();
            }
        }

        public decimal CalculateEMI(int loanId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT PrincipalAmount, InterestRate, LoanTerm FROM Loan WHERE LoanId = @LoanId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@LoanId", loanId);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    decimal principalAmount = reader.GetDecimal(0);
                    decimal interestRate = reader.GetDecimal(1) / 12 / 100;
                    int loanTerm = reader.GetInt32(2);
                    return (principalAmount * interestRate * (decimal)Math.Pow((double)(1 + interestRate), loanTerm)) / (decimal)(Math.Pow((double)(1 + interestRate), loanTerm) - 1);
                }

                throw new InvalidOperationException("Loan not found.");
            }
        }

        public void LoanRepayment(int loanId, decimal amount)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                decimal emi = CalculateEMI(loanId);
                if (amount < emi)
                {
                    Console.WriteLine("The amount is less than the EMI. Payment rejected.");
                    return;
                }
                string query = "UPDATE Loan SET LoanStatus = 'Repayment' WHERE LoanId = @LoanId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@LoanId", loanId);
                cmd.ExecuteNonQuery();

                Console.WriteLine($"Repayment of {amount} processed successfully for Loan ID {loanId}.");
            }
        }

        public List<Loan> GetAllLoans()
        {
            List<Loan> loans = new List<Loan>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Loan";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int loanId = reader.GetInt32(0);
                    int customerId = reader.GetInt32(1);
                    decimal principalAmount = reader.GetDecimal(2);
                    decimal interestRate = reader.GetDecimal(3);
                    int loanTerm = reader.GetInt32(4);
                    string loanType = reader.GetString(5);
                    string loanStatus = reader.GetString(6);


                    Customer customer = GetCustomerById(customerId);


                    Loan loan;
                    if (loanType == "HomeLoan")
                    {
                        loan = new HomeLoan(loanId, customer, principalAmount, interestRate, loanTerm, loanType, "Some Address", 0);
                    }
                    else
                    {
                        loan = new CarLoan(loanId, customer, principalAmount, interestRate, loanTerm, loanType, "Some Model", 0);
                    }

                    loans.Add(loan);
                }
            }
            return loans;
        }

        public Loan GetLoanById(int loanId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Loan WHERE LoanId = @LoanId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@LoanId", loanId);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    int customerId = reader.GetInt32(1);
                    decimal principalAmount = reader.GetDecimal(2);
                    decimal interestRate = reader.GetDecimal(3);
                    int loanTerm = reader.GetInt32(4);
                    string loanType = reader.GetString(5);
                    string loanStatus = reader.GetString(6);


                    Customer customer = GetCustomerById(customerId);

                    Loan loan;
                    if (loanType == "HomeLoan")
                    {
                        loan = new HomeLoan(loanId, customer, principalAmount, interestRate, loanTerm, loanType, "Some Address", 0);
                    }
                    else
                    {
                        loan = new CarLoan(loanId, customer, principalAmount, interestRate, loanTerm, loanType, "Some Model", 0);
                    }

                    return loan;
                }
                throw new InvalidOperationException("Loan not found.");
            }
        }

        private Customer GetCustomerById(int customerId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Customer WHERE CustomerId = @CustomerId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CustomerId", customerId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Customer
                        {
                            CustomerId = (int)reader["CustomerId"],
                            Name = reader["Name"].ToString(),
                            EmailAddress = reader["EmailAddress"].ToString(),
                            PhoneNumber = (decimal)reader["PhoneNumber"],
                            Address = reader["Address"].ToString(),
                            CreditScore = (decimal)reader["CreditScore"]
                        };
                    }
                }
            }
            return null;
        }
    }
}