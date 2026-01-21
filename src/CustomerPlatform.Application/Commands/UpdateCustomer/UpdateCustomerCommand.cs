using CustomerPlatform.Domain.Enums;
using CustomerPlatform.Domain.ValueObjects;
using MediatR;

namespace CustomerPlatform.Application.Commands.UpdateCustomer
{
    public class UpdateCustomerCommand : IRequest<Guid>
    {
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
