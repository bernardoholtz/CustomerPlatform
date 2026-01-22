using CustomerPlatform.Application.Commands.DuplicateList;
using CustomerPlatform.Domain.Entities;
using CustomerPlatform.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CustomerPlatform.Tests.Handlers;

public class DuplicateListHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly ILogger<DuplicateListHandler> _logger;
    private readonly DuplicateListHandler _handler;

    public DuplicateListHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _logger = Mock.Of<ILogger<DuplicateListHandler>>();
        _handler = new DuplicateListHandler(_unitOfWorkMock.Object, _logger);
    }

    [Fact]
    public async Task Handle_DeveRetornarListaDeDuplicatas_QuandoExistemDuplicatas()
    {
        // Arrange
        var command = new DuplicateListCommand
        {
            DataIni = DateTimeOffset.UtcNow.AddDays(-30),
            DataFim = DateTimeOffset.UtcNow
        };

        var expectedDuplicatas = new List<SuspeitaDuplicidade>
        {
            new SuspeitaDuplicidade
            {
                Id = Guid.NewGuid(),
                IdOriginal = Guid.NewGuid(),
                IdSuspeito = Guid.NewGuid(),
                Score = 0.95,
                DetalhesSimilaridade = "Similaridade alta",
                DataDeteccao = DateTime.Now
            },
            new SuspeitaDuplicidade
            {
                Id = Guid.NewGuid(),
                IdOriginal = Guid.NewGuid(),
                IdSuspeito = Guid.NewGuid(),
                Score = 0.85,
                DetalhesSimilaridade = "Similaridade média",
                DataDeteccao = DateTime.Now.AddDays(-1)
            }
        };

        _unitOfWorkMock.Setup(x => x.Customers.ListaSuspeitosDuplicatas(
                It.IsAny<DateTimeOffset>(),
                It.IsAny<DateTimeOffset>()))
            .ReturnsAsync(expectedDuplicatas);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(expectedDuplicatas);
        _unitOfWorkMock.Verify(x => x.Customers.ListaSuspeitosDuplicatas(
            It.IsAny<DateTimeOffset>(),
            It.IsAny<DateTimeOffset>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveRetornarListaVazia_QuandoNaoExistemDuplicatas()
    {
        // Arrange
        var command = new DuplicateListCommand
        {
            DataIni = DateTimeOffset.UtcNow.AddDays(-30),
            DataFim = DateTimeOffset.UtcNow
        };

        _unitOfWorkMock.Setup(x => x.Customers.ListaSuspeitosDuplicatas(
                It.IsAny<DateTimeOffset>(),
                It.IsAny<DateTimeOffset>()))
            .ReturnsAsync(new List<SuspeitaDuplicidade>());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_DeveConverterDatasParaUniversalTime()
    {
        // Arrange
        var dataIni = new DateTimeOffset(2024, 1, 1, 10, 0, 0, TimeSpan.FromHours(-3));
        var dataFim = new DateTimeOffset(2024, 1, 31, 10, 0, 0, TimeSpan.FromHours(-3));

        var command = new DuplicateListCommand
        {
            DataIni = dataIni,
            DataFim = dataFim
        };

        _unitOfWorkMock.Setup(x => x.Customers.ListaSuspeitosDuplicatas(
                It.IsAny<DateTimeOffset>(),
                It.IsAny<DateTimeOffset>()))
            .ReturnsAsync(new List<SuspeitaDuplicidade>());

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.Customers.ListaSuspeitosDuplicatas(
            It.Is<DateTimeOffset>(d => d == dataIni.ToUniversalTime()),
            It.Is<DateTimeOffset>(d => d == dataFim.ToUniversalTime())), Times.Once);
    }
}
