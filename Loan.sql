create database Loan
use loan
CREATE TABLE Customer (
    CustomerId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL, 
    EmailAddress VARCHAR(50) NOT NULL,
    PhoneNumber Numeric(10),
    Address VARCHAR(50), 
    CreditScore Numeric(3) NOT NULL
);

CREATE TABLE Loan (
    LoanId INT PRIMARY KEY IDENTITY(1,1),
    CustomerId INT FOREIGN KEY REFERENCES Customer(CustomerId),
    PrincipalAmount DECIMAL(18, 2) NOT NULL,
    InterestRate DECIMAL(5, 2) NOT NULL,
    LoanTerm INT NOT NULL,
    LoanType VARCHAR(20) NOT NULL CHECK (LoanType IN ('CarLoan', 'HomeLoan')),
    LoanStatus VARCHAR(10) NOT NULL CHECK (LoanStatus IN ('Pending', 'Approved'))
);
select * from Customer