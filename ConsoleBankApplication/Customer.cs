// Name: Jefferson Duarte
// ID: 71205

using System;

public class Customer
{
    // Properties to store customer information
    public string FirstName { get; set; } // First name of the customer
    public string LastName { get; set; } // Last name of the customer
    public string Email { get; set; } // Email address of the customer
    public string AccountNumber { get; set; } // Unique account number of the customer
    public string Pin { get; set; } // PIN associated with the customer's account
    public CurrentAccount CurrentAccount { get; set; } // Current account associated with the customer
    public SavingsAccount SavingsAccount { get; set; } // Savings account associated with the customer

    // Constructor to initialize customer object with provided information
    public Customer(string firstName, string lastName, string email, string accountNumber, string pin, decimal currentAccountBalance, decimal savingsAccountBalance)
    {
        // Initialize customer properties
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        AccountNumber = accountNumber;
        Pin = pin;

        // Create new instances of current and savings accounts for the customer
        CurrentAccount = new CurrentAccount();
        SavingsAccount = new SavingsAccount();
    }
}

// Custom exception class for handling insufficient funds during account transactions
public class InsufficientFundsException : Exception
{
    // No additional functionality required; inherits Exception class
}