using CustomerPlatform.Application.Commands.UpdateCustomer;
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

public class UpdateCustomerHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMessagePublisher> _messagePublisherMock;
    private readonly Mock<IDocumentValidationService> _documentValidationServiceMock;
    private readonly Mock<IElasticsearchIndexService> _elasticsearchIndexServiceMock;
    private readonly ILogger<UpdateCustomerHandler> _logger;
    private readonly UpdateCustomerHandler _handler;

    public UpdateCustomerHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _messagePublisherMock = new Mock<IMessagePublisher>();
        _documentValidationServiceMock = new Mock<IDocumentValidationService>();
        _elasticsearchIndexServiceMock = new Mock<IElasticsearchIndexService>();
        _logger = Mock.Of<ILogger<UpdateCustomerHandler>>();

        _handler = new UpdateCustomerHandler(
            _unitOfWorkMock.Object,
            _messagePublisherMock.Object,
            _documentValidationServiceMock.Object,
            _elasticsearchIndexServiceMock.Object,
            _logger);
    }

    [Fact]
    public async Task Handle_DeveAtualizarClientePessoaFisica_QuandoClienteExiste()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var existingCustomer = new ClientePessoaFisica(
            "João Silva",
            "52998224725",
            DateTime.UtcNow.AddYears(-30),
            "joao@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103"));

        var command = new UpdateCustomerCommand
        {
            Id = customerId,
            TipoCliente = TipoCliente.PessoaFisica,
            Nome = "João Silva Atualizado",
            CPF = "52998224725",
            DataNascimento = DateTime.UtcNow.AddYears(-30),
            Email = "joao.novo@email.com",
            Telefone = "2199887767",
            Endereco = new Endereco("Av 7 de Setembro", "4201", "22076091", "Rio de Janeiro", "RJ", "Sala 104")
        };

        _unitOfWorkMock.Setup(x => x.Customers.BuscarPorId(customerId))
            .ReturnsAsync(existingCustomer);

        _unitOfWorkMock.Setup(x => x.Customers.Editar(It.IsAny<Customer>()))
            .ReturnsAsync((Customer c) => c);

        _documentValidationServiceMock.Setup(x => x.ValidateCpfAsync(It.IsAny<string>(), It.IsAny<Guid?>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(customerId);
        _unitOfWorkMock.Verify(x => x.Customers.BuscarPorId(customerId), Times.Once);
        _unitOfWorkMock.Verify(x => x.Customers.Editar(It.IsAny<Customer>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
        _documentValidationServiceMock.Verify(x => x.ValidateCpfAsync(It.IsAny<string>(), It.IsAny<Guid?>()), Times.Once);
        _messagePublisherMock.Verify(x => x.PublishAsync(It.IsAny<Domain.Events.CustomerEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveAtualizarClientePessoaJuridica_QuandoClienteExiste()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var existingCustomer = new ClientePessoaJuridica(
            "Empresa LTDA",
            "Empresa",
            "67964706000144",
            "empresa@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103"));

        var command = new UpdateCustomerCommand
        {
            Id = customerId,
            TipoCliente = TipoCliente.PessoaJuridica,
            RazaoSocial = "Empresa LTDA Atualizada",
            NomeFantasia = "Empresa Nova",
            CNPJ = "67964706000144",
            Email = "empresa.nova@email.com",
            Telefone = "2199887767",
            Endereco = new Endereco("Av 7 de Setembro", "4201", "22076091", "Rio de Janeiro", "RJ", "Sala 104")
        };

        _unitOfWorkMock.Setup(x => x.Customers.BuscarPorId(customerId))
            .ReturnsAsync(existingCustomer);

        _unitOfWorkMock.Setup(x => x.Customers.Editar(It.IsAny<Customer>()))
            .ReturnsAsync((Customer c) => c);

        _documentValidationServiceMock.Setup(x => x.ValidateCnpjAsync(It.IsAny<string>(), It.IsAny<Guid?>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(customerId);
        _unitOfWorkMock.Verify(x => x.Customers.BuscarPorId(customerId), Times.Once);
        _unitOfWorkMock.Verify(x => x.Customers.Editar(It.IsAny<Customer>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
        _documentValidationServiceMock.Verify(x => x.ValidateCnpjAsync(It.IsAny<string>(), It.IsAny<Guid?>()), Times.Once);
        _messagePublisherMock.Verify(x => x.PublishAsync(It.IsAny<Domain.Events.CustomerEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_QuandoClienteNaoExiste()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var command = new UpdateCustomerCommand
        {
            Id = customerId,
            TipoCliente = TipoCliente.PessoaFisica,
            Nome = "João Silva",
            CPF = "52998224725",
            DataNascimento = DateTime.UtcNow.AddYears(-30),
            Email = "joao@email.com",
            Telefone = "2199887766",
            Endereco = new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103")
        };

        _unitOfWorkMock.Setup(x => x.Customers.BuscarPorId(customerId))
            .ReturnsAsync((Customer?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        exception.Message.Should().Be("Cliente não encontrado.");
        _unitOfWorkMock.Verify(x => x.Customers.Editar(It.IsAny<Customer>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_QuandoCPFJaExisteEmOutroCliente()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var existingCustomer = new ClientePessoaFisica(
            "João Silva",
            "52998224725",
            DateTime.UtcNow.AddYears(-30),
            "joao@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103"));

        var command = new UpdateCustomerCommand
        {
            Id = customerId,
            TipoCliente = TipoCliente.PessoaFisica,
            Nome = "João Silva",
            CPF = "52998224725",
            DataNascimento = DateTime.UtcNow.AddYears(-30),
            Email = "joao@email.com",
            Telefone = "2199887766",
            Endereco = new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103")
        };

        _unitOfWorkMock.Setup(x => x.Customers.BuscarPorId(customerId))
            .ReturnsAsync(existingCustomer);

        _documentValidationServiceMock.Setup(x => x.ValidateCpfAsync(It.IsAny<string>(), It.IsAny<Guid?>()))
            .ThrowsAsync(new InvalidOperationException("Já existe outro Cliente com esse CPF cadastrado."));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        _unitOfWorkMock.Verify(x => x.Customers.Editar(It.IsAny<Customer>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Never);
    }
}
