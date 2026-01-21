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
            var customer = CustomerFactory.CriarInstancia(command);

            await ValidateDocumentAsync(customer);

            await _unitOfWork.Customers.Criar(customer);
            await _unitOfWork.CommitAsync();

            _ = Task.Run(async () =>
            {
                try
                {
                    await _elasticsearchIndexService.IndexCustomerAsync(customer, cancellationToken);
                }
                catch (Exception ex)
                {
                }
            }, cancellationToken);

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
