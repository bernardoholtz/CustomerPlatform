using CustomerPlatform.Domain.Interfaces;
using CustomerPlatform.Infrastructure.Contexts;
using Microsoft.Extensions.Logging;

namespace CustomerPlatform.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CustomerDbContext _context;
        private readonly ILogger<UnitOfWork> _logger;

        public ICustomerRepository Customers { get; }

        public UnitOfWork(
            CustomerDbContext context,
            ICustomerRepository customerRepository,
            ILogger<UnitOfWork> logger)
        {
            _context = context;
            Customers = customerRepository;
            _logger = logger;
        }

        public async Task<int> CommitAsync()
        {
            try
            {
                _logger.LogDebug("Iniciando commit de alterações no banco de dados");
                var result = await _context.SaveChangesAsync();
                _logger.LogDebug("Commit realizado com sucesso. {Count} entidades afetadas", result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao realizar commit no banco de dados");
                throw;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
