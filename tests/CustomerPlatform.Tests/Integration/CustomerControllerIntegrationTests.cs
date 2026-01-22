using CustomerPlatform.Api.Controllers;
using CustomerPlatform.Application.Commands.CreateCustomer;
using CustomerPlatform.Application.Commands.SearchCustomer;
using CustomerPlatform.Application.Commands.UpdateCustomer;
using CustomerPlatform.Domain.DTOs;
using CustomerPlatform.Domain.Enums;
using CustomerPlatform.Domain.ValueObjects;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CustomerPlatform.Tests.Integration;

public class CustomerControllerIntegrationTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ILogger<CustomerController> _logger;
    private readonly CustomerController _controller;

    public CustomerControllerIntegrationTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _logger = Mock.Of<ILogger<CustomerController>>();
        _controller = new CustomerController(_mediatorMock.Object, _logger);
    }

    [Fact]
    public async Task Post_DeveRetornarCreatedAtAction_QuandoClienteCriado()
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

        var customerId = Guid.NewGuid();
        _mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(customerId);

        // Act
        var result = await _controller.Post(command);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdAtResult = result as CreatedAtActionResult;
        createdAtResult!.Value.Should().Be(customerId);
        _mediatorMock.Verify(x => x.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Put_DeveRetornarCreatedAtAction_QuandoClienteAtualizado()
    {
        // Arrange
        var command = new UpdateCustomerCommand
        {
            Id = Guid.NewGuid(),
            TipoCliente = TipoCliente.PessoaFisica,
            Nome = "João Silva",
            CPF = "52998224725",
            DataNascimento = DateTime.UtcNow.AddYears(-30),
            Email = "joao@email.com",
            Telefone = "2199887766",
            Endereco = new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103")
        };

        var customerId = command.Id;
        _mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(customerId);

        // Act
        var result = await _controller.Put(command);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdAtResult = result as CreatedAtActionResult;
        createdAtResult!.Value.Should().Be(customerId);
        _mediatorMock.Verify(x => x.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Search_DeveRetornarOk_QuandoBuscaValida()
    {
        // Arrange
        var command = new SearchCustomerCommand
        {
            Nome = "João",
            Page = 1,
            PageSize = 10
        };

        var searchResponse = new CustomerSearchResponse
        {
            Results = new List<CustomerSearchResult>
            {
                new CustomerSearchResult
                {
                    Id = Guid.NewGuid(),
                    Nome = "João Silva",
                    Email = "joao@email.com"
                }
            },
            Total = 1,
            Page = 1,
            PageSize = 10
        };

        _mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(searchResponse);

        // Act
        var result = await _controller.Search(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(searchResponse);
        _mediatorMock.Verify(x => x.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Search_DeveRetornarBadRequest_QuandoCommandNulo()
    {
        // Act
        var result = await _controller.Search(null!, CancellationToken.None);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.Value.Should().Be("Command não pode ser nulo");
        _mediatorMock.Verify(x => x.Send(It.IsAny<SearchCustomerCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Get_DeveRetornarOk_QuandoListaDuplicatas()
    {
        // Arrange
        var command = new CustomerPlatform.Application.Commands.DuplicateList.DuplicateListCommand
        {
            DataIni = DateTimeOffset.UtcNow.AddDays(-30),
            DataFim = DateTimeOffset.UtcNow
        };

        var duplicatas = new List<CustomerPlatform.Domain.Entities.SuspeitaDuplicidade>
        {
            new CustomerPlatform.Domain.Entities.SuspeitaDuplicidade
            {
                Id = Guid.NewGuid(),
                Score = 0.95,
                DetalhesSimilaridade = "Similaridade alta"
            }
        };

        _mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(duplicatas);

        // Act
        var result = await _controller.Get(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(duplicatas);
        _mediatorMock.Verify(x => x.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }
}
