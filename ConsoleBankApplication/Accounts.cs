// Name: Jefferson Duarte
// ID: 71205

using System;
using System.IO;

// Abstract base class representing a generic account
public abstract class Account
{
    // Property to store the balance of the account
    public decimal Balance { get; set; }

    // Method to deposit funds into the account
    public virtual void Lodge(decimal amount, Customer customer)
    {
        // Increase the account balance by the deposited amount
        Balance += amount;

        // Record the transaction
        RecordTransaction(new Transaction("Lodgement", amount, Balance), GetFileName(customer));
    }

    // Method to withdraw funds from the accoun
    public virtual void Withdraw(decimal amount, Customer customer)
    {
        // Check if there are sufficient funds in the account
        if (amount > Balance)
        {
            // Throw an exception if there are insufficient funds
            throw new InsufficientFundsException();
        }

        // Decrease the account balance by the withdrawn amount
        Balance -= amount;

        // Record the transaction
        RecordTransaction(new Transaction("Withdrawal", amount, Balance), GetFileName(customer));
    }

    // Abstract method to get the file name for storing transaction records
    protected abstract string GetFileName(Customer customer);

    // Method to record a transaction in a file
    protected void RecordTransaction(Transaction transaction, string fileName)
    {
        try
        {
            // Write transaction details to the file
            using (StreamWriter writer = new StreamWriter(fileName, true))
            {
                writer.WriteLine($"{transaction.Timestamp}\t{transaction.Action}\t{transaction.Amount}\t{transaction.Balance}");
            }
        }
        catch (IOException ex)
        {
            // Handle IO exceptions
            Console.WriteLine($"Error writing transaction to file: {ex.Message}");
        }
    }
}

// Class representing a current account, derived from the Account class
public class CurrentAccount : Account
{
    // Override the Lodge method
    public override void Lodge(decimal amount, Customer customer)
    {
        base.Lodge(amount, customer);
    }

    // Override the Withdraw method
    public override void Withdraw(decimal amount, Customer customer)
    {
        base.Withdraw(amount, customer);
    }

    // Implement the abstract method to get the current file name
    protected override string GetFileName(Customer customer)
    {
        // Get the first name and last name of the customer
        string fn = customer.FirstName;
        string ln = customer.LastName;

        // Generate the account number based on the customer's name
        string accountNumber = BankApplication.GenerateAccountNumber(fn, ln);

        // Get the file name for the current account
        return BankApplication.GenerateAccountFiles(accountNumber).currentFileName;
    }
}

// Class representing a savings account, derived from the Account class
public class SavingsAccount : Account
{
    // Override the Lodge method
    public override void Lodge(decimal amount, Customer customer)
    {
        base.Lodge(amount, customer);
    }

    // Override the Withdraw method
    public override void Withdraw(decimal amount, Customer customer)
    {
        base.Withdraw(amount, customer);
    }

    // Implement the abstract method to get the savings file name
    protected override string GetFileName(Customer customer)
    {
        // Get the first name and last name of the customer
        string fn = customer.FirstName;
        string ln = customer.LastName;

        // Generate the account number based on the customer's name
        string accountNumber = BankApplication.GenerateAccountNumber(fn, ln);

        // Get the file name for the savings account
        return BankApplication.GenerateAccountFiles(accountNumber).savingsFileName;
    }
}

// Class representing a transaction
public class Transaction
{
    // Properties to store transaction details
    public DateTime Timestamp { get; } // Timestamp of the transaction
    public string Action { get; } // Type of transaction (e.g., lodgement, withdrawal)
    public decimal Amount { get; } // Amount involved in the transaction
    public decimal Balance { get; } // Balance after the transaction

    // Constructor to initialize transaction details
    public Transaction(string action, decimal amount, decimal balance)
    {
        // Set the timestamp to the current date and time
        Timestamp = DateTime.Now;

        // Set the action (e.g., lodgement, withdrawal)
        Action = action;

        // Set the transaction amount
        Amount = amount;

        // Set the balance after the transaction
        Balance = balance;
    }
}
