using CustomerPlatform.Application.Commands.CreateCustomer;
using CustomerPlatform.Application.Factories;
using CustomerPlatform.Application.Services;
using CustomerPlatform.Domain.Entities;
using CustomerPlatform.Domain.Enums;
using CustomerPlatform.Domain.Interfaces;
using CustomerPlatform.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CustomerPlatform.Tests.Handlers;

public class CreateCustomerHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMessagePublisher> _messagePublisherMock;
    private readonly Mock<IDocumentValidationService> _documentValidationServiceMock;
    private readonly Mock<IElasticsearchIndexService> _elasticsearchIndexServiceMock;
    private readonly ILogger<CreateCustomerHandler> _logger;
    private readonly CreateCustomerHandler _handler;

    public CreateCustomerHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _messagePublisherMock = new Mock<IMessagePublisher>();
        _documentValidationServiceMock = new Mock<IDocumentValidationService>();
        _elasticsearchIndexServiceMock = new Mock<IElasticsearchIndexService>();
        _logger = Mock.Of<ILogger<CreateCustomerHandler>>();

        _handler = new CreateCustomerHandler(
            _unitOfWorkMock.Object,
            _messagePublisherMock.Object,
            _documentValidationServiceMock.Object,
            _elasticsearchIndexServiceMock.Object,
            _logger);
    }

    [Fact]
    public async Task Handle_DeveCriarClientePessoaFisica_QuandoCommandValido()
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
            Endereco = new Endereco(
                "Av 7 de Setembro",
                "4200",
                "22076090",
                "Rio de Janeiro",
                "RJ",
                "Sala 103")
        };

        _unitOfWorkMock.Setup(x => x.Customers.Criar(It.IsAny<Customer>()))
            .ReturnsAsync((Customer c) => c);

        _documentValidationServiceMock.Setup(x => x.ValidateCpfAsync(It.IsAny<string>(), It.IsAny<Guid?>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        _unitOfWorkMock.Verify(x => x.Customers.Criar(It.IsAny<Customer>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
        _documentValidationServiceMock.Verify(x => x.ValidateCpfAsync(It.IsAny<string>(), It.IsAny<Guid?>()), Times.Once);
        _messagePublisherMock.Verify(x => x.PublishAsync(It.IsAny<Domain.Events.CustomerEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveCriarClientePessoaJuridica_QuandoCommandValido()
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
            Endereco = new Endereco(
                "Av 7 de Setembro",
                "4200",
                "22076090",
                "Rio de Janeiro",
                "RJ",
                "Sala 103")
        };

        _unitOfWorkMock.Setup(x => x.Customers.Criar(It.IsAny<Customer>()))
            .ReturnsAsync((Customer c) => c);

        _documentValidationServiceMock.Setup(x => x.ValidateCnpjAsync(It.IsAny<string>(), It.IsAny<Guid?>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        _unitOfWorkMock.Verify(x => x.Customers.Criar(It.IsAny<Customer>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
        _documentValidationServiceMock.Verify(x => x.ValidateCnpjAsync(It.IsAny<string>(), It.IsAny<Guid?>()), Times.Once);
        _messagePublisherMock.Verify(x => x.PublishAsync(It.IsAny<Domain.Events.CustomerEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_QuandoCPFJaExiste()
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
            Endereco = new Endereco(
                "Av 7 de Setembro",
                "4200",
                "22076090",
                "Rio de Janeiro",
                "RJ",
                "Sala 103")
        };

        _documentValidationServiceMock.Setup(x => x.ValidateCpfAsync(It.IsAny<string>(), It.IsAny<Guid?>()))
            .ThrowsAsync(new InvalidOperationException("Já existe um cliente cadastrado com este CPF."));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        _unitOfWorkMock.Verify(x => x.Customers.Criar(It.IsAny<Customer>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_QuandoCNPJJaExiste()
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
            Endereco = new Endereco(
                "Av 7 de Setembro",
                "4200",
                "22076090",
                "Rio de Janeiro",
                "RJ",
                "Sala 103")
        };

        _documentValidationServiceMock.Setup(x => x.ValidateCnpjAsync(It.IsAny<string>(), It.IsAny<Guid?>()))
            .ThrowsAsync(new InvalidOperationException("Já existe um cliente cadastrado com este CNPJ."));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        _unitOfWorkMock.Verify(x => x.Customers.Criar(It.IsAny<Customer>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Never);
    }
}
