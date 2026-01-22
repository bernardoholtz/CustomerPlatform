using CustomerPlatform.Domain.DTOs;
using CustomerPlatform.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CustomerPlatform.Application.Commands.SearchCustomer
{
    public class SearchCustomerHandler : IRequestHandler<SearchCustomerCommand, CustomerSearchResponse>
    {
        private readonly ISearchService _searchService;
        private readonly ILogger<SearchCustomerHandler> _logger;

        public SearchCustomerHandler(
            ISearchService searchService,
            ILogger<SearchCustomerHandler> logger)
        {
            _searchService = searchService;
            _logger = logger;
        }

        public async Task<CustomerSearchResponse> Handle(
            SearchCustomerCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation(
                    "Iniciando busca de clientes. Filtros: Nome={Nome}, CPF={CPF}, CNPJ={CNPJ}, Email={Email}, Telefone={Telefone}, Page={Page}, PageSize={PageSize}",
                    command.Nome,
                    command.CPF,
                    command.CNPJ,
                    command.Email,
                    command.Telefone,
                    command.Page,
                    command.PageSize);

                if (command.Page < 1)
                {
                    _logger.LogDebug("Page ajustado de {OldPage} para 1", command.Page);
                    command.Page = 1;
                }

                if (command.PageSize < 1 || command.PageSize > 100)
                {
                    var oldPageSize = command.PageSize;
                    command.PageSize = Math.Clamp(command.PageSize, 1, 100);
                    _logger.LogDebug("PageSize ajustado de {OldPageSize} para {NewPageSize}", oldPageSize, command.PageSize);
                }

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

                var result = await _searchService.SearchAsync(request, cancellationToken);

                _logger.LogInformation(
                    "Busca concluída. Total encontrado: {Total}, Página: {Page}, Tamanho da página: {PageSize}",
                    result.Total,
                    result.Page,
                    result.PageSize);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao realizar busca de clientes. Filtros: Nome={Nome}, CPF={CPF}, CNPJ={CNPJ}",
                    command.Nome,
                    command.CPF,
                    command.CNPJ);
                throw;
            }
        }
    }
}
