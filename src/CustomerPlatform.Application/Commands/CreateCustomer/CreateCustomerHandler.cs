using CustomerPlatform.Application.Factories;
using CustomerPlatform.Application.Services;
using CustomerPlatform.Domain.Entities;
using CustomerPlatform.Domain.Interfaces;
using MediatR;

namespace CustomerPlatform.Application.Commands.CreateCustomer
{
    public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessagePublisher _messagePublisher;
        private readonly IDocumentValidationService _documentValidationService;
        private readonly IElasticsearchIndexService _elasticsearchIndexService;

        public CreateCustomerHandler(
            IUnitOfWork unitOfWork, 
            IMessagePublisher messagePublisher,
            IDocumentValidationService documentValidationService,
            IElasticsearchIndexService elasticsearchIndexService)
        {
            _unitOfWork = unitOfWork;
            _messagePublisher = messagePublisher;
            _documentValidationService = documentValidationService;
            _elasticsearchIndexService = elasticsearchIndexService;
        }

        public async Task<Guid> Handle(
           CreateCustomerCommand command,
           CancellationToken cancellationToken)
        {
            // 1. Cria a entidade Customer
            var customer = CustomerFactory.Criar(command);

            // 2. Valida documento duplicado
            await ValidateDocumentAsync(customer);

            // 3. Persiste no banco
            await _unitOfWork.Customers.Criar(customer);
            await _unitOfWork.CommitAsync();

            // 4. Indexa no Elasticsearch (fire and forget para não bloquear)
            _ = Task.Run(async () =>
            {
                try
                {
                    await _elasticsearchIndexService.IndexCustomerAsync(customer, cancellationToken);
                }
                catch (Exception ex)
                {
                    // Log do erro, mas não falha a operação principal
                    // Em produção, considerar usar um serviço de retry ou fila
                }
            }, cancellationToken);

            // 5. Publica evento
            var evento = CustomerEventFactory.CreateCustomerCreatedEvent(customer);
            await _messagePublisher.PublishAsync(evento, cancellationToken);

            return customer.Id;
        }

        private async Task ValidateDocumentAsync(Customer customer)
        {
            switch (customer)
            {
                case ClientePessoaFisica pf:
                    await _documentValidationService.ValidateCpfAsync(pf.CPF);
                    break;

                case ClientePessoaJuridica pj:
                    await _documentValidationService.ValidateCnpjAsync(pj.CNPJ);
                    break;

                default:
                    throw new ArgumentException("Tipo de cliente inválido", nameof(customer));
            }
        }
    }
}
