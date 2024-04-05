// Name: Jefferson Duarte
// ID: 71205

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.ExceptionServices;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class BankApplication
{
    private static readonly string EmployeePin = "A1234";

    // List to store customer data
    private static List<Customer> customers = new List<Customer>();

    // File path to store customer data
    private static readonly string customersFilePath = $"C:\\Users\\jeffe\\Documents\\.Projetos_C#\\Projeto_Particionado\\ConsoleBankApplication\\customers.txt";

    public static void Main(string[] args)
    {
        // Load customers data when the program starts
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

    // Method to create a new customer account
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

    // Method to save customers data to a file
    public static void SaveCustomerData(Customer customer)
    {
        // Get the current directory where the program is executing
        string parentDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName.ToString();
        string path = parentDirectory.Remove(parentDirectory.LastIndexOf("\\")).ToString();
        string newPath = path.Remove(path.LastIndexOf("\\")).ToString(); ;
        path = newPath.Remove(newPath.LastIndexOf("\\")).ToString();

        // Combine the current directory with the file name
        string fileName = "customers.txt";
        string filePath = Path.Combine(path, fileName);

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
            // Open a StreamWriter to write customer data to the file
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

    // Method to load customers data from a file
    private static void LoadCustomers()
    {
        if (File.Exists(customersFilePath))
        {
            try
            {
                // Open a StreamReader to read customer data from the file
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

                        // Create a new customer object from the data read from the file
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
        // Generating a unique account number based on customer's first and last names
        string initials = $"{firstName.ToUpper()[0]}{lastName.ToUpper()[0]}";
        int fullNameLength = firstName.Length + lastName.Length;

        string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        int firstInitialPosition = alphabet.IndexOf(initials[0]) + 1;
        int secondInitialPosition = alphabet.IndexOf(initials[1]) + 1;

        // Format and return the generated account number
        return $"{initials}-{fullNameLength}-{firstInitialPosition}-{secondInitialPosition}";
    }
    public static (string currentFileName, string savingsFileName) GenerateAccountFiles(string accountNumber)
    {

        //Get the current directory where the program is executing
        string parentDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName.ToString();
        string path = parentDirectory.Remove(parentDirectory.LastIndexOf("\\")).ToString();
        string newPath = path.Remove(path.LastIndexOf("\\")).ToString(); ;
        path = newPath.Remove(newPath.LastIndexOf("\\")).ToString();


        //Combine the current directory with the file name
        string savingsFileName = $"{accountNumber.PadLeft(8, '0')}-savings.txt";
        string savingsFilePath = Path.Combine(path, savingsFileName);

        string currentFileName = $"{accountNumber.PadLeft(8, '0')}-current.txt";
        string currentFilePath = Path.Combine(path, currentFileName);

        try
        {
            // Check if the files already exist
            if (!File.Exists(savingsFilePath))
            {
                File.Create(savingsFilePath).Dispose();
            }
            if (!File.Exists(currentFilePath))
            {
                File.Create(currentFilePath).Dispose();
            }
        }
        catch (IOException ex)
        {
            // Handle IOException if file creation fails
            Console.WriteLine($"Error creating account files: {ex.Message}");
        }

        // Return the generated file names
        return (currentFilePath, savingsFilePath);
    }
    private static void DeleteCustomerAccount()
    {
        try
        {
            // Prompt user to enter the account number of the customer to be deleted
            Console.WriteLine("Enter Account Number: ");
            string accountNumber = Console.ReadLine();

            // Find the customer with the given account number
            Customer customer = customers.Find(c => c.AccountNumber == accountNumber);

            // If customer not found, inform the user and return
            if (customer == null)
            {
                Console.WriteLine("Customer not found.");
                return;
            }

            // Check if the customer's account balances are zero before deletion
            if (customer.CurrentAccount.Balance != 0 || customer.SavingsAccount.Balance != 0)
            {
                Console.WriteLine("Customer cannot be deleted. Account balance must be zero.");
                return;
            }

            // Remove the customer from the list
            customers.Remove(customer);

            // Inform the user that the customer account has been deleted successfully
            Console.WriteLine("Customer account deleted successfully!");
        }
        catch (Exception ex)
        {
            // Handle any exceptions that occur during the process
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static void CreateTransaction()
    {
        try
        {
            // Prompt user to enter the account number for the transaction
            Console.WriteLine("Enter Account Number: ");
            string accountNumber = Console.ReadLine();

            // Find the customer associated with the entered account number
            Customer customer = customers.Find(c => c.AccountNumber == accountNumber);

            // If customer not found, inform the user and return
            if (customer == null)
            {
                Console.WriteLine("Customer not found.");
                return;
            }

            // Present options for the type of transaction: lodge or withdraw
            Console.WriteLine("1. Lodge Money");
            Console.WriteLine("2. Withdraw Money");

            // Get user's choice for the transaction type
            int choice = GetIntInput("Enter your choice: ");

            switch (choice)
            {
                case 1:
                    // Perform a deposit transaction
                    DepositMoney(customer);
                    break;
                case 2:
                    // Perform a withdrawal transaction
                    WithdrawMoney(customer);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions that occur during the process
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static void DepositMoney(Customer customer)
    {
        try
        {
            // Present options for the account type: current or savings
            Console.WriteLine("1. Current Account");
            Console.WriteLine("2. Savings Account");

            // Get user's choice for the account type
            int choice = GetIntInput("Select Account Type: ");

            Account selectedAccount;

            switch (choice)
            {
                case 1:
                    // Deposit into the current account
                    selectedAccount = customer.CurrentAccount;
                    break;
                case 2:
                    // Deposit into the savings account
                    selectedAccount = customer.SavingsAccount;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    return;
            }

            // Prompt user to enter the deposit amount
            decimal amount = GetDecimalInput("Enter Amount: ");

            // Perform the deposit operation
            selectedAccount.Lodge(amount, customer);

            // Inform the user about the success of the deposit operation
            Console.WriteLine($"${amount} deposited successfully!");
            Console.WriteLine($"{selectedAccount.GetType().Name} Balance: {selectedAccount.Balance}");
        }
        catch (Exception ex)
        {
            // Handle any exceptions that occur during the process
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public static void WithdrawMoney(Customer customer)
    {
        // Present options for the account type: current or savings
        Console.WriteLine("1. Current Account");
        Console.WriteLine("2. Savings Account");

        // Get user's choice for the account type
        int choice = GetIntInput("Select Account Type: ");

        Account selectedAccount;

        switch (choice)
        {
            case 1:
                // Withdraw from the current account
                selectedAccount = customer.CurrentAccount;
                break;
            case 2:
                // Withdraw from the savings account
                selectedAccount = customer.SavingsAccount;
                break;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                return;
        }

        // Prompt user to enter the withdrawal amount
        decimal amount = GetDecimalInput("Enter Amount: ");

        try
        {
            // Perform the withdrawal operation
            selectedAccount.Withdraw(amount, customer);

            // Inform the user about the success of the withdrawal operation
            Console.WriteLine($"${amount} withdrawn successfully!");
            Console.WriteLine($"{selectedAccount.GetType().Name} Balance: {selectedAccount.Balance}");
        }
        catch (InsufficientFundsException)
        {
            // Handle any exceptions that occur during the process
            Console.WriteLine("Insufficient funds. Please try again.");
        }
    }

    private static void ListAllCustomersWithBalances()
    {
        try
        {
            // Display the header for the customer details table
            Console.WriteLine("---------------------------------------------------------------------------------------------");
            Console.WriteLine("|Account Number | First Name | Last Name | Current Account Balance | Savings Account Balance|");
            Console.WriteLine("---------------------------------------------------------------------------------------------");

            // Iterate through each customer and display their details
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

            // Display the footer for the customer details table
            Console.WriteLine("---------------------------------------------------------------------------------------------");
        }
        catch (Exception ex)
        {
            // Handle any exceptions that occur during the process
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
    private static string CenterAlign(string text, int width)
    {
        return string.Format("{0," + ((width + text.Length) / 2).ToString() + "}", text);
    }

    private static void ListAllCustomerAccountNumbers()
    {
        try
        {
            // Display the account numbers of all customers
            Console.WriteLine("Account Number");

            foreach (Customer customer in customers)
            {
                Console.WriteLine(customer.AccountNumber);
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions that occur during the process
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static int GetIntInput(string prompt)
    {
        while (true)
        {
            try
            {
                // Prompt user for input and parse it into an integer
                Console.Write(prompt);
                string input = Console.ReadLine();
                return int.Parse(input);
            }
            catch (FormatException)
            {
                // Handle invalid input (non-integer)
                Console.WriteLine("Invalid input. Please enter a valid integer.");
            }
            catch (Exception ex)
            {
                // Handle any other exceptions that might occur
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }

    private static decimal GetDecimalInput(string prompt)
    {
        while (true)
        {
            try
            {
                // Prompt user for input and parse it into a decimal
                Console.Write(prompt);
                string input = Console.ReadLine();
                return decimal.Parse(input);
            }
            catch (FormatException)
            {
                // Handle invalid input (non-decimal)
                Console.WriteLine("Invalid input. Please enter a valid decimal.");
            }
            catch (Exception ex)
            {
                // Handle any other exceptions that might occur
                Console.WriteLine($"An error occurred: {ex.Message}");
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