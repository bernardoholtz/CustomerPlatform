using CustomerPlatform.Domain.Entities;
using CustomerPlatform.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace CustomerPlatform.Application.Services
{
    /// <summary>
    /// Implementação do serviço de validação de documentos
    /// </summary>
    public class DocumentValidationService : IDocumentValidationService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<DocumentValidationService> _logger;

        public DocumentValidationService(
            ICustomerRepository customerRepository,
            ILogger<DocumentValidationService> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }

        public async Task ValidateCpfAsync(string cpf, Guid? excludeCustomerId = null)
        {
            try
            {
                _logger.LogDebug(
                    "Validando CPF. CPF: {CPF}, ExcludeCustomerId: {ExcludeCustomerId}",
                    cpf,
                    excludeCustomerId);

                var cpfExistente = await _customerRepository.BuscaCpfExistente(
                    cpf,
                    excludeCustomerId ?? default);

                if (cpfExistente != null)
                {
                    var mensagem = excludeCustomerId.HasValue
                        ? "Já existe outro Cliente com esse CPF cadastrado."
                        : "Já existe um cliente cadastrado com este CPF.";

                    _logger.LogWarning(
                        "CPF já cadastrado. CPF: {CPF}, ClienteExistenteId: {ClienteExistenteId}, ExcludeCustomerId: {ExcludeCustomerId}",
                        cpf,
                        cpfExistente.Id,
                        excludeCustomerId);

                    throw new InvalidOperationException(mensagem);
                }

                _logger.LogDebug("CPF validado com sucesso. CPF: {CPF}", cpf);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao validar CPF. CPF: {CPF}",
                    cpf);
                throw;
            }
        }

        public async Task ValidateCnpjAsync(string cnpj, Guid? excludeCustomerId = null)
        {
            try
            {
                _logger.LogDebug(
                    "Validando CNPJ. CNPJ: {CNPJ}, ExcludeCustomerId: {ExcludeCustomerId}",
                    cnpj,
                    excludeCustomerId);

                var cnpjExistente = await _customerRepository.BuscaCnpjExistente(
                    cnpj,
                    excludeCustomerId ?? default);

                if (cnpjExistente != null)
                {
                    var mensagem = excludeCustomerId.HasValue
                        ? "Já existe outro Cliente com esse CNPJ cadastrado."
                        : "Já existe um cliente cadastrado com este CNPJ.";

                    _logger.LogWarning(
                        "CNPJ já cadastrado. CNPJ: {CNPJ}, ClienteExistenteId: {ClienteExistenteId}, ExcludeCustomerId: {ExcludeCustomerId}",
                        cnpj,
                        cnpjExistente.Id,
                        excludeCustomerId);

                    throw new InvalidOperationException(mensagem);
                }

                _logger.LogDebug("CNPJ validado com sucesso. CNPJ: {CNPJ}", cnpj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao validar CNPJ. CNPJ: {CNPJ}",
                    cnpj);
                throw;
            }
        }
    }
}
