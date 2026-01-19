using CustomerPlatform.Domain.Entities;
using CustomerPlatform.Domain.Interfaces;
using CustomerPlatform.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CustomerPlatform.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext _context;

        public CustomerRepository(CustomerDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> Criar(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            return customer;
        }

        public Task<List<Customer>> BuscaFuzzy(string nome)
        {
            throw new NotImplementedException();
        }

        public Task<List<Customer>> BuscaDuplicatas(string nome)
        {
            throw new NotImplementedException();
        }

        public async Task<Customer> Editar(Customer customer)
        {
            
           _context.Customers.Update(customer);
   
            return customer;
        }

        public async Task<Customer> BuscarPorId(Guid id)
        {
            return await _context.Customers.FindAsync(id);
        }
    }
}
