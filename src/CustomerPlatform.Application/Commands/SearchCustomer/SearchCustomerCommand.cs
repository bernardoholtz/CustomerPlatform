using CustomerPlatform.Domain.DTOs;
using MediatR;

namespace CustomerPlatform.Application.Commands.SearchCustomer
{
    /// <summary>
    /// Command para busca avançada de clientes
    /// </summary>
    public class SearchCustomerCommand : IRequest<CustomerSearchResponse>
    {
        /// <summary>
        /// Nome completo (PF) ou Razão Social (PJ) - busca fuzzy
        /// </summary>
        public string? Nome { get; set; }

        /// <summary>
        /// CPF (busca exata)
        /// </summary>
        public string? CPF { get; set; }

        /// <summary>
        /// CNPJ (busca exata)
        /// </summary>
        public string? CNPJ { get; set; }

        /// <summary>
        /// Email (busca parcial)
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Telefone (busca parcial)
        /// </summary>
        public string? Telefone { get; set; }

        /// <summary>
        /// Página atual (padrão: 1)
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Tamanho da página (padrão: 10)
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}
