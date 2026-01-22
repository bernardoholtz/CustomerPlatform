using CustomerPlatform.Application.Factories;
using CustomerPlatform.Application.Services;
using CustomerPlatform.Domain.Entities;
using CustomerPlatform.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CustomerPlatform.Application.Commands.UpdateCustomer
{
    public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessagePublisher _messagePublisher;
        private readonly IDocumentValidationService _documentValidationService;
        private readonly IElasticsearchIndexService _elasticsearchIndexService;
        private readonly ILogger<UpdateCustomerHandler> _logger;

        public UpdateCustomerHandler(
            IUnitOfWork unitOfWork,
            IMessagePublisher messagePublisher,
            IDocumentValidationService documentValidationService,
            IElasticsearchIndexService elasticsearchIndexService,
            ILogger<UpdateCustomerHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _messagePublisher = messagePublisher;
            _documentValidationService = documentValidationService;
            _elasticsearchIndexService = elasticsearchIndexService;
            _logger = logger;
        }

        public async Task<Guid> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation(
                    "Iniciando atualização de cliente. Id: {CustomerId}, Tipo: {TipoCliente}",
                    command.Id,
                    command.TipoCliente);

                var customer = await _unitOfWork.Customers.BuscarPorId(command.Id);

                if (customer == null)
                {
                    _logger.LogWarning("Cliente não encontrado. Id: {CustomerId}", command.Id);
                    throw new InvalidOperationException("Cliente não encontrado.");
                }

                await ValidateDocumentAsync(customer, command);

                UpdateCustomerEntity(customer, command);

                await _unitOfWork.Customers.Editar(customer);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation(
                    "Cliente atualizado com sucesso. Id: {CustomerId}",
                    customer.Id);

                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _elasticsearchIndexService.IndexCustomerAsync(customer, cancellationToken);
                        _logger.LogDebug("Cliente {CustomerId} atualizado no Elasticsearch", customer.Id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            ex,
                            "Erro ao atualizar índice do cliente {CustomerId} no Elasticsearch",
                            customer.Id);
                    }
                }, cancellationToken);

                try
                {
                    var evento = CustomerEventFactory.CreateCustomerUpdatedEvent(customer);
                    await _messagePublisher.PublishAsync(evento, cancellationToken);
                    _logger.LogDebug("Evento de atualização publicado para cliente {CustomerId}", customer.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Erro ao publicar evento de atualização para cliente {CustomerId}",
                        customer.Id);
                    // Não relança a exceção para não falhar a atualização do cliente
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
                    "Erro ao atualizar cliente. Id: {CustomerId}",
                    command.Id);
                throw;
            }
        }

        private async Task ValidateDocumentAsync(Customer customer, UpdateCustomerCommand command)
        {
            try
            {
                switch (customer)
                {
                    case ClientePessoaFisica:
                        _logger.LogDebug("Validando CPF para atualização do cliente {CustomerId}", customer.Id);
                        await _documentValidationService.ValidateCpfAsync(command.CPF!, command.Id);
                        break;

                    case ClientePessoaJuridica:
                        _logger.LogDebug("Validando CNPJ para atualização do cliente {CustomerId}", customer.Id);
                        await _documentValidationService.ValidateCnpjAsync(command.CNPJ!, command.Id);
                        break;
                }
            }
            catch (InvalidOperationException)
            {
                _logger.LogWarning(
                    "Documento já existe para outro cliente. ClienteId: {CustomerId}",
                    customer.Id);
                throw;
            }
        }

        private static void UpdateCustomerEntity(Customer customer, UpdateCustomerCommand command)
        {
            switch (customer)
            {
                case ClientePessoaFisica pf:
                    pf.Atualizar(
                        command.Nome!,
                        command.CPF!,
                        command.DataNascimento,
                        command.Email,
                        command.Telefone,
                        command.Endereco);
                    break;

                case ClientePessoaJuridica pj:
                    pj.Atualizar(
                        command.RazaoSocial!,
                        command.NomeFantasia!,
                        command.CNPJ!,
                        command.Email,
                        command.Telefone,
                        command.Endereco);
                    break;
            }
        }
    }
}
