using CustomerPlatform.Application.Commands.CreateCustomer;
using CustomerPlatform.Application.Factories;
using CustomerPlatform.Domain.Entities;
using CustomerPlatform.Domain.Enums;
using CustomerPlatform.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace CustomerPlatform.Tests.Factories;

public class CustomerFactoryTests
{
    [Fact]
    public void CriarInstancia_DeveCriarClientePessoaFisica_QuandoTipoClienteEhPessoaFisica()
    {
        // Arrange
        var command = new CreateCustomerCommand
        {
            TipoCliente = TipoCliente.PessoaFisica,
            Nome = "João Silva",
            CPF = "52998224725",
            DataNascimento = DateTime.UtcNow.AddYears(-30),
            Email = "joao@email.com",
            Telefone = "2199887766",
            Endereco = new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103")
        };

        // Act
        var result = CustomerFactory.CriarInstancia(command);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ClientePessoaFisica>();
        var pf = result as ClientePessoaFisica;
        pf!.Nome.Should().Be(command.Nome);
        pf.CPF.Should().Be(command.CPF);
        pf.Email.Should().Be(command.Email);
        pf.Telefone.Should().Be(command.Telefone);
    }

    [Fact]
    public void CriarInstancia_DeveCriarClientePessoaJuridica_QuandoTipoClienteEhPessoaJuridica()
    {
        // Arrange
        var command = new CreateCustomerCommand
        {
            TipoCliente = TipoCliente.PessoaJuridica,
            RazaoSocial = "Empresa LTDA",
            NomeFantasia = "Empresa",
            CNPJ = "67964706000144",
            Email = "empresa@email.com",
            Telefone = "2199887766",
            Endereco = new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103")
        };

        // Act
        var result = CustomerFactory.CriarInstancia(command);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ClientePessoaJuridica>();
        var pj = result as ClientePessoaJuridica;
        pj!.RazaoSocial.Should().Be(command.RazaoSocial);
        pj.NomeFantasia.Should().Be(command.NomeFantasia);
        pj.CNPJ.Should().Be(command.CNPJ);
        pj.Email.Should().Be(command.Email);
        pj.Telefone.Should().Be(command.Telefone);
    }

    [Fact]
    public void CriarInstancia_DeveLancarExcecao_QuandoTipoClienteInvalido()
    {
        // Arrange
        var command = new CreateCustomerCommand
        {
            TipoCliente = (TipoCliente)999, // Tipo inválido
            Email = "teste@email.com",
            Telefone = "2199887766",
            Endereco = new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103")
        };

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => CustomerFactory.CriarInstancia(command));
        exception.Message.Should().Be("Tipo de cliente inválido");
    }

    [Fact]
    public void CriarInstancia_DeveCriarEnderecoCorretamente()
    {
        // Arrange
        var command = new CreateCustomerCommand
        {
            TipoCliente = TipoCliente.PessoaFisica,
            Nome = "João Silva",
            CPF = "52998224725",
            DataNascimento = DateTime.UtcNow.AddYears(-30),
            Email = "joao@email.com",
            Telefone = "2199887766",
            Endereco = new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103")
        };

        // Act
        var result = CustomerFactory.CriarInstancia(command);

        // Assert
        result.Endereco.Should().NotBeNull();
        result.Endereco.Logradouro.Should().Be(command.Endereco.Logradouro);
        result.Endereco.Numero.Should().Be(command.Endereco.Numero);
        result.Endereco.CEP.Should().Be(command.Endereco.CEP);
        result.Endereco.Cidade.Should().Be(command.Endereco.Cidade);
        result.Endereco.Estado.Should().Be(command.Endereco.Estado);
        result.Endereco.Complemento.Should().Be(command.Endereco.Complemento);
    }
}
