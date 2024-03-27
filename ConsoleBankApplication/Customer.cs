using System;

public class Customer
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string AccountNumber { get; set; }
    public string Pin { get; set; }
    public CurrentAccount CurrentAccount { get; set; }
    public SavingsAccount SavingsAccount { get; set; }

    public Customer(string firstName, string lastName, string email, string accountNumber, string pin, decimal currentAccountBalance, decimal savingsAccountBalance)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        AccountNumber = accountNumber;
        Pin = pin;
        CurrentAccount = new CurrentAccount();
        SavingsAccount = new SavingsAccount();
    }
}

public class InsufficientFundsException : Exception
{
}