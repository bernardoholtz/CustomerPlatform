using CustomerPlatform.Domain.Entities;
using CustomerPlatform.Domain.Interfaces;

namespace CustomerPlatform.Application.Services
{
    /// <summary>
    /// Implementação do serviço de validação de documentos
    /// </summary>
    public class DocumentValidationService : IDocumentValidationService
    {
        private readonly ICustomerRepository _customerRepository;

        public DocumentValidationService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task ValidateCpfAsync(string cpf, Guid? excludeCustomerId = null)
        {
            var cpfExistente = await _customerRepository.BuscaCpfExistente(cpf, excludeCustomerId ?? default);

            if (cpfExistente != null)
            {
                var mensagem = excludeCustomerId.HasValue
                    ? "Já existe outro Cliente com esse CPF cadastrado."
                    : "Já existe um cliente cadastrado com este CPF.";
                
                throw new InvalidOperationException(mensagem);
            }
        }

        public async Task ValidateCnpjAsync(string cnpj, Guid? excludeCustomerId = null)
        {
            var cnpjExistente = await _customerRepository.BuscaCnpjExistente(cnpj, excludeCustomerId ?? default);

            if (cnpjExistente != null)
            {
                var mensagem = excludeCustomerId.HasValue
                    ? "Já existe outro Cliente com esse CNPJ cadastrado."
                    : "Já existe um cliente cadastrado com este CNPJ.";
                
                throw new InvalidOperationException(mensagem);
            }
        }
    }
}
