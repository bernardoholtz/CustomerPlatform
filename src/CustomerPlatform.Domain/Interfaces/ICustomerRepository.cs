using CustomerPlatform.Domain.Entities;
using CustomerPlatform.Domain.Enums;

namespace CustomerPlatform.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task <Customer> Criar(Customer customer);
        Task<Customer> Editar(Customer customer);
        Task<Customer> BuscarPorId(Guid id);
        Task<List<SuspeitaDuplicidade>> ListaSuspeitosDuplicatas(DateTimeOffset dataIni, DateTimeOffset dataFim);
        Task<ClientePessoaFisica> BuscaCpfExistente(string Cpf, Guid id = default);
        Task<ClientePessoaJuridica> BuscaCnpjExistente(string Cnpj, Guid id = default);
    }
}
