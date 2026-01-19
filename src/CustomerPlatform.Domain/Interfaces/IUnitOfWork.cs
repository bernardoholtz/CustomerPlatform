namespace CustomerPlatform.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository Customers { get; }

        Task<int> CommitAsync();
    }
}
