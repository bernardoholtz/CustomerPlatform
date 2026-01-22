using CustomerPlatform.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace CustomerPlatform.Tests.ValueObjects;

public class EnderecoTests
{
    [Fact]
    public void Construtor_DeveCriarEndereco_QuandoDadosValidos()
    {
        // Arrange & Act
        var endereco = new Endereco(
            "Av 7 de Setembro",
            "4200",
            "22076090",
            "Rio de Janeiro",
            "RJ",
            "Sala 103");

        // Assert
        endereco.Should().NotBeNull();
        endereco.Logradouro.Should().Be("Av 7 de Setembro");
        endereco.Numero.Should().Be("4200");
        endereco.CEP.Should().Be("22076090");
        endereco.Cidade.Should().Be("Rio de Janeiro");
        endereco.Estado.Should().Be("RJ");
        endereco.Complemento.Should().Be("Sala 103");
    }

    [Fact]
    public void Construtor_DeveCriarEndereco_QuandoComplementoNulo()
    {
        // Arrange & Act
        var endereco = new Endereco(
            "Av 7 de Setembro",
            "4200",
            "22076090",
            "Rio de Janeiro",
            "RJ");

        // Assert
        endereco.Should().NotBeNull();
        endereco.Complemento.Should().BeNull();
    }

    [Fact]
    public void Construtor_DeveCriarEndereco_QuandoComplementoVazio()
    {
        // Arrange & Act
        var endereco = new Endereco(
            "Av 7 de Setembro",
            "4200",
            "22076090",
            "Rio de Janeiro",
            "RJ",
            "");

        // Assert
        endereco.Should().NotBeNull();
        endereco.Complemento.Should().Be("");
    }
}
