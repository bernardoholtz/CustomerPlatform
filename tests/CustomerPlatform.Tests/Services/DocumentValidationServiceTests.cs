using CustomerPlatform.Application.Services;
using CustomerPlatform.Domain.Entities;
using CustomerPlatform.Domain.Interfaces;
using CustomerPlatform.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CustomerPlatform.Tests.Services;

public class DocumentValidationServiceTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly ILogger<DocumentValidationService> _logger;
    private readonly DocumentValidationService _service;

    public DocumentValidationServiceTests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _logger = Mock.Of<ILogger<DocumentValidationService>>();
        _service = new DocumentValidationService(_customerRepositoryMock.Object, _logger);
    }

    [Fact]
    public async Task ValidateCpfAsync_DevePassar_QuandoCPFNaoExiste()
    {
        // Arrange
        var cpf = "52998224725";
        _customerRepositoryMock.Setup(x => x.BuscaCpfExistente(cpf, Guid.Empty))
            .ReturnsAsync((ClientePessoaFisica?)null);

        // Act
        await _service.ValidateCpfAsync(cpf);

        // Assert
        _customerRepositoryMock.Verify(x => x.BuscaCpfExistente(cpf, Guid.Empty), Times.Once);
    }

    [Fact]
    public async Task ValidateCpfAsync_DeveLancarExcecao_QuandoCPFJaExiste()
    {
        // Arrange
        var cpf = "52998224725";
        var clienteExistente = new ClientePessoaFisica(
            "João Silva",
            cpf,
            DateTime.UtcNow.AddYears(-30),
            "joao@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103"));

        _customerRepositoryMock.Setup(x => x.BuscaCpfExistente(cpf, Guid.Empty))
            .ReturnsAsync(clienteExistente);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.ValidateCpfAsync(cpf));
        exception.Message.Should().Be("Já existe um cliente cadastrado com este CPF.");
    }

    [Fact]
    public async Task ValidateCpfAsync_DeveLancarExcecao_QuandoCPFJaExisteEmOutroCliente()
    {
        // Arrange
        var cpf = "52998224725";
        var excludeCustomerId = Guid.NewGuid();
        var clienteExistente = new ClientePessoaFisica(
            "João Silva",
            cpf,
            DateTime.UtcNow.AddYears(-30),
            "joao@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103"));

        _customerRepositoryMock.Setup(x => x.BuscaCpfExistente(cpf, excludeCustomerId))
            .ReturnsAsync(clienteExistente);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.ValidateCpfAsync(cpf, excludeCustomerId));
        exception.Message.Should().Be("Já existe outro Cliente com esse CPF cadastrado.");
    }

    [Fact]
    public async Task ValidateCnpjAsync_DevePassar_QuandoCNPJNaoExiste()
    {
        // Arrange
        var cnpj = "67964706000144";
        _customerRepositoryMock.Setup(x => x.BuscaCnpjExistente(cnpj, Guid.Empty))
            .ReturnsAsync((ClientePessoaJuridica?)null);

        // Act
        await _service.ValidateCnpjAsync(cnpj);

        // Assert
        _customerRepositoryMock.Verify(x => x.BuscaCnpjExistente(cnpj, Guid.Empty), Times.Once);
    }

    [Fact]
    public async Task ValidateCnpjAsync_DeveLancarExcecao_QuandoCNPJJaExiste()
    {
        // Arrange
        var cnpj = "67964706000144";
        var clienteExistente = new ClientePessoaJuridica(
            "Empresa LTDA",
            "Empresa",
            cnpj,
            "empresa@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103"));

        _customerRepositoryMock.Setup(x => x.BuscaCnpjExistente(cnpj, Guid.Empty))
            .ReturnsAsync(clienteExistente);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.ValidateCnpjAsync(cnpj));
        exception.Message.Should().Be("Já existe um cliente cadastrado com este CNPJ.");
    }

    [Fact]
    public async Task ValidateCnpjAsync_DeveLancarExcecao_QuandoCNPJJaExisteEmOutroCliente()
    {
        // Arrange
        var cnpj = "67964706000144";
        var excludeCustomerId = Guid.NewGuid();
        var clienteExistente = new ClientePessoaJuridica(
            "Empresa LTDA",
            "Empresa",
            cnpj,
            "empresa@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103"));

        _customerRepositoryMock.Setup(x => x.BuscaCnpjExistente(cnpj, excludeCustomerId))
            .ReturnsAsync(clienteExistente);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.ValidateCnpjAsync(cnpj, excludeCustomerId));
        exception.Message.Should().Be("Já existe outro Cliente com esse CNPJ cadastrado.");
    }
}
