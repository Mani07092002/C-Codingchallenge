namespace LoanException
{
    public class InvalidLoanException : Exception
    {
        public InvalidLoanException(string message) : base(message) { }
    }
}
