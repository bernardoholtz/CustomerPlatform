using CustomerPlatform.Application.Commands.SearchCustomer;
using CustomerPlatform.Domain.DTOs;
using CustomerPlatform.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CustomerPlatform.Tests.Handlers;

public class SearchCustomerHandlerTests
{
    private readonly Mock<ISearchService> _searchServiceMock;
    private readonly ILogger<SearchCustomerHandler> _logger;
    private readonly SearchCustomerHandler _handler;

    public SearchCustomerHandlerTests()
    {
        _searchServiceMock = new Mock<ISearchService>();
        _logger = Mock.Of<ILogger<SearchCustomerHandler>>();
        _handler = new SearchCustomerHandler(_searchServiceMock.Object, _logger);
    }

    [Fact]
    public async Task Handle_DeveRetornarResultado_QuandoBuscaValida()
    {
        // Arrange
        var command = new SearchCustomerCommand
        {
            Nome = "João",
            Page = 1,
            PageSize = 10
        };

        var expectedResponse = new CustomerSearchResponse
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

        _searchServiceMock.Setup(x => x.SearchAsync(It.IsAny<Domain.DTOs.CustomerSearchRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Total.Should().Be(1);
        result.Results.Should().HaveCount(1);
        _searchServiceMock.Verify(x => x.SearchAsync(It.IsAny<Domain.DTOs.CustomerSearchRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveAjustarPagePara1_QuandoPageMenorQue1()
    {
        // Arrange
        var command = new SearchCustomerCommand
        {
            Nome = "João",
            Page = 0,
            PageSize = 10
        };

        var expectedResponse = new CustomerSearchResponse
        {
            Results = new List<CustomerSearchResult>(),
            Total = 0,
            Page = 1,
            PageSize = 10
        };

        _searchServiceMock.Setup(x => x.SearchAsync(It.IsAny<Domain.DTOs.CustomerSearchRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _searchServiceMock.Verify(x => x.SearchAsync(
            It.Is<Domain.DTOs.CustomerSearchRequest>(r => r.Page == 1),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveAjustarPageSize_QuandoPageSizeMaiorQue100()
    {
        // Arrange
        var command = new SearchCustomerCommand
        {
            Nome = "João",
            Page = 1,
            PageSize = 150
        };

        var expectedResponse = new CustomerSearchResponse
        {
            Results = new List<CustomerSearchResult>(),
            Total = 0,
            Page = 1,
            PageSize = 100
        };

        _searchServiceMock.Setup(x => x.SearchAsync(It.IsAny<Domain.DTOs.CustomerSearchRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _searchServiceMock.Verify(x => x.SearchAsync(
            It.Is<Domain.DTOs.CustomerSearchRequest>(r => r.PageSize == 100),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveAjustarPageSize_QuandoPageSizeMenorQue1()
    {
        // Arrange
        var command = new SearchCustomerCommand
        {
            Nome = "João",
            Page = 1,
            PageSize = 0
        };

        var expectedResponse = new CustomerSearchResponse
        {
            Results = new List<CustomerSearchResult>(),
            Total = 0,
            Page = 1,
            PageSize = 1
        };

        _searchServiceMock.Setup(x => x.SearchAsync(It.IsAny<Domain.DTOs.CustomerSearchRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _searchServiceMock.Verify(x => x.SearchAsync(
            It.Is<Domain.DTOs.CustomerSearchRequest>(r => r.PageSize == 1),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
