using CustomerPlatform.Application.Commands.CreateCustomer;
using CustomerPlatform.Domain.Entities;
using CustomerPlatform.Domain.Enums;
using CustomerPlatform.Domain.ValueObjects;

namespace CustomerPlatform.Application.Factories
{
    public static class CustomerFactory
    {
        public static Customer CriarInstancia(CreateCustomerCommand command)
        {
            var endereco = new Endereco(
                command.Endereco.Logradouro,
                command.Endereco.Numero,
                command.Endereco.CEP,
                command.Endereco.Cidade,
                command.Endereco.Estado,
                command.Endereco.Complemento
            );

            return command.TipoCliente switch
            {
                TipoCliente.PessoaFisica =>
                    new ClientePessoaFisica(
                        command.Nome!,
                        command.CPF!,
                        command.DataNascimento!.Value,
                        command.Email,
                        command.Telefone,
                        endereco
                    ),

                TipoCliente.PessoaJuridica =>
                    new ClientePessoaJuridica(
                        command.RazaoSocial!,
                        command.NomeFantasia!,
                        command.CNPJ!,
                        command.Email,
                        command.Telefone,
                        endereco
                    ),

                _ => throw new ArgumentException("Tipo de cliente inválido")
            };
        }

      
    }

}
