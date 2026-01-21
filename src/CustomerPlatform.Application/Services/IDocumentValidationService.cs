using CustomerPlatform.Domain.Entities;

namespace CustomerPlatform.Application.Services
{
    /// <summary>
    /// Serviço para validação de documentos duplicados
    /// </summary>
    public interface IDocumentValidationService
    {
        /// <summary>
        /// Valida se o CPF já existe para outro cliente
        /// </summary>
        Task ValidateCpfAsync(string cpf, Guid? excludeCustomerId = null);

        /// <summary>
        /// Valida se o CNPJ já existe para outro cliente
        /// </summary>
        Task ValidateCnpjAsync(string cnpj, Guid? excludeCustomerId = null);
    }
}
