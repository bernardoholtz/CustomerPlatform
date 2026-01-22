using CustomerPlatform.Application.Commands.CreateCustomer;
using CustomerPlatform.Application.Validators;
using CustomerPlatform.Domain.Entities;
using CustomerPlatform.Domain.ValueObjects;

namespace CustomerPlatform.Tests;

public class CustomerTests
{
    [Fact]
    public void ClientePessoaFisica_DeveRetornarCPFComoDocumento()
    {
        // Arrange
        var cliente = new ClientePessoaFisica(
            "João Silva",
            "49303076001",
            DateTime.Now,
            "joao@email.com",
            "2199887865",
             new Endereco(
                 "Av 7 de Setembro",
                 "4200",
                 "22076090",
                 "Rio de Janeiro",
                 "RJ",
                 "Sala 103"
                 )
            );
     
        // Act
        var documento = cliente.GetDocumento();

        // Assert
        Assert.Equal("49303076001", documento);
    }

    [Fact]
    public void ClientePessoaJuridica_DeveRetornarCNPJComoDocumento()
    {
        // Arrange
        var cliente = new ClientePessoaJuridica(
            "Udemy LTDA",
            "Cursos Udemy",
            "67964706000144",
            "joao@email.com",
            "2199887865",
             new Endereco(
                 "Av 7 de Setembro",
                 "4200",
                 "22076090",
                 "Rio de Janeiro",
                 "RJ",
                 "Sala 103"
                 )
            );

        // Act
        var documento = cliente.GetDocumento();

        // Assert
        Assert.Equal("67964706000144", documento);
    }

    [Fact]
    public void ClientePessoaFisica_DeveRetornarCPFInvalido()
    {
        // Arrange
        var exception = Assert.Throws<ArgumentException>(() => 
        new ClientePessoaFisica(
            "João Silva",
            "12343627876",
            DateTime.Now,
            "joao@email.com",
            "2199887865",
             new Endereco(
                 "Av 7 de Setembro",
                 "4200",
                 "22076090",
                 "Rio de Janeiro",
                 "RJ",
                 "Sala 103"
                 )
            )
        );

        // Assert
        Assert.Equal("CPF inválido", exception.Message);
    }

    [Fact]
    public void ClientePessoaJuridica_DeveRetornarCNPJInvalido()
    {
        // Arrange
        var exception = Assert.Throws<ArgumentException>(() =>
        new ClientePessoaJuridica(
            "Udemy LTDA",
            "Cursos Udemy",
            "12232786546787",
            "joao@email.com",
            "2199887865",
             new Endereco(
                 "Av 7 de Setembro",
                 "4200",
                 "22076090",
                 "Rio de Janeiro",
                 "RJ",
                 "Sala 103"
                 )
            )
        );

        // Assert
        Assert.Equal("CNPJ inválido", exception.Message);
    }

    [Fact]
    public void Cliente_DeveRetornarEmailInvalido()
    {
        // Arrange
        var command = new CreateCustomerCommand
        {
            Nome = "João Silva",
            CPF = "52998224725", 
            DataNascimento = DateTime.UtcNow.AddYears(-30),
            Email = "email-invalido", // ❌ inválido
            Telefone = "2199887766",
            Endereco = new Endereco(
                 "Av 7 de Setembro",
                 "4200",
                 "22076090",
                 "Rio de Janeiro",
                 "RJ",
                 "Sala 103"
                 )
        };

        var validator = new CreateCustomerCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e =>
            e.PropertyName == "Email" &&
            e.ErrorMessage.Contains("email inválido")
        );
    }

    //TestContainers
}

