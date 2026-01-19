using CustomerPlatform.Domain.Entities;
using CustomerPlatform.Domain.Interfaces;

namespace CustomerPlatform.Application.Commands.UpdateCustomer
{
    public class UpdateCustomerHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCustomerHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(UpdateCustomerCommand command)
        {
            var customer = await _unitOfWork.Customers.BuscarPorId(command.Request.Id);

            if (customer == null) {
                throw new Exception("Cliente não encontrado.");
            }
                 
            switch (customer)
            {
                case ClientePessoaFisica pf:
                    // Aqui você tem acesso aos campos de PF
                    pf.Atualizar(command.Request.Nome,
                        command.Request.CPF,
                        command.Request.DataNascimento,
                        command.Request.Email,
                        command.Request.Telefone,
                        command.Request.Endereco);
                    _unitOfWork.Customers.Editar(pf);
                    break;

                case ClientePessoaJuridica pj:
                    // Aqui você tem acesso aos campos de PJ
                    pj.Atualizar(command.Request.RazaoSocial,
                      command.Request.NomeFantasia,
                      command.Request.CNPJ,
                      command.Request.Email,
                      command.Request.Telefone,
                      command.Request.Endereco);
                    _unitOfWork.Customers.Editar(pj);
                    break;
            }
            ;

            await _unitOfWork.CommitAsync();
            return customer.Id;

        }
    }

}
