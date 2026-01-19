using CustomerPlatform.Application.Factories;
using CustomerPlatform.Domain.Interfaces;

namespace CustomerPlatform.Application.Commands.CreateCustomer
{
    public class CreateCustomerHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCustomerHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateCustomerCommand command)
        {
            var customer = CustomerFactory.Criar(command.Request);

            await _unitOfWork.Customers.Criar(customer);
            await _unitOfWork.CommitAsync();

            return customer.Id;
        }
    }

}
