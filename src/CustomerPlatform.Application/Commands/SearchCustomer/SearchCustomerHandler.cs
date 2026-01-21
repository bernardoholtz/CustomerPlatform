using CustomerPlatform.Domain.DTOs;
using CustomerPlatform.Domain.Interfaces;
using MediatR;

namespace CustomerPlatform.Application.Commands.SearchCustomer
{
    /// <summary>
    /// Handler para busca avançada de clientes
    /// </summary>
    public class SearchCustomerHandler : IRequestHandler<SearchCustomerCommand, CustomerSearchResponse>
    {
        private readonly ISearchService _searchService;

        public SearchCustomerHandler(ISearchService searchService)
        {
            _searchService = searchService;
        }

        public async Task<CustomerSearchResponse> Handle(
            SearchCustomerCommand command,
            CancellationToken cancellationToken)
        {
            // Validação básica
            if (command.Page < 1)
            {
                command.Page = 1;
            }

            if (command.PageSize < 1 || command.PageSize > 100)
            {
                command.PageSize = Math.Clamp(command.PageSize, 1, 100);
            }

            // Converte Command para Request
            var request = new CustomerSearchRequest
            {
                Nome = command.Nome,
                CPF = command.CPF,
                CNPJ = command.CNPJ,
                Email = command.Email,
                Telefone = command.Telefone,
                Page = command.Page,
                PageSize = command.PageSize
            };

            return await _searchService.SearchAsync(request, cancellationToken);
        }
    }
}
