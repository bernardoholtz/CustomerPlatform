using CustomerPlatform.Domain.Interfaces;
using CustomerPlatform.Infrastructure.Contexts;

namespace CustomerPlatform.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CustomerDbContext _context;

        public ICustomerRepository Customers { get; }

        public UnitOfWork(
            CustomerDbContext context,
            ICustomerRepository customerRepository)
        {
            _context = context;
            Customers = customerRepository;
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
