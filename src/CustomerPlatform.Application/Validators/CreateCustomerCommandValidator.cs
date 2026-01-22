using CustomerPlatform.Application.Commands.CreateCustomer;
using CustomerPlatform.Domain.Enums;
using FluentValidation;

namespace CustomerPlatform.Application.Validators
{
    public class CreateCustomerCommandValidator
    : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Email)
                .NotNull();

            RuleFor(x => x.Endereco.Estado)
                .Length(2);

            When(x => x.TipoCliente == TipoCliente.PessoaFisica, () =>
            {
                RuleFor(x => x.Nome)
                    .NotEmpty()
                    .MaximumLength(200);

                RuleFor(x => x.CPF)
                    .NotEmpty()
                    .Length(11);

                RuleFor(x => x.DataNascimento)
                    .NotNull();
            });

            When(x => x.TipoCliente == TipoCliente.PessoaJuridica, () =>
            {
                RuleFor(x => x.RazaoSocial)
                    .NotEmpty()
                    .MaximumLength(200);

                RuleFor(x => x.CNPJ)
                    .NotEmpty()
                    .Length(14);
            });
        }
    }
}
