using System;
using System.IO;

public class Account()
{
    public decimal Balance { get; set; }

    public virtual void Lodge(decimal amount, Customer customer)
    {
        Balance += amount;
        RecordTransaction("Lodgement", amount);
    }

    public virtual void Withdraw(decimal amount, Customer customer)
    {
        if (amount > Balance)
        {
            throw new InsufficientFundsException();
        }

        Balance -= amount;
        RecordTransaction("Withdrawal", amount);
    }

    protected void RecordTransaction(string action, decimal amount)
    {
    }
}

public class CurrentAccount : Account
{
    public override void Lodge(decimal amount, Customer customer)
    {
        string fn = customer.FirstName;
        string ln = customer.LastName;

        base.Lodge(amount, customer);
        string accountNumber = BankApplication.GenerateAccountNumber(fn, ln);
        string currentFileName = BankApplication.GenerateAccountFiles(accountNumber).currentFileName;

        RecordTransaction(
            "Lodgement",
            amount,
            currentFileName
        );
    }

    public override void Withdraw(decimal amount, Customer customer)
    {
        string fn = customer.FirstName;
        string ln = customer.LastName;

        base.Withdraw(amount, customer);
        string accountNumber = BankApplication.GenerateAccountNumber(fn, ln);
        string currentFileName = BankApplication.GenerateAccountFiles(accountNumber).currentFileName;

        RecordTransaction(
            "Withdrawal",
            amount,
            currentFileName
        );
    }

    private void RecordTransaction(string action, decimal amount, string fileName)
    {
        string transactionRecord = $"{DateTime.Now}\t{action}\t{amount}\t{Balance}";

        using (StreamWriter writer = new StreamWriter(fileName, true))
        {
            writer.WriteLine(transactionRecord);
        }
    }
}

public class SavingsAccount : Account
{
    public override void Lodge(decimal amount, Customer customer)
    {
        string fn = customer.FirstName;
        string ln = customer.LastName;

        base.Lodge(amount, customer);
        string accountNumber = BankApplication.GenerateAccountNumber(fn, ln);
        string savingsFileName = BankApplication.GenerateAccountFiles(accountNumber).savingsFileName;

        RecordTransaction(
            "Lodgement",
            amount,
            savingsFileName
        );
    }

    public override void Withdraw(decimal amount, Customer customer)
    {
        string fn = customer.FirstName;
        string ln = customer.LastName;

        base.Withdraw(amount, customer);
        string accountNumber = BankApplication.GenerateAccountNumber(fn, ln);
        string savingsFileName = BankApplication.GenerateAccountFiles(accountNumber).savingsFileName;

        RecordTransaction(
            "Withdrawal",
            amount,
            savingsFileName
        );
    }

    private void RecordTransaction(string action, decimal amount, string fileName)
    {
        string transactionRecord = $"{DateTime.Now}\t{action}\t{amount}\t{Balance}";

        using (StreamWriter writer = new StreamWriter(fileName, true))
        {
            writer.WriteLine(transactionRecord);
        }
    }
}
