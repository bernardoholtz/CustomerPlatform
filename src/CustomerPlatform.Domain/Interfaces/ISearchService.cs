using CustomerPlatform.Domain.DTOs;

namespace CustomerPlatform.Domain.Interfaces
{
    public interface ISearchService
    {
        Task<CustomerSearchResponse> SearchAsync(CustomerSearchRequest request, CancellationToken cancellationToken = default);
    }
}
