using CustomerPlatform.Application.Factories;
using CustomerPlatform.Domain.Entities;
using CustomerPlatform.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace CustomerPlatform.Tests.Factories;

public class CustomerEventFactoryTests
{
    [Fact]
    public void CreateCustomerCreatedEvent_DeveCriarEventoComTipoClienteCriado()
    {
        // Arrange
        var customer = new ClientePessoaFisica(
            "João Silva",
            "52998224725",
            DateTime.UtcNow.AddYears(-30),
            "joao@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103"));

        // Act
        var result = CustomerEventFactory.CreateCustomerCreatedEvent(customer);

        // Assert
        result.Should().NotBeNull();
        result.EventType.Should().Be("ClienteCriado");
        result.EventId.Should().NotBeEmpty();
        result.Timestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        result.Data.Should().NotBeNull();
        result.Data.ClienteId.Should().Be(customer.Id);
        result.Data.Email.Should().Be(customer.Email);
        result.Data.Telefone.Should().Be(customer.Telefone);
        result.Data.TipoCliente.Should().Be("PF");
        result.Data.Nome.Should().Be(customer.Nome);
        result.Data.Documento.Should().Be(customer.CPF);
    }

    [Fact]
    public void CreateCustomerUpdatedEvent_DeveCriarEventoComTipoClienteAtualizado()
    {
        // Arrange
        var customer = new ClientePessoaFisica(
            "João Silva",
            "52998224725",
            DateTime.UtcNow.AddYears(-30),
            "joao@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103"));

        // Act
        var result = CustomerEventFactory.CreateCustomerUpdatedEvent(customer);

        // Assert
        result.Should().NotBeNull();
        result.EventType.Should().Be("ClienteAtualizado");
        result.EventId.Should().NotBeEmpty();
        result.Timestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        result.Data.Should().NotBeNull();
        result.Data.ClienteId.Should().Be(customer.Id);
    }

    [Fact]
    public void CreateCustomerCreatedEvent_DeveCriarEventoParaPessoaJuridica()
    {
        // Arrange
        var customer = new ClientePessoaJuridica(
            "Empresa LTDA",
            "Empresa",
            "67964706000144",
            "empresa@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103"));

        // Act
        var result = CustomerEventFactory.CreateCustomerCreatedEvent(customer);

        // Assert
        result.Should().NotBeNull();
        result.Data.TipoCliente.Should().Be("PJ");
        result.Data.Nome.Should().Be(customer.NomeFantasia);
        result.Data.Documento.Should().Be(customer.CNPJ);
    }

    [Fact]
    public void CreateCustomerUpdatedEvent_DeveCriarEventoParaPessoaJuridica()
    {
        // Arrange
        var customer = new ClientePessoaJuridica(
            "Empresa LTDA",
            "Empresa",
            "67964706000144",
            "empresa@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103"));

        // Act
        var result = CustomerEventFactory.CreateCustomerUpdatedEvent(customer);

        // Assert
        result.Should().NotBeNull();
        result.EventType.Should().Be("ClienteAtualizado");
        result.Data.TipoCliente.Should().Be("PJ");
    }
}
