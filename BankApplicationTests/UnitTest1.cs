// Name: Jefferson Duarte
// ID: 71205

using NUnit.Framework;
using System;
using System.IO;

public class BankApplicationTests
{

    [Test]
    public void CreateCustomerAccount_ValidInput_AccountCreatedSuccessfully()
    {
        // Arrange
        string firstName = "John";
        string lastName = "Doe";
        string email = "john.doe@example.com";

        // Simulating user input
        StringReader stringReader = new StringReader($"{firstName}\n{lastName}\n{email}\n");
        Console.SetIn(stringReader);

        // Capturing console output
        StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        // Act

        BankApplication.CreateCustomerAccount();

        // Assert
        string output = stringWriter.ToString();
        Assert.That(output.Contains("Customer account created successfully!"));
        Assert.That(output.Contains("Account Number:")); // Verifies if the account number was displayed
        Assert.That(output.Contains("PIN:")); // Verifies if the PIN was displayed
    }

    [Test]
    public void GenerateAccountNumber_ValidNames_ReturnsCorrectAccountNumber()
    {
        // Arrange
        string firstName = "John";
        string lastName = "Smith";
        string expectedAccountNumber = "JS-9-10-19"; // This is just an example. The actual value may vary depending on the provided names.

        // Act
        string actualAccountNumber = BankApplication.GenerateAccountNumber(firstName, lastName);

        // Assert
        Assert.That(actualAccountNumber, Is.EqualTo(expectedAccountNumber));
    }

    [Test]
    public void GeneratePin_ValidAccountNumber_ReturnsCorrectPin()
    {
        // Arrange
        string accountNumber = "JS-9-10-19"; // Sample account number
        string expectedPin = "1019"; // The expected PIN based on the last four digits of the account number

        // Act
        string actualPin = BankApplication.GeneratePin(accountNumber);

        // Assert
        Assert.That(actualPin, Is.EqualTo(expectedPin));
    }
}
