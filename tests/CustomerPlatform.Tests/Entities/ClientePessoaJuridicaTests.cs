using CustomerPlatform.Domain.Entities;
using CustomerPlatform.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace CustomerPlatform.Tests.Entities;

public class ClientePessoaJuridicaTests
{
    [Fact]
    public void Construtor_DeveCriarCliente_QuandoDadosValidos()
    {
        // Arrange & Act
        var cliente = new ClientePessoaJuridica(
            "Empresa LTDA",
            "Empresa",
            "67964706000144",
            "empresa@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103"));

        // Assert
        cliente.Should().NotBeNull();
        cliente.Id.Should().NotBeEmpty();
        cliente.RazaoSocial.Should().Be("Empresa LTDA");
        cliente.NomeFantasia.Should().Be("Empresa");
        cliente.CNPJ.Should().Be("67964706000144");
        cliente.Email.Should().Be("empresa@email.com");
        cliente.Telefone.Should().Be("2199887766");
        cliente.DataCriacao.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Construtor_DeveLancarExcecao_QuandoCNPJInvalido()
    {
        // Arrange & Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new ClientePessoaJuridica(
            "Empresa LTDA",
            "Empresa",
            "12345678901234", // CNPJ inválido
            "empresa@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103")));

        exception.Message.Should().Be("CNPJ inválido");
    }

    [Fact]
    public void GetDocumento_DeveRetornarCNPJ()
    {
        // Arrange
        var cliente = new ClientePessoaJuridica(
            "Empresa LTDA",
            "Empresa",
            "67964706000144",
            "empresa@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103"));

        // Act
        var documento = cliente.GetDocumento();

        // Assert
        documento.Should().Be("67964706000144");
    }

    [Fact]
    public void GetNome_DeveRetornarRazaoSocial()
    {
        // Arrange
        var cliente = new ClientePessoaJuridica(
            "Empresa LTDA",
            "Empresa",
            "67964706000144",
            "empresa@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103"));

        // Act
        var nome = cliente.GetNome();

        // Assert
        nome.Should().Be("Empresa LTDA");
    }

    [Fact]
    public void ValidarDocumento_DeveRetornarTrue_QuandoCNPJValido()
    {
        // Arrange
        var cliente = new ClientePessoaJuridica(
            "Empresa LTDA",
            "Empresa",
            "67964706000144",
            "empresa@email.com",
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
        var cliente = new ClientePessoaJuridica(
            "Empresa LTDA",
            "Empresa",
            "67964706000144",
            "empresa@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103"));

        var novoEndereco = new Endereco("Av Nova", "100", "22000000", "São Paulo", "SP", "Apto 101");

        // Act
        cliente.Atualizar(
            "Empresa LTDA Atualizada",
            "Empresa Nova",
            "67964706000144",
            "empresa.nova@email.com",
            "2199887767",
            novoEndereco);

        // Assert
        cliente.RazaoSocial.Should().Be("Empresa LTDA Atualizada");
        cliente.NomeFantasia.Should().Be("Empresa Nova");
        cliente.Email.Should().Be("empresa.nova@email.com");
        cliente.Telefone.Should().Be("2199887767");
        cliente.DataAtualizacao.Should().NotBeNull();
        cliente.DataAtualizacao.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Atualizar_DeveLancarExcecao_QuandoCNPJInvalido()
    {
        // Arrange
        var cliente = new ClientePessoaJuridica(
            "Empresa LTDA",
            "Empresa",
            "67964706000144",
            "empresa@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103"));

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => cliente.Atualizar(
            "Empresa LTDA",
            "Empresa",
            "12345678901234", // CNPJ inválido
            "empresa@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103")));

        exception.Message.Should().Be("CNPJ inválido");
    }
}
