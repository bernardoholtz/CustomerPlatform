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


        public async Task<List<SuspeitaDuplicidade>> ListaSuspeitosDuplicatas(DateTimeOffset dataIni, DateTimeOffset dataFim)
        {
            return await _context.SuspeitaDuplicidades
                .Where(s => s.DataDeteccao >= dataIni && s.DataDeteccao <= dataFim)
                .OrderByDescending(s => s.Score)
                .ToListAsync();
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

        public async Task<ClientePessoaFisica> BuscaCpfExistente(string cpf,Guid id)
        {
            return id == Guid.Empty ?
                await _context.PessoaFisica
                        .Where(c => c.CPF == cpf).FirstOrDefaultAsync() :
                await _context.PessoaFisica
                        .Where(c => c.Id != id
                        && c.CPF == cpf).FirstOrDefaultAsync();
        }

        public async Task<ClientePessoaJuridica> BuscaCnpjExistente(string cnpj, Guid id)
        {
            return id == Guid.Empty ?
             await _context.PessoaJuridica
                     .Where(c => c.CNPJ == cnpj).FirstOrDefaultAsync() :
             await _context.PessoaJuridica
                     .Where(c => c.Id != id
                     && c.CNPJ == cnpj).FirstOrDefaultAsync();
        }


    }
}
