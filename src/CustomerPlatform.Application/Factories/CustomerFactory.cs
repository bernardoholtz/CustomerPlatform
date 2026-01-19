using CustomerPlatform.Application.DTO;
using CustomerPlatform.Domain.Entities;
using CustomerPlatform.Domain.Enums;
using CustomerPlatform.Domain.ValueObjects;

namespace CustomerPlatform.Application.Factories
{
    public static class CustomerFactory
    {
        public static Customer Criar(CreateCustomerRequest request)
        {
            var endereco = new Endereco(
                request.Endereco.Logradouro,
                request.Endereco.Numero,
                request.Endereco.CEP,
                request.Endereco.Cidade,
                request.Endereco.Estado,
                request.Endereco.Complemento
            );

            return request.TipoCliente switch
            {
                TipoCliente.PessoaFisica =>
                    new ClientePessoaFisica(
                        request.Nome!,
                        request.CPF!,
                        request.DataNascimento!.Value,
                        request.Email,
                        request.Telefone,
                        endereco
                    ),

                TipoCliente.PessoaJuridica =>
                    new ClientePessoaJuridica(
                        request.RazaoSocial!,
                        request.NomeFantasia!,
                        request.CNPJ!,
                        request.Email,
                        request.Telefone,
                        endereco
                    ),

                _ => throw new ArgumentException("Tipo de cliente inválido")
            };
        }

      
    }

}
