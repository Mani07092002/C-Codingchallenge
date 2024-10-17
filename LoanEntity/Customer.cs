namespace LoanEntity
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public decimal PhoneNumber { get; set; }
        public string Address { get; set; }
        public decimal CreditScore { get; set; }
        public Customer() { }
        public Customer(int customerId, string name, string emailAddress, decimal phoneNumber, string address, decimal creditScore)
        {
            CustomerId = customerId;
            Name = name;
            EmailAddress = emailAddress;
            PhoneNumber = phoneNumber;
            Address = address;
            CreditScore = creditScore;
        }
        public override string ToString()
        {
            return $"Customer ID: {CustomerId}, Name: {Name}, Email: {EmailAddress}, Phone: {PhoneNumber}, Address: {Address}, Credit Score: {CreditScore}";
        }
    }
}
