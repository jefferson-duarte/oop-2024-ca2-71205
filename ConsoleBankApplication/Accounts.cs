using System;
using System.IO;

public abstract class Account
{
    public decimal Balance { get; set; }

    public virtual void Lodge(decimal amount, Customer customer)
    {
        Balance += amount;
        RecordTransaction(new Transaction("Lodgement", amount, Balance), GetFileName(customer));
    }

    public virtual void Withdraw(decimal amount, Customer customer)
    {
        if (amount > Balance)
        {
            throw new InsufficientFundsException();
        }

        Balance -= amount;
        RecordTransaction(new Transaction("Withdrawal", amount, Balance), GetFileName(customer));
    }

    protected abstract string GetFileName(Customer customer);

    protected void RecordTransaction(Transaction transaction, string fileName)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(fileName, true))
            {
                writer.WriteLine($"{transaction.Timestamp}\t{transaction.Action}\t{transaction.Amount}\t{transaction.Balance}");
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error writing transaction to file: {ex.Message}");
        }
    }
}

public class CurrentAccount : Account
{
    public override void Lodge(decimal amount, Customer customer)
    {
        base.Lodge(amount, customer);
    }

    public override void Withdraw(decimal amount, Customer customer)
    {
        base.Withdraw(amount, customer);
    }

    protected override string GetFileName(Customer customer)
    {
        string fn = customer.FirstName;
        string ln = customer.LastName;
        string accountNumber = BankApplication.GenerateAccountNumber(fn, ln);
        return BankApplication.GenerateAccountFiles(accountNumber).currentFileName;
    }
}

public class SavingsAccount : Account
{
    public override void Lodge(decimal amount, Customer customer)
    {
        base.Lodge(amount, customer);
    }

    public override void Withdraw(decimal amount, Customer customer)
    {
        base.Withdraw(amount, customer);
    }

    protected override string GetFileName(Customer customer)
    {
        string fn = customer.FirstName;
        string ln = customer.LastName;
        string accountNumber = BankApplication.GenerateAccountNumber(fn, ln);
        return BankApplication.GenerateAccountFiles(accountNumber).savingsFileName;
    }
}

public class Transaction
{
    public DateTime Timestamp { get; }
    public string Action { get; }
    public decimal Amount { get; }
    public decimal Balance { get; }

    public Transaction(string action, decimal amount, decimal balance)
    {
        Timestamp = DateTime.Now;
        Action = action;
        Amount = amount;
        Balance = balance;
    }
}
