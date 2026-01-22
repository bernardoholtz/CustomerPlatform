using CustomerPlatform.Application.Factories;
using CustomerPlatform.Application.Services;
using CustomerPlatform.Domain.Entities;
using CustomerPlatform.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CustomerPlatform.Application.Commands.CreateCustomer
{
    public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessagePublisher _messagePublisher;
        private readonly IDocumentValidationService _documentValidationService;
        private readonly IElasticsearchIndexService _elasticsearchIndexService;
        private readonly ILogger<CreateCustomerHandler> _logger;

        public CreateCustomerHandler(
            IUnitOfWork unitOfWork,
            IMessagePublisher messagePublisher,
            IDocumentValidationService documentValidationService,
            IElasticsearchIndexService elasticsearchIndexService,
            ILogger<CreateCustomerHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _messagePublisher = messagePublisher;
            _documentValidationService = documentValidationService;
            _elasticsearchIndexService = elasticsearchIndexService;
            _logger = logger;
        }

        public async Task<Guid> Handle(
           CreateCustomerCommand command,
           CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation(
                    "Iniciando criação de cliente. Tipo: {TipoCliente}",
                    command.TipoCliente);

                var customer = CustomerFactory.CriarInstancia(command);

                await ValidateDocumentAsync(customer);

                await _unitOfWork.Customers.Criar(customer);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation(
                    "Cliente criado com sucesso. Id: {CustomerId}, Tipo: {TipoCliente}",
                    customer.Id,
                    command.TipoCliente);

                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _elasticsearchIndexService.IndexCustomerAsync(customer, cancellationToken);
                        _logger.LogDebug("Cliente {CustomerId} indexado no Elasticsearch", customer.Id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            ex,
                            "Erro ao indexar cliente {CustomerId} no Elasticsearch",
                            customer.Id);
                    }
                }, cancellationToken);

                try
                {
                    var evento = CustomerEventFactory.CreateCustomerCreatedEvent(customer);
                    await _messagePublisher.PublishAsync(evento, cancellationToken);
                    _logger.LogDebug("Evento de criação publicado para cliente {CustomerId}", customer.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Erro ao publicar evento de criação para cliente {CustomerId}",
                        customer.Id);
                    // Não relança a exceção para não falhar a criação do cliente
                }

                return customer.Id;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao criar cliente. Tipo: {TipoCliente}",
                    command.TipoCliente);
                throw;
            }
        }

        private async Task ValidateDocumentAsync(Customer customer)
        {
            try
            {
                switch (customer)
                {
                    case ClientePessoaFisica pf:
                        _logger.LogDebug("Validando CPF para cliente {CustomerId}", customer.Id);
                        await _documentValidationService.ValidateCpfAsync(pf.CPF);
                        break;

                    case ClientePessoaJuridica pj:
                        _logger.LogDebug("Validando CNPJ para cliente {CustomerId}", customer.Id);
                        await _documentValidationService.ValidateCnpjAsync(pj.CNPJ);
                        break;

                    default:
                        throw new ArgumentException("Tipo de cliente inválido", nameof(customer));
                }
            }
            catch (InvalidOperationException)
            {
                _logger.LogWarning(
                    "Documento já existe para cliente {CustomerId}",
                    customer.Id);
                throw;
            }
        }
    }
}
