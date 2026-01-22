using CustomerPlatform.Domain.Entities;
using CustomerPlatform.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace CustomerPlatform.Tests.Entities;

public class ClientePessoaFisicaTests
{
    [Fact]
    public void Construtor_DeveCriarCliente_QuandoDadosValidos()
    {
        // Arrange & Act
        var cliente = new ClientePessoaFisica(
            "João Silva",
            "52998224725",
            DateTime.UtcNow.AddYears(-30),
            "joao@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103"));

        // Assert
        cliente.Should().NotBeNull();
        cliente.Id.Should().NotBeEmpty();
        cliente.Nome.Should().Be("João Silva");
        cliente.CPF.Should().Be("52998224725");
        cliente.Email.Should().Be("joao@email.com");
        cliente.Telefone.Should().Be("2199887766");
        cliente.DataCriacao.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Construtor_DeveLancarExcecao_QuandoCPFInvalido()
    {
        // Arrange & Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new ClientePessoaFisica(
            "João Silva",
            "12345678901", // CPF inválido
            DateTime.UtcNow.AddYears(-30),
            "joao@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103")));

        exception.Message.Should().Be("CPF inválido");
    }

    [Fact]
    public void GetDocumento_DeveRetornarCPF()
    {
        // Arrange
        var cliente = new ClientePessoaFisica(
            "João Silva",
            "52998224725",
            DateTime.UtcNow.AddYears(-30),
            "joao@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103"));

        // Act
        var documento = cliente.GetDocumento();

        // Assert
        documento.Should().Be("52998224725");
    }

    [Fact]
    public void GetNome_DeveRetornarNome()
    {
        // Arrange
        var cliente = new ClientePessoaFisica(
            "João Silva",
            "52998224725",
            DateTime.UtcNow.AddYears(-30),
            "joao@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103"));

        // Act
        var nome = cliente.GetNome();

        // Assert
        nome.Should().Be("João Silva");
    }

    [Fact]
    public void ValidarDocumento_DeveRetornarTrue_QuandoCPFValido()
    {
        // Arrange
        var cliente = new ClientePessoaFisica(
            "João Silva",
            "52998224725",
            DateTime.UtcNow.AddYears(-30),
            "joao@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103"));

        // Act
        var resultado = cliente.ValidarDocumento();

        // Assert
        resultado.Should().BeTrue();
    }

    [Fact]
    public void Atualizar_DeveAtualizarDados_QuandoDadosValidos()
    {
        // Arrange
        var cliente = new ClientePessoaFisica(
            "João Silva",
            "52998224725",
            DateTime.UtcNow.AddYears(-30),
            "joao@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103"));

        var novoEndereco = new Endereco("Av Nova", "100", "22000000", "São Paulo", "SP", "Apto 101");

        // Act
        cliente.Atualizar(
            "João Silva Atualizado",
            "52998224725",
            DateTime.UtcNow.AddYears(-25),
            "joao.novo@email.com",
            "2199887767",
            novoEndereco);

        // Assert
        cliente.Nome.Should().Be("João Silva Atualizado");
        cliente.Email.Should().Be("joao.novo@email.com");
        cliente.Telefone.Should().Be("2199887767");
        cliente.DataAtualizacao.Should().NotBeNull();
        cliente.DataAtualizacao.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Atualizar_DeveLancarExcecao_QuandoCPFInvalido()
    {
        // Arrange
        var cliente = new ClientePessoaFisica(
            "João Silva",
            "52998224725",
            DateTime.UtcNow.AddYears(-30),
            "joao@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103"));

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => cliente.Atualizar(
            "João Silva",
            "12345678901", // CPF inválido
            DateTime.UtcNow.AddYears(-30),
            "joao@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103")));

        exception.Message.Should().Be("CPF inválido");
    }
}
