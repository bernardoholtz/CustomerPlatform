using CustomerPlatform.Domain.Enums;
using FluentValidation;

namespace CustomerPlatform.Application.Commands.CreateCustomer
{
    public class CreateCustomerCommandValidator
    : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.Request.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Request.Email)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.Request.Email)
                .NotNull();

            RuleFor(x => x.Request.Endereco.Estado)
                .Length(2);

            When(x => x.Request.TipoCliente == TipoCliente.PessoaFisica, () =>
            {
                RuleFor(x => x.Request.Nome)
                    .NotEmpty()
                    .MaximumLength(200);

                RuleFor(x => x.Request.CPF)
                    .NotEmpty()
                    .Length(11);

                RuleFor(x => x.Request.DataNascimento)
                    .NotNull();
            });

            When(x => x.Request.TipoCliente == TipoCliente.PessoaJuridica, () =>
            {
                RuleFor(x => x.Request.RazaoSocial)
                    .NotEmpty()
                    .MaximumLength(200);

                RuleFor(x => x.Request.CNPJ)
                    .NotEmpty()
                    .Length(14);
            });
        }
    }


}
