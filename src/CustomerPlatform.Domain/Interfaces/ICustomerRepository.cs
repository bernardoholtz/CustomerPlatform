using CustomerPlatform.Domain.Entities;

namespace CustomerPlatform.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task <Customer> Criar(Customer customer);
        Task<Customer> Editar(Customer customer);
        Task<Customer> BuscarPorId(Guid id);
        Task<List<Customer>> BuscaFuzzy (string Nome);
        Task<List<Customer>> BuscaDuplicatas(string Nome);
    }
}
