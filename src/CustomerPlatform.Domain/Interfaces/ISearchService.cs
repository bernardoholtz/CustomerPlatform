using CustomerPlatform.Domain.DTOs;

namespace CustomerPlatform.Domain.Interfaces
{
    /// <summary>
    /// Interface para serviço de busca avançada de clientes
    /// </summary>
    public interface ISearchService
    {
        /// <summary>
        /// Realiza busca avançada de clientes com suporte a fuzzy search, busca exata e parcial
        /// </summary>
        /// <param name="request">Parâmetros de busca</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns>Resultado paginado ordenado por relevância</returns>
        Task<CustomerSearchResponse> SearchAsync(CustomerSearchRequest request, CancellationToken cancellationToken = default);
    }
}
