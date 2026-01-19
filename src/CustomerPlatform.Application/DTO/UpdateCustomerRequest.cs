using CustomerPlatform.Domain.Enums;
using CustomerPlatform.Domain.ValueObjects;

namespace CustomerPlatform.Application.DTO
{
    public class UpdateCustomerRequest
    {
        // Comuns
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public Endereco Endereco { get; set; }
        public TipoCliente TipoCliente { get; set; }

        // PF
        public string? Nome { get; set; }
        public string? CPF { get; set; }
        public DateTime DataNascimento { get; set; }

        // PJ
        public string? RazaoSocial { get; set; }
        public string? NomeFantasia { get; set; }
        public string? CNPJ { get; set; }
    }
}
