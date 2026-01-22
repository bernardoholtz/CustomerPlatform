using CustomerPlatform.Domain.Entities;
using CustomerPlatform.Domain.Interfaces;
using CustomerPlatform.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CustomerPlatform.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext _context;
        private readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(
            CustomerDbContext context,
            ILogger<CustomerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Customer> Criar(Customer customer)
        {
            try
            {
                _logger.LogDebug("Adicionando cliente ao contexto. Id: {CustomerId}", customer.Id);
                await _context.Customers.AddAsync(customer);
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao adicionar cliente ao contexto. Id: {CustomerId}",
                    customer.Id);
                throw;
            }
        }

        public async Task<List<SuspeitaDuplicidade>> ListaSuspeitosDuplicatas(
            DateTimeOffset dataIni,
            DateTimeOffset dataFim)
        {
            try
            {
                _logger.LogDebug(
                    "Buscando suspeitas de duplicidade no banco. Período: {DataIni} a {DataFim}",
                    dataIni,
                    dataFim);

                var result = await _context.SuspeitaDuplicidades
                    .Where(s => s.DataDeteccao >= dataIni && s.DataDeteccao <= dataFim)
                    .OrderByDescending(s => s.Score)
                    .ToListAsync();

                _logger.LogDebug(
                    "Encontradas {Count} suspeitas de duplicidade no banco",
                    result.Count);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao buscar suspeitas de duplicidade. Período: {DataIni} a {DataFim}",
                    dataIni,
                    dataFim);
                throw;
            }
        }

        public async Task<Customer> Editar(Customer customer)
        {
            try
            {
                _logger.LogDebug("Atualizando cliente no contexto. Id: {CustomerId}", customer.Id);
                _context.Customers.Update(customer);
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao atualizar cliente no contexto. Id: {CustomerId}",
                    customer.Id);
                throw;
            }
        }

        public async Task<Customer> BuscarPorId(Guid id)
        {
            try
            {
                _logger.LogDebug("Buscando cliente por Id: {CustomerId}", id);
                var customer = await _context.Customers.FindAsync(id);

                if (customer == null)
                {
                    _logger.LogWarning("Cliente não encontrado. Id: {CustomerId}", id);
                }
                else
                {
                    _logger.LogDebug("Cliente encontrado. Id: {CustomerId}", id);
                }

                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao buscar cliente por Id: {CustomerId}",
                    id);
                throw;
            }
        }

        public async Task<ClientePessoaFisica> BuscaCpfExistente(string cpf, Guid id)
        {
            try
            {
                _logger.LogDebug(
                    "Buscando CPF existente. CPF: {CPF}, ExcludeId: {ExcludeId}",
                    cpf,
                    id);

                var result = id == Guid.Empty
                    ? await _context.PessoaFisica
                        .Where(c => c.CPF == cpf)
                        .FirstOrDefaultAsync()
                    : await _context.PessoaFisica
                        .Where(c => c.Id != id && c.CPF == cpf)
                        .FirstOrDefaultAsync();

                if (result != null)
                {
                    _logger.LogDebug(
                        "CPF encontrado. CPF: {CPF}, ClienteId: {ClienteId}",
                        cpf,
                        result.Id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao buscar CPF existente. CPF: {CPF}",
                    cpf);
                throw;
            }
        }

        public async Task<ClientePessoaJuridica> BuscaCnpjExistente(string cnpj, Guid id)
        {
            try
            {
                _logger.LogDebug(
                    "Buscando CNPJ existente. CNPJ: {CNPJ}, ExcludeId: {ExcludeId}",
                    cnpj,
                    id);

                var result = id == Guid.Empty
                    ? await _context.PessoaJuridica
                        .Where(c => c.CNPJ == cnpj)
                        .FirstOrDefaultAsync()
                    : await _context.PessoaJuridica
                        .Where(c => c.Id != id && c.CNPJ == cnpj)
                        .FirstOrDefaultAsync();

                if (result != null)
                {
                    _logger.LogDebug(
                        "CNPJ encontrado. CNPJ: {CNPJ}, ClienteId: {ClienteId}",
                        cnpj,
                        result.Id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao buscar CNPJ existente. CNPJ: {CNPJ}",
                    cnpj);
                throw;
            }
        }
    }
}
