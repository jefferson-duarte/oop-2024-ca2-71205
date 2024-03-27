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

        // Simulando entrada do usu�rio
        StringReader stringReader = new StringReader($"{firstName}\n{lastName}\n{email}\n");
        Console.SetIn(stringReader);

        // Capturando a sa�da do console
        StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        // Act

        BankApplication.CreateCustomerAccount();

        // Assert
        string output = stringWriter.ToString();
        Assert.That(output.Contains("Customer account created successfully!"));
        Assert.That(output.Contains("Account Number:")); // Verifica se o n�mero da conta foi exibido
        Assert.That(output.Contains("PIN:")); // Verifica se o PIN foi exibido
        // Aqui voc� pode adicionar mais asser��es conforme necess�rio
    }

    [Test]
    public void GenerateAccountNumber_ValidNames_ReturnsCorrectAccountNumber()
    {
        // Arrange
        string firstName = "John";
        string lastName = "Smith";
        string expectedAccountNumber = "JS-9-10-19"; // Isso � apenas um exemplo. O valor real pode ser diferente dependendo dos nomes fornecidos.

        // Act
        string actualAccountNumber = BankApplication.GenerateAccountNumber(firstName, lastName);

        // Assert
        Assert.That(actualAccountNumber, Is.EqualTo(expectedAccountNumber));
    }

    [Test]
    public void GeneratePin_ValidAccountNumber_ReturnsCorrectPin()
    {
        // Arrange
        string accountNumber = "JS-9-10-19"; // N�mero de conta de exemplo
        string expectedPin = "1019"; // O PIN esperado com base nos �ltimos quatro d�gitos do n�mero da conta

        // Act
        string actualPin = BankApplication.GeneratePin(accountNumber);

        // Assert
        Assert.That(actualPin, Is.EqualTo(expectedPin));
    }
}
