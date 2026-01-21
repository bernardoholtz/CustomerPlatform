using CustomerPlatform.Domain.DTOs;
using CustomerPlatform.Domain.Entities;

namespace CustomerPlatform.Domain.Interfaces
{
    /// <summary>
    /// Interface para serviço de indexação de clientes no Elasticsearch
    /// </summary>
    public interface IElasticsearchIndexService
    {
        /// <summary>
        /// Indexa ou atualiza um cliente no Elasticsearch
        /// </summary>
        Task IndexCustomerAsync(Customer customer, CancellationToken cancellationToken = default);

        /// <summary>
        /// Remove um cliente do índice
        /// </summary>
        Task DeleteCustomerAsync(Guid customerId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Cria o índice se não existir
        /// </summary>
        Task EnsureIndexExistsAsync(CancellationToken cancellationToken = default);
    }
}
