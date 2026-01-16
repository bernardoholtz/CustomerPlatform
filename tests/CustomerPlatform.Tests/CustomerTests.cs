using CustomerPlatform.Domain.Entities;

namespace CustomerPlatform.Tests;

public class CustomerTests
{
    [Fact]
    public void ClientePessoaFisica_DeveRetornarCPFComoDocumento()
    {
        // Arrange
        //var cliente = new ClientePessoaFisica
        //{
        //    Id = Guid.NewGuid(),
        //    Nome = "João Silva",
        //    CPF = "12345678900",
        //    Email = "joao@email.com"
        //};

        // Act
        //var documento = cliente.GetDocumento();

        // Assert
       // Assert.Equal("12345678900", documento);
    }

    [Fact]
    public void ClientePessoaJuridica_DeveRetornarCNPJComoDocumento()
    {
        // Arrange
        //var cliente = new ClientePessoaJuridica
        //{
        //    Id = Guid.NewGuid(),
        //    RazaoSocial = "Empresa LTDA",
        //    CNPJ = "12345678000190",
        //    Email = "contato@empresa.com"
        //};

        //// Act
        //var documento = cliente.GetDocumento();

        //// Assert
        //Assert.Equal("12345678000190", documento);
    }
}

