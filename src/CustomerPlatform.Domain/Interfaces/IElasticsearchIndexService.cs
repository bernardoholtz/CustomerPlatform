using CustomerPlatform.Domain.DTOs;
using CustomerPlatform.Domain.Entities;

namespace CustomerPlatform.Domain.Interfaces
{
    public interface IElasticsearchIndexService
    {
        Task IndexCustomerAsync(Customer customer, CancellationToken cancellationToken = default);
        Task DeleteCustomerAsync(Guid customerId, CancellationToken cancellationToken = default);
        Task EnsureIndexExistsAsync(CancellationToken cancellationToken = default);
        Task DeleteAllAsync(CancellationToken cancellationToken = default);

    }
}
