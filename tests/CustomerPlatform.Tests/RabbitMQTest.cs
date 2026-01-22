using Castle.Core.Resource;
using CustomerPlatform.Application.Commands.CreateCustomer;
using CustomerPlatform.Application.Services;
using CustomerPlatform.Domain.Entities;
using CustomerPlatform.Domain.Enums;
using CustomerPlatform.Domain.Events;
using CustomerPlatform.Domain.Interfaces;
using CustomerPlatform.Domain.ValueObjects;
using Moq;

namespace CustomerPlatform.Tests
{
    public class RabbitMQTest
    {
        [Fact]
        public async Task Handle_DevePublicarEvento_QuandoClienteCriado()
        {
            // Arrange
            var publisher = new Mock<IMessagePublisher>();
            var uow = new Mock<IUnitOfWork>();
            var service = new Mock<IDocumentValidationService>();
            var elastic = new Mock<IElasticsearchIndexService>();

            uow.Setup(u => u.Customers.Criar(It.IsAny<Customer>()))
               .ReturnsAsync((Customer c) => c);

            var handler = new CreateCustomerHandler(
                uow.Object,
                publisher.Object,
                service.Object,
                elastic.Object);

            var command = new CreateCustomerCommand
            {
                Nome = "João Silva",
                CPF = "52998224725",
                DataNascimento = DateTime.UtcNow.AddYears(-30),
                Email = "email-invalido", 
                Telefone = "2199887766",
                Endereco = new Endereco(
                 "Av 7 de Setembro",
                 "4200",
                 "22076090",
                 "Rio de Janeiro",
                 "RJ",
                 "Sala 103"
                 )
            };

            // Act
            await handler.Handle(command, CancellationToken.None);
       
            // Assert
            publisher.Verify(p =>
             p.PublishAsync(
                 It.Is<CustomerEvent>(e =>
                     e.EventType == "ClienteCriado" &&
                     e.Data.ClienteId != Guid.Empty &&
                     e.Data.Email == command.Email &&
                     e.Data.Telefone == command.Telefone
                 ),
                 It.IsAny<CancellationToken>()),
             Times.Once);
            }

    }
}
