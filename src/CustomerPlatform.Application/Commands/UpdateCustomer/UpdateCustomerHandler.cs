using CustomerPlatform.Application.Factories;
using CustomerPlatform.Application.Services;
using CustomerPlatform.Domain.Entities;
using CustomerPlatform.Domain.Interfaces;
using MediatR;

namespace CustomerPlatform.Application.Commands.UpdateCustomer
{
    public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessagePublisher _messagePublisher;
        private readonly IDocumentValidationService _documentValidationService;
        private readonly IElasticsearchIndexService _elasticsearchIndexService;

        public UpdateCustomerHandler(
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

        public async Task<Guid> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
        {
            var customer = await _unitOfWork.Customers.BuscarPorId(command.Id);

            if (customer == null)
            {
                throw new InvalidOperationException("Cliente não encontrado.");
            }

            await ValidateDocumentAsync(customer, command);

            UpdateCustomerEntity(customer, command);

            await _unitOfWork.Customers.Editar(customer);
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

            var evento = CustomerEventFactory.CreateCustomerUpdatedEvent(customer);
            await _messagePublisher.PublishAsync(evento, cancellationToken);

            return customer.Id;
        }

        private async Task ValidateDocumentAsync(Customer customer, UpdateCustomerCommand command)
        {
            switch (customer)
            {
                case ClientePessoaFisica:
                    await _documentValidationService.ValidateCpfAsync(command.CPF!, command.Id);
                    break;

                case ClientePessoaJuridica:
                    await _documentValidationService.ValidateCnpjAsync(command.CNPJ!, command.Id);
                    break;
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
