using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class BankApplication
{
    private static readonly string EmployeePin = "A1234";
    private static List<Customer> customers = new List<Customer>();
    //private static readonly string customersFilePath = $"C:\\Users\\jeffe\\Documents\\.Projetos_C#\\BankApplicationCA2\\ConsoleBanckApp\\customers.txt";
    private static readonly string customersFilePath = $"C:\\Users\\jeffe\\Documents\\.Projetos_C#\\Projeto_Particionado\\ConsoleBankApplication\\customers.txt";

    public static void Main(string[] args)
    {
        LoadCustomers();

        while (true)
        {
            Console.WriteLine("Welcome to the Bank Application");
            Console.WriteLine("1. Login as Bank Employee");
            Console.WriteLine("2. Login as Customer");
            Console.WriteLine("3. Exit");

            int choice = GetIntInput("Enter your choice: ");

            switch (choice)
            {
                case 1:
                    if (LoginEmployee())
                    {
                        EmployeeMenu();
                    }
                    break;
                case 2:
                    if (LoginCustomer(out Customer customer))
                    {
                        CustomerMenu(customer);
                    }
                    break;
                case 3:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private static bool LoginEmployee()
    {
        Console.WriteLine("Enter Employee Pin: ");
        string pin = Console.ReadLine();
        return pin == EmployeePin;
    }

    private static bool LoginCustomer(out Customer customer)
    {
        Console.WriteLine("Enter First Name: ");
        string firstName = Console.ReadLine();
        Console.WriteLine("Enter Last Name: ");
        string lastName = Console.ReadLine();
        Console.WriteLine("Enter Account Number: ");
        string accountNumber = Console.ReadLine();
        Console.WriteLine("Enter PIN: ");
        string pin = Console.ReadLine();

        customer = customers.Find(c => c.FirstName == firstName && c.LastName == lastName && c.AccountNumber == accountNumber && c.Pin == pin);
        return customer != null;
    }

    private static void EmployeeMenu()
    {
        while (true)
        {
            Console.WriteLine("\nEmployee Menu");
            Console.WriteLine("1. Create Customer Account");
            Console.WriteLine("2. Delete Customer Account");
            Console.WriteLine("3. Create Transaction (Lodge/Withdraw)");
            Console.WriteLine("4. List All Customers with Balances");
            Console.WriteLine("5. List All Customer Account Numbers");
            Console.WriteLine("6. Exit");

            int choice = GetIntInput("Enter your choice: ");

            switch (choice)
            {
                case 1:
                    CreateCustomerAccount();
                    break;
                case 2:
                    DeleteCustomerAccount();
                    break;
                case 3:
                    CreateTransaction();
                    break;
                case 4:
                    ListAllCustomersWithBalances();
                    break;
                case 5:
                    ListAllCustomerAccountNumbers();
                    break;
                case 6:
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private static void CustomerMenu(Customer customer)
    {
        while (true)
        {
            Console.WriteLine("\nCustomer Menu - Welcome, {0} {1}", customer.FirstName, customer.LastName);
            Console.WriteLine("1. View Current Account Balance");
            Console.WriteLine("2. View Savings Account Balance");
            Console.WriteLine("3. Lodge Money");
            Console.WriteLine("4. Withdraw Money");
            Console.WriteLine("5. Exit");

            int choice = GetIntInput("Enter your choice: ");

            switch (choice)
            {
                case 1:
                    Console.WriteLine($"Your Current Account Balance: {customer.CurrentAccount.Balance}");
                    break;
                case 2:
                    Console.WriteLine($"Your Savings Account Balance: {customer.SavingsAccount.Balance}");
                    break;
                case 3:
                    DepositMoney(customer);
                    break;
                case 4:
                    WithdrawMoney(customer);
                    break;
                case 5:
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    public static void CreateCustomerAccount()
    {
        Console.WriteLine("Enter First Name: ");
        string firstName = Console.ReadLine();
        Console.WriteLine("Enter Last Name: ");
        string lastName = Console.ReadLine();
        Console.WriteLine("Enter Email: ");
        string email = Console.ReadLine();

        string accountNumber = GenerateAccountNumber(firstName, lastName);
        GenerateAccountFiles(accountNumber);
        string pin = GeneratePin(accountNumber);

        Customer newCustomer = new Customer(firstName, lastName, email, accountNumber, pin, 0, 0);
        customers.Add(newCustomer);

        SaveCustomerData(newCustomer);

        Console.WriteLine("Customer account created successfully!");
        Console.WriteLine($"Account Number: {newCustomer.AccountNumber}");
        Console.WriteLine($"PIN: {newCustomer.Pin}");
    }

    private static void SaveCustomerData(Customer customer)
    {
        string filePath = $"C:\\Users\\jeffe\\Documents\\.Projetos_C#\\BankApplicationCA2\\ConsoleBanckApp\\{customer.AccountNumber}.txt";
    
        try
        {
            // Check if the files already exist
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();
            }            
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error creating account files: {ex.Message}");
        }

        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(
                    $"{customer.FirstName}," +
                    $"{customer.LastName}," +
                    $"{customer.Email}," +
                    $"{customer.AccountNumber}," +
                    $"{customer.Pin}," +
                    $"{customer.CurrentAccount.Balance}," +
                    $"{customer.SavingsAccount.Balance}"
                );
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error writing customer data to file: {ex.Message}");
        }
    }

    private static void LoadCustomers()
    {
        if (File.Exists(customersFilePath))
        {
            try
            {
                //customers.Clear();
                using (StreamReader reader = new StreamReader(customersFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        string firstName = parts[0];
                        string lastName = parts[1];
                        string email = parts[2];
                        string accountNumber = parts[3];
                        string pin = parts[4];
                        decimal currentBalance = decimal.Parse(parts[5]);
                        decimal savingsBalance = decimal.Parse(parts[6]);

                        Customer customer = new Customer(firstName, lastName, email, accountNumber, pin, currentBalance, savingsBalance);
                        customers.Add(customer);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error loading customer data: {ex.Message}");
            }
        }
    }
    public static string GenerateAccountNumber(string firstName, string lastName)
    {
        string initials = $"{firstName.ToUpper()[0]}{lastName.ToUpper()[0]}";
        int fullNameLength = firstName.Length + lastName.Length;

        string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        int firstInitialPosition = alphabet.IndexOf(initials[0]) + 1;
        int secondInitialPosition = alphabet.IndexOf(initials[1]) + 1;

        return $"{initials}-{fullNameLength}-{firstInitialPosition}-{secondInitialPosition}";
    }
    public static (string currentFileName, string savingsFileName) GenerateAccountFiles(string accountNumber)
    {
        // Generate file names with leading zeros for account number
        string savingsFileName =
            $"C:\\Users\\jeffe\\Documents\\.Projetos_C#\\BankApplicationCA2\\ConsoleBanckApp\\{accountNumber.PadLeft(8, '0')}-savings.txt";
        string currentFileName =
            $"C:\\Users\\jeffe\\Documents\\.Projetos_C#\\BankApplicationCA2\\ConsoleBanckApp\\{accountNumber.PadLeft(8, '0')}-current.txt";

        try
        {
            // Check if the files already exist
            if (!File.Exists(savingsFileName))
            {
                File.Create(savingsFileName).Dispose();
            }
            if (!File.Exists(currentFileName))
            {
                File.Create(currentFileName).Dispose();
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error creating account files: {ex.Message}");
        }

        return (currentFileName, savingsFileName);
    }
    private static void DeleteCustomerAccount()
    {
        try
        {
            Console.WriteLine("Enter Account Number: ");
            string accountNumber = Console.ReadLine();

            Customer customer = customers.Find(c => c.AccountNumber == accountNumber);

            if (customer == null)
            {
                Console.WriteLine("Customer not found.");
                return;
            }

            if (customer.CurrentAccount.Balance != 0 || customer.SavingsAccount.Balance != 0)
            {
                Console.WriteLine("Customer cannot be deleted. Account balance must be zero.");
                return;
            }

            customers.Remove(customer);

            Console.WriteLine("Customer account deleted successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static void CreateTransaction()
    {
        Console.WriteLine("Enter Account Number: ");
        string accountNumber = Console.ReadLine();

        Customer customer = customers.Find(c => c.AccountNumber == accountNumber);

        if (customer == null)
        {
            Console.WriteLine("Customer not found.");
            return;
        }

        Console.WriteLine("1. Lodge Money");
        Console.WriteLine("2. Withdraw Money");

        int choice = GetIntInput("Enter your choice: ");

        switch (choice)
        {
            case 1:
                DepositMoney(customer);
                break;
            case 2:
                WithdrawMoney(customer);
                break;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
    }

    private static void DepositMoney(Customer customer)
    {
        Console.WriteLine("1. Current Account");
        Console.WriteLine("2. Savings Account");

        int choice = GetIntInput("Select Account Type: ");

        Account selectedAccount;

        switch (choice)
        {
            case 1:
                selectedAccount = customer.CurrentAccount;
                break;
            case 2:
                selectedAccount = customer.SavingsAccount;
                break;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                return;
        }

        Console.WriteLine("Enter Amount: ");
        decimal amount = GetDecimalInput("Enter Amount: ");

        selectedAccount.Lodge(amount, customer);

        Console.WriteLine($"${amount} deposited successfully!");
        Console.WriteLine($"{selectedAccount.GetType().Name} Balance: {selectedAccount.Balance}");
    }

    public static void WithdrawMoney(Customer customer)
    {
        Console.WriteLine("1. Current Account");
        Console.WriteLine("2. Savings Account");

        int choice = GetIntInput("Select Account Type: ");

        Account selectedAccount;

        switch (choice)
        {
            case 1:
                selectedAccount = customer.CurrentAccount;
                break;
            case 2:
                selectedAccount = customer.SavingsAccount;
                break;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                return;
        }

        Console.WriteLine("Enter Amount: ");
        decimal amount = GetDecimalInput("Enter Amount: ");

        try
        {
            selectedAccount.Withdraw(amount, customer);
            Console.WriteLine($"${amount} withdrawn successfully!");
            Console.WriteLine($"{selectedAccount.GetType().Name} Balance: {selectedAccount.Balance}");
        }
        catch (InsufficientFundsException)
        {
            Console.WriteLine("Insufficient funds. Please try again.");
        }
    }

    private static void ListAllCustomersWithBalances()
    {
        Console.WriteLine("---------------------------------------------------------------------------------------------");
        Console.WriteLine("|Account Number | First Name | Last Name | Current Account Balance | Savings Account Balance|");
        Console.WriteLine("---------------------------------------------------------------------------------------------");

        foreach (Customer customer in customers)
        {
            Console.WriteLine(
                $"|{customer.AccountNumber,-15}| " +
                $"{customer.FirstName,-11}| " +
                $"{customer.LastName,-10}| " +
                $"{CenterAlign(customer.CurrentAccount.Balance.ToString(), 24)}" +
                $"{CenterAlign(customer.SavingsAccount.Balance.ToString(), 48)}"
            );
        }
        Console.WriteLine("---------------------------------------------------------------------------------------------");
    }
    private static string CenterAlign(string text, int width)
    {
        return string.Format("{0," + ((width + text.Length) / 2).ToString() + "}", text);
    }

    private static void ListAllCustomerAccountNumbers()
    {
        Console.WriteLine("Account Number");

        foreach (Customer customer in customers)
        {
            Console.WriteLine(customer.AccountNumber);
        }
    }

    private static int GetIntInput(string prompt)
    {
        while (true)
        {
            try
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                return int.Parse(input);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter a valid integer.");
            }
        }
    }

    private static decimal GetDecimalInput(string prompt)
    {
        while (true)
        {
            try
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                return decimal.Parse(input);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter a valid decimal.");
            }
        }
    }

    public static string GeneratePin(string accountNumber)
    {
        // Extract the last four digits of the account number (assuming PIN is 4 digits)
        string pin = accountNumber.Substring(accountNumber.Length - 5).Replace("-", "");

        pin = pin.PadLeft(4, '0');

        return pin;
    }
}